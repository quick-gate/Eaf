import { Component, OnInit, Input, ViewChild, TemplateRef, ContentChild, AfterContentInit, Output, EventEmitter } from '@angular/core';
import { AttributeValue } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/AttributeValue.model';
import { EntityService } from '../../../services/entities/entity.service';
import { GetEntityDetailParams } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/Params/GetEntityDetailParams.model';
import { EntityDetail } from '../../../dtos/QGate/Eaf/Domain/Components/Entities/EntityDetail.model';
import { SaveEntityParams } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/Params/SaveEntityParams.model';
import { ComponentType } from '../../../dtos/QGate/Eaf/Domain/Components/General/ComponentType.enum';
import { EntitySelector } from '../../../dtos/QGate/Eaf/Domain/Components/Entities/EntitySelector.model';
import { SaveEntityResult } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/Params/SaveEntityResult.model';

@Component({
  selector: 'eaf-entity-detail',
  templateUrl: './entity-detail.component.html',
  styleUrls: ['./entity-detail.component.css']
})
export class EntityDetailComponent implements OnInit {
  @ContentChild(TemplateRef, {static: false}) customTemplate: TemplateRef<any>;
  customTemplateContext: any;
  @Input() entityName: string;
  @Input() autogenerate = true;
  @Input() keys: AttributeValue[];
  @Output() okClick = new EventEmitter<any>();
  @Output() entityLoaded = new EventEmitter<any>();

  model: EntityDetail;
  isNew = false;
  isVisible = false;

  constructor(private entityService: EntityService) { }

  async ngOnInit() {
    const params = new GetEntityDetailParams();
    params.EntityName = this.entityName;
    params.Keys = this.keys;
    params.IncludePropertyPaths = ['*'];


    this.model = await this.entityService.getEntityDetail(params);

    this.customTemplateContext = { $implicit: this.model };

    this.entityLoaded.emit(this.model.Entity);
  }

  async okClickInternal() {

    const result = await this.saveEntity();

    this.okClick.emit(result.EntityLisItem);
  }

  open() {
    this.openClose(true);
  }

  close() {
    this.openClose(false);
  }

  private openClose(open: boolean) {
    this.isVisible = open;
  }

  async saveEntity(): Promise<SaveEntityResult> {
    const params = new SaveEntityParams();
    params.Entity = this.model.Entity;
    params.EntityName = this.entityName;
    params.IsFillEntityListItemRequired = true;

    if (this.isNewEntity()) {
      params.IsNew = true;
    }

    for (const component of this.model.Components) {
      if (component.Type === ComponentType.EntitySelector) {
        const entitySelector = <EntitySelector>component;
        if (entitySelector.IsComposition) {
          continue;
        }

        //TODO refresh property owner
        //TODO improve path
        params.Entity[component.Binding.PropertyPath[0]] = null;
      }
    }

    return this.entityService.saveEntity(params);

  }

  isNewEntity(): boolean {
    return !this.keys;
  }
}
