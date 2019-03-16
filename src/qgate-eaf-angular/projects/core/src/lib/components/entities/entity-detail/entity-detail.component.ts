import { Component, OnInit, Input, ViewChild, TemplateRef, ContentChild, AfterContentInit } from '@angular/core';
import { keyframes } from '@angular/animations';
import { AttributeValue } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/AttributeValue.model';
import { EntityService } from '../../../services/entities/entity.service';
import { GetEntityDetailParams } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/Params/GetEntityDetailParams.model';
import { EntityDetail } from '../../../dtos/QGate/Eaf/Domain/Components/Entities/EntityDetail.model';
import { SaveEntityParams } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/Params/SaveEntityParams.model';
import { EntityDetailTemplateComponent } from './entity-detail-template/entity-detail-template.component';

@Component({
  selector: 'eaf-entity-detail',
  templateUrl: './entity-detail.component.html',
  styleUrls: ['./entity-detail.component.css']
})
export class EntityDetailComponent implements OnInit  {
  @ContentChild(TemplateRef) customTemplate: TemplateRef<any>;
  customTemplateContext: any;
  @Input() entityName: string;
  @Input() autogenerate = true;
  isVisible = false;
  @Input() keys: AttributeValue[];
  model: EntityDetail;

  constructor(private entityService: EntityService) { }

  async ngOnInit() {
    if(this.keys){
      await this.show();
    }
    
  }

  async show() {
    var params = new GetEntityDetailParams();
    params.EntityName = this.entityName;
    params.Keys = this.keys;

    this.model = await this.entityService.GetEntityDetail(params);

    this.customTemplateContext  = {$implicit: this.model};
  }

  async onOkClick(){
    var params = new SaveEntityParams();
    params.Entity = this.model.Entity;
    params.EntityName = this.entityName;
    await this.entityService.SaveEntity(params);
    alert("Saved");
  }
}
