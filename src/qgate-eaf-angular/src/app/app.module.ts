import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EafCoreModule } from '../../projects/eaf-core/src/public-api';
@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    EafCoreModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
