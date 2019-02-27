import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'eaf-entity-list',
  templateUrl: './entity-list.component.html',
  styleUrls: ['./entity-list.component.css']
})
export class EntityListComponent implements OnInit {
  model: any[];

  attributes: any[];

  constructor() { }

  ngOnInit() {
    this.model = [{
      Code: 'Product 1',
      Id: 555
    },
    {
      Code: 'Product 2',
      Id: 123
    }
    ];

    this.attributes = [
      { Name: 'Code', Caption: 'Kód' },
      { Name: 'Id', Caption: 'Id' }
    ];
  }

}
