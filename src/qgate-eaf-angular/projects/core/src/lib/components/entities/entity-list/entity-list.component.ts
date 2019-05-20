import { Component, OnInit, Input, ViewChild, ViewContainerRef, Inject, ComponentFactoryResolver, ComponentRef, Output, EventEmitter } from '@angular/core';
import { EntityService } from '../../../services/entities/entity.service';
import { EntityList } from '../../../dtos/QGate/Eaf/Domain/Components/Entities/EntityList.model';
import { GetEntityListParams } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/Params/GetEntityListParams.model';
import { EntityDetailComponent } from '../entity-detail/entity-detail.component';
import { AttributeValue } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/AttributeValue.model';
import { EntityUiService } from '../../../services/entities/entity-ui.service';
import { Dialog } from 'primeng/dialog';
import { DeleteEntityParams } from '../../../dtos/QGate/Eaf/Domain/Entities/Models/Params/DeleteEntityParams.model';

@Component({
  selector: 'eaf-entity-list',
  templateUrl: './entity-list.component.html',
  styleUrls: ['./entity-list.component.css']
})

export class EntityListComponent implements OnInit {
  @Input() entityName: string;
  @Input() isEmbedded = false;
  @Input() ownerEntity: any;
  @ViewChild('entityDetailContainer', { read: ViewContainerRef }) entityDetailContainer: ViewContainerRef;
  @Output() selectionDone = new EventEmitter<any>();
  @Input() model: EntityList;
  entityDetail: EntityDetailComponent;
  entityDetailRef: ComponentRef<EntityDetailComponent>;
  isSelectionMode = false;
  isDialogVisible = false;
  isConfirmDialogVisible = false;

  //TODO Add to constants - 1
  private selectedEntityIndex = -1;
  selectedEntities: any[];



  constructor(private entityService: EntityService, @Inject(ComponentFactoryResolver)
  private componentFactoryResolver: ComponentFactoryResolver, private entityUiService: EntityUiService) { }

  async ngOnInit() {
    if (this.isEmbedded) {
      return;
    }
    const params = new GetEntityListParams();

    params.EntityName = this.entityName;

    this.model = await this.entityService.getEntityList(params);
  }

  openAsDialog() {
    this.isDialogVisible = true;
  }

  async editCurrentEntity() {
    const currentEntity = this.getCurrentEntity();

    if (!currentEntity) {
      return;
    }


    return this.onEditClick(currentEntity);

  }

  getCurrentEntity(): any {
    if (!this.selectedEntities || this.selectedEntities.length === 0) {
      return null;
    }

    // TODO Add support for multiple row selection
    return this.selectedEntities[0];
  }

  async onEditClick(entity: any) {
    this.setCurrentEntityndex(entity);
    await this.showEntityDetail(this.getEntityKeys(entity));
  }

  getEntityKeys(entity: any): Array<AttributeValue> {
    const keys = new Array<AttributeValue>();
    for (const attribute of this.model.Attributes) {
      if (attribute.IsKey) {
        const keyValue = new AttributeValue();
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

    this.closeEntityDetail();

    const factory = this.componentFactoryResolver.resolveComponentFactory(
      EntityDetailComponent
    );
    this.entityDetailRef = this.entityDetailContainer.createComponent(factory);

    this.entityDetail = this.entityDetailRef.instance;
    this.entityDetail.entityName = this.entityName;
    this.entityDetail.keys = keys;
    this.entityDetail.entityLoaded.subscribe(x => this.onEntityDetailLoaded(x));
    this.entityDetail.okClick.subscribe(x => this.onEntityDetailOkClick(x));
    this.openEntityDetail();
  }

  openEntityDetail() {
    this.entityDetail.open();
  }


  onEntityDetailLoaded(entity: any) {
    // Owner key must by assigned to related entity
    this.entityUiService.fillParentKeysToChild(this.model.RelationAttributes, this.ownerEntity, entity);
  }

  async onEntityDetailOkClick(entityListItem: any) {
    if (this.selectedEntityIndex > -1) {
      this.model.Entities[this.selectedEntityIndex] = entityListItem;
    } else {
      this.model.Entities.push(entityListItem);
    }

    this.closeEntityDetail();
  }

  async deleteCurrentEntity() {
    const currentEntity = this.getCurrentEntity();
    if (!currentEntity) {
      return;
    }

    this.isConfirmDialogVisible = true;
  }

  async onConfirmDialogClose(accept: boolean) {

    if (!accept) {
      return;
    }

    const currentEntity = this.getCurrentEntity();
    if (!currentEntity) {
      return;
    }

    this.setCurrentEntityndex(currentEntity);
    const deleteEntityParams = new DeleteEntityParams();
    deleteEntityParams.EntityName = this.entityName;
    deleteEntityParams.Keys = this.getEntityKeys(currentEntity);

    await this.entityService.deleteEntity(deleteEntityParams);
    this.setCurrentEntityndex(currentEntity);
    this.model.Entities.splice(this.selectedEntityIndex, 1);
  }

  setCurrentEntityndex(entity: any) {
    this.selectedEntityIndex = this.model.Entities.indexOf(entity);
  }



  onRowDblClick(selectedEtityIndex: number, selectedEntity: any) {

    if (this.isSelectionMode) {
      this.selectionDone.emit(selectedEntity);
      return;
    }

    this.onEditClick(selectedEntity);
  }

  closeEntityDetail() {
    if (this.entityDetailRef) {
      this.entityDetail.close();
      this.entityDetailRef.destroy();
    }
  }
}
