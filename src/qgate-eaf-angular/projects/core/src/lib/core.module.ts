import { NgModule } from '@angular/core';
import { CoreComponent } from './core.component';
import { EntityListComponent } from './components/entities/entity-list/entity-list.component';
import {TableModule} from 'primeng/table';
import {BrowserModule} from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [CoreComponent, EntityListComponent],
  imports: [
    HttpClientModule,
    BrowserModule,
    BrowserAnimationsModule,
    TableModule
  ],
  exports: [CoreComponent, EntityListComponent]
})
export class CoreModule { }
