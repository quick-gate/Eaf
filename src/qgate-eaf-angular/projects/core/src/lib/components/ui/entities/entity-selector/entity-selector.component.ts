import { Component, OnInit, Input, Output, EventEmitter, ViewChild, ViewContainerRef, ComponentRef, ComponentFactoryResolver, Inject } from '@angular/core';
import { EntitySelector } from 'projects/core/src/lib/dtos/QGate/Eaf/Domain/Components/Entities/EntitySelector.model';
import { EntityListComponent } from '../../../entities/entity-list/entity-list.component';
import { EntityService } from 'projects/core/src/lib/services/entities/entity.service';

@Component({
  selector: 'eaf-entity-selector',
  templateUrl: './entity-selector.component.html',
  styleUrls: ['./entity-selector.component.css']
})
export class EntitySelectorComponent implements OnInit {
  @ViewChild('entityContainer', { read: ViewContainerRef }) entityContainer: ViewContainerRef;
  entitList: EntityListComponent;
  entityListRef: ComponentRef<EntityListComponent>;
  
  @Input() component: EntitySelector;
  @Input() model: any;
  value: any;
  constructor(private entityService: EntityService, @Inject(ComponentFactoryResolver)
  private componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit() {
    if (!this.model || !this.component || !this.component.DisplayAttributes) {
      return;
    }

    for (const displayAttribute of this.component.DisplayAttributes) {
      this.value = `${this.value? this.value: ''} ${this.model[displayAttribute]}`
    }
  }

  onValueChanged() {
    
  }

  async onShowEntityListClick() {
    const factory = this.componentFactoryResolver.resolveComponentFactory(
      EntityListComponent
    );
    this.entityListRef = this.entityContainer.createComponent(factory);

    var entityList = this.entityListRef.instance;
    entityList.entityName = this.component.EntityName;
    //entityList.onOkClick.subscribe(x => this.onEntityDetailOkClick(x));
  }

  get name(): string {
    return this.component.Binding.PropertyPath.join('.');
  }

}
