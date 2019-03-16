import { Component, OnInit } from '@angular/core';
import { AttributeValue } from 'projects/core/src/lib/dtos/QGate/Eaf/Domain/Entities/Models/AttributeValue.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  ngOnInit(): void {
    //this.keys = [{Name:"Id", Value:"27"}]
  }
  title = 'qgate-eaf-angular';
  entityName = "QGate.Erp.Domain.Models.Products.Product, QGate.Erp.Domain";
  keys: AttributeValue[] = [{ Name: "Id", Value: "27" }];
}
