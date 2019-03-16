import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { EntityService } from '../../../services/entities/entity.service';
import { EntityList } from '../../../dtos/QGate/Eaf/Domain/Components/Entities/EntityList.model';
import { GetEntityListParams } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/Params/GetEntityListParams.model';
import { EntityDetailComponent } from '../entity-detail/entity-detail.component';
import { AttributeValue } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/AttributeValue.model';

@Component({
  selector: 'eaf-entity-list',
  templateUrl: './entity-list.component.html',
  styleUrls: ['./entity-list.component.css']
})
export class EntityListComponent implements OnInit {
  model: EntityList;
  isListVisible = false;
  isDetailVisible = false;
  @Input() entityName: string;
  @ViewChild(EntityDetailComponent) entityDetail: EntityDetailComponent;

  constructor(private entityService: EntityService) { }

  async ngOnInit() {

    const params = new GetEntityListParams();

    params.EntityName = this.entityName;

    this.model = await this.entityService.GetEntityList(params);

    if (this.model) {
      this.isListVisible = true;
    }

  }

  async showEntityDetail(entity: any) {
    this.isListVisible = false;
    this.isDetailVisible = true;

    var keys = new Array<AttributeValue>();
    for (const attribute of this.model.Attributes) {
      if (attribute.IsKey) {
        var keyValue = new AttributeValue();
        keyValue.Name = attribute.Name;
        keyValue.Value = entity[attribute.Name];

        keys.push(keyValue);
      }

    }

    this.entityDetail.keys = keys;
    this.entityDetail.entityName = this.entityName;
    this.entityDetail.isVisible = this.isDetailVisible;
    await this.entityDetail.show();
  }



}
