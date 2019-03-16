import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CoreModule } from 'projects/core/src/public_api';
import { AppComponent } from './app.component';
import { EntityListComponent } from './components/entity-list/entity-list.component';

@NgModule({
  declarations: [
    AppComponent,
    EntityListComponent
  ],
  imports: [
    BrowserModule,
    CoreModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
