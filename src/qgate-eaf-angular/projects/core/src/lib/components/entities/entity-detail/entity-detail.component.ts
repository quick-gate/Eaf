import { Component, OnInit, Input, ViewChild, TemplateRef, ContentChild, AfterContentInit, Output, EventEmitter } from '@angular/core';
import { AttributeValue } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/AttributeValue.model';
import { EntityService } from '../../../services/entities/entity.service';
import { GetEntityDetailParams } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/Params/GetEntityDetailParams.model';
import { EntityDetail } from '../../../dtos/QGate/Eaf/Domain/Components/Entities/EntityDetail.model';
import { SaveEntityParams } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/Params/SaveEntityParams.model';

@Component({
  selector: 'eaf-entity-detail',
  templateUrl: './entity-detail.component.html',
  styleUrls: ['./entity-detail.component.css']
})
export class EntityDetailComponent implements OnInit {
  @ContentChild(TemplateRef) customTemplate: TemplateRef<any>;
  customTemplateContext: any;
  @Input() entityName: string;
  @Input() autogenerate = true;
  @Input() keys: AttributeValue[];
  @Output() onOkClick = new EventEmitter<any>();

  model: EntityDetail;


  constructor(private entityService: EntityService) { }

  async ngOnInit() {
    const params = new GetEntityDetailParams();
    params.EntityName = this.entityName;
    params.Keys = this.keys;
    params.IncludePropertyPaths = ['Description', 'Description.Translations'];


    this.model = await this.entityService.GetEntityDetail(params);

    this.customTemplateContext = { $implicit: this.model };
  }

  async onOkClickInternal() {
    const params = new SaveEntityParams();
    params.Entity = this.model.Entity;
    params.EntityName = this.entityName;
    params.IsFillEntityListItemRequired = true;

    if (this.isNewEntity()) {
      params.IsNew = true;
    }

    const result = await this.entityService.SaveEntity(params);

    this.onOkClick.emit(result.EntityLisItem);
  }

  isNewEntity(): boolean {
    return !this.keys;
  }
}
