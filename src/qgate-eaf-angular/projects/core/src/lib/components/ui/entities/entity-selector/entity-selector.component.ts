import { Component, OnInit, Input, Output, EventEmitter, ViewChild, ViewContainerRef, ComponentRef, ComponentFactoryResolver, Inject } from '@angular/core';
import { EntitySelector } from 'projects/core/src/lib/dtos/QGate/Eaf/Domain/Components/Entities/EntitySelector.model';
import { EntityListComponent } from '../../../entities/entity-list/entity-list.component';
import { EntityService } from 'projects/core/src/lib/services/entities/entity.service';
import { EntityDetailComponent } from '../../../entities/entity-detail/entity-detail.component';
import { AttributeValue } from 'projects/core/src/lib/dtos/QGate/Eaf/Domain/Entities/Models/AttributeValue.model';
import { EntityUiService } from 'projects/core/src/lib/services/entities/entity-ui.service';

@Component({
  selector: 'eaf-entity-selector',
  templateUrl: './entity-selector.component.html',
  styleUrls: ['./entity-selector.component.css']
})
export class EntitySelectorComponent implements OnInit {

  @ViewChild('entityContainer', { read: ViewContainerRef }) entityContainer: ViewContainerRef;
  entityList: EntityListComponent;
  entityListRef: ComponentRef<EntityListComponent>;

  entityDetail: EntityDetailComponent;
  entityDetailRef: ComponentRef<EntityDetailComponent>;

  @Input() component: EntitySelector;
  @Input() model: any;
  @Input() ownerEntity: any;

  value: any;
  constructor(private entityService: EntityService, @Inject(ComponentFactoryResolver)
  private componentFactoryResolver: ComponentFactoryResolver, private entityUiService: EntityUiService) { }

  ngOnInit() {
    this.fillEntity();
  }

  fillEntity() {
    if (!this.model || !this.component || !this.component.DisplayAttributes) {
      return;
    }

    this.value = '';

    for (const displayAttribute of this.component.DisplayAttributes) {
      this.value = `${this.value ? this.value : ''} ${this.model[displayAttribute]}`;
    }
  }

  onValueChanged() {

  }

  async onShowEntityListClick() {

    if (this.component.IsInverted && this.component.RelationAttributes && this.ownerEntity) {
      for (const relationAttribute of this.component.RelationAttributes) {
        if (!this.ownerEntity[relationAttribute.Attribute]) {
          alert(`You must save new record before set ${this.component.Caption} `);
          return;
        }
      }
    }

    if (this.component.IsComposition) {
      const detailFactory = this.componentFactoryResolver.resolveComponentFactory(
        EntityDetailComponent
      );
      this.entityDetailRef = this.entityContainer.createComponent(detailFactory);

      this.entityDetail = this.entityDetailRef.instance;
      this.entityDetail.entityName = this.component.EntityName;

      if (this.model && this.component.RelationAttributes) {
        this.entityDetail.keys = new Array<AttributeValue>();
        for (const attribute of this.component.RelationAttributes) {
          const attributeValue = new AttributeValue();
          attributeValue.Name = attribute.Attribute;
          attributeValue.Value = this.model[attribute.Attribute];
          this.entityDetail.keys.push(attributeValue);
        }
      }
      this.entityDetail.entityLoaded.subscribe(x => this.onEntityDetailLoaded(x));
      this.entityDetail.okClick.subscribe(x => this.onSelectionDone(x));
      return;
    }

    const factory = this.componentFactoryResolver.resolveComponentFactory(
      EntityListComponent
    );
    this.entityListRef = this.entityContainer.createComponent(factory);

    this.entityList = this.entityListRef.instance;
    this.entityList.entityName = this.component.EntityName;
    this.entityList.isSelectionMode = true;
    this.entityList.selectionDone.subscribe(x => this.onSelectionDone(x));
    // entityList.onOkClick.subscribe(x => this.onEntityDetailOkClick(x));
  }

  get name(): string {
    return this.component.Binding.PropertyPath.join('.');
  }

  onEntityDetailLoaded(entity: any) {
    // Is Inverted OneToOne relation. Owner key must by assigned to related entity
    if (this.component.IsInverted) {
      this.entityUiService.fillParentKeysToChild(this.component.RelationAttributes, this.ownerEntity, entity);
    }
  }

  onSelectionDone(entity: any) {

    this.model = entity;
    this.fillEntity();

    if (this.component.IsInverted) {
      this.entityDetailRef.destroy();
      return;
    }

    // Fill related entity keys to owner attributes
    if (entity && this.component.RelationAttributes && this.ownerEntity) {
      for (const relationAttribute of this.component.RelationAttributes) {
        this.ownerEntity[relationAttribute.Attribute] = entity[relationAttribute.LinkedAttribute];
      }

      this.model = entity;
    }



    this.entityListRef.destroy();
  }


}
