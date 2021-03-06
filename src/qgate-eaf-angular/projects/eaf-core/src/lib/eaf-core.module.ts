import { NgModule } from '@angular/core';
import { FormsModule} from '@angular/forms';
import { EafCoreComponent } from './eaf-core.component';
import { EntityListComponent } from './components/entities/entity-list/entity-list.component';
import {TableModule} from 'primeng/table';
// Browser and Browser animation must be removed here. Because of NullInjection Exception for Zone
// import {BrowserModule} from '@angular/platform-browser';
// import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {CommonModule} from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { EntityMetadataListComponent } from './components/metadatas/entity-metadata-list/entity-metadata-list.component';
import { EntityDetailComponent } from './components/entities/entity-detail/entity-detail.component';
import { TextBoxComponent } from './components/ui/editors/text-box/text-box.component';
import { UiComponentFactoryComponent } from './components/ui/general/ui-component-factory/ui-component-factory.component';
import { ComponentRendererComponent } from './components/ui/general/component-renderer/component-renderer.component';
import { EntityDetailTemplateComponent } from './components/entities/entity-detail/entity-detail-template/entity-detail-template.component';
import { EntitySelectorComponent } from './components/ui/entities/entity-selector/entity-selector.component';
import {DialogModule} from 'primeng/dialog';
import { DialogComponent } from './components/ui/dialogs/dialog/dialog.component';
import { ToolbarComponent } from './components/ui/toolbars/toolbar/toolbar.component';
import {ToolbarModule} from 'primeng/toolbar';
import {ButtonModule} from 'primeng/components/button/button';
import { ToolbarItemGroupComponent } from './components/ui/toolbars/toolbar/toolbar-item-group/toolbar-item-group.component';
import { ButtonComponent } from './components/ui/buttons/button/button.component';
import {InputTextModule} from 'primeng/inputtext';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import { ConfirmDialogComponent } from './components/ui/dialogs/confirm-dialog/confirm-dialog.component';

@NgModule({
  declarations: [EafCoreComponent, EntityListComponent, EntityMetadataListComponent, EntityDetailComponent, TextBoxComponent,
    UiComponentFactoryComponent, ComponentRendererComponent, EntityDetailTemplateComponent, EntitySelectorComponent,
    DialogComponent, ToolbarComponent, ToolbarItemGroupComponent, ButtonComponent, ConfirmDialogComponent],
  imports: [
    HttpClientModule,
    //BrowserModule,
    //BrowserAnimationsModule,
    CommonModule,
    TableModule,
    FormsModule,
    DialogModule,
    ToolbarModule,
    ButtonModule,
    InputTextModule,
    ConfirmDialogModule
  ],
  exports: [EafCoreComponent, EntityListComponent, EntityMetadataListComponent, EntityDetailComponent, TextBoxComponent,
    UiComponentFactoryComponent, ComponentRendererComponent, EntityDetailTemplateComponent, EntitySelectorComponent,
    DialogComponent, ToolbarComponent, ToolbarItemGroupComponent, ButtonComponent, ConfirmDialogComponent],
  entryComponents: [EntityDetailComponent, EntityListComponent]
})
export class EafCoreModule { }


// import { NgModule } from '@angular/core';
// import { EafCoreComponent } from './eaf-core.component';

// @NgModule({
//   declarations: [EafCoreComponent],
//   imports: [
//   ],
//   exports: [EafCoreComponent]
// })
// export class EafCoreModule { }


