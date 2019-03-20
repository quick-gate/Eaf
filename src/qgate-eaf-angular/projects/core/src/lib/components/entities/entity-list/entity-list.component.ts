import { Component, OnInit, Input, ViewChild, ViewContainerRef, Inject, ComponentFactoryResolver, ComponentRef } from '@angular/core';
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
  @Input() entityName: string;
  @ViewChild('entityDetailContainer', { read: ViewContainerRef }) entityDetailContainer: ViewContainerRef;

  model: EntityList;
  entityDetail: EntityDetailComponent;
  entityDetailRef: ComponentRef<EntityDetailComponent>;
  //TODO Add to constants - 1
  private selectedEntityIndex = -1;
  private selectedEntities: any[];



  constructor(private entityService: EntityService, @Inject(ComponentFactoryResolver)
  private componentFactoryResolver: ComponentFactoryResolver) { }

  async ngOnInit() {

    const params = new GetEntityListParams();

    params.EntityName = this.entityName;

    this.model = await this.entityService.GetEntityList(params);
  }

  async onEditClick(entity: any) {
    this.selectedEntityIndex = this.model.Entities.indexOf(entity);
    console.log(this.selectedEntityIndex);
    await this.showEntityDetail(this.getEntityKeys(entity));
  }

  getEntityKeys(entity: any): Array<AttributeValue> {
    var keys = new Array<AttributeValue>();
    for (const attribute of this.model.Attributes) {
      if (attribute.IsKey) {
        var keyValue = new AttributeValue();
        keyValue.Name = attribute.Name;
        keyValue.Value = entity[attribute.Name];

        keys.push(keyValue);
      }
    }

    return keys;
  }

  async onAddClick() {
    this.selectedEntityIndex = -1;
    await this.showEntityDetail(null);
  }

  async showEntityDetail(keys: Array<AttributeValue>) {
    const factory = this.componentFactoryResolver.resolveComponentFactory(
      EntityDetailComponent
    );
    this.entityDetailRef = this.entityDetailContainer.createComponent(factory);

    var entityDetail = this.entityDetailRef.instance;
    entityDetail.entityName = this.entityName;
    entityDetail.keys = keys;
    entityDetail.onOkClick.subscribe(x => this.onEntityDetailOkClick(x));
  }

  async onEntityDetailOkClick(entityListItem: any) {
    if (this.selectedEntityIndex > -1) {
      this.model.Entities[this.selectedEntityIndex] = entityListItem;
    } else  {
      this.model.Entities.push(entityListItem);
    }

    this.entityDetailRef.destroy();
  }


  onRowDblClick(selectedEtityIndex: number, selectedEntity:any){
    console.log(selectedEtityIndex);
    console.log(selectedEntity);
  }



}
