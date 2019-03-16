import { NgModule } from '@angular/core';
import { FormsModule} from '@angular/forms';
import { CoreComponent } from './core.component';
import { EntityListComponent } from './components/entities/entity-list/entity-list.component';
import {TableModule} from 'primeng/table';
import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { EntityMetadataListComponent } from './components/metadatas/entity-metadata-list/entity-metadata-list.component';
import { EntityDetailComponent } from './components/entities/entity-detail/entity-detail.component';
import { TextBoxComponent } from './components/ui-components/editors/text-box/text-box.component';
import { UiComponentFactoryComponent } from './components/ui-components/general/ui-component-factory/ui-component-factory.component';
import { ComponentRendererComponent } from './components/ui-components/general/component-renderer/component-renderer.component';
import { EntityDetailTemplateComponent } from './components/entities/entity-detail/entity-detail-template/entity-detail-template.component';

@NgModule({
  declarations: [CoreComponent, EntityListComponent, EntityMetadataListComponent, EntityDetailComponent, TextBoxComponent, UiComponentFactoryComponent, ComponentRendererComponent, EntityDetailTemplateComponent],
  imports: [
    HttpClientModule,
    BrowserModule,
    BrowserAnimationsModule,
    TableModule,
    FormsModule
  ],
  exports: [CoreComponent, EntityListComponent, EntityMetadataListComponent, EntityDetailComponent, TextBoxComponent, EntityDetailTemplateComponent]
})
export class CoreModule { }
