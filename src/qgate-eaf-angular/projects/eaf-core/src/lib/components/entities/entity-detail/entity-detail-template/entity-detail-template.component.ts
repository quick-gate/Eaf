import { OnInit, ViewChild, TemplateRef, Component } from '@angular/core';

@Component({
    selector: 'eaf-entity-detail-template',
    template: './entity-detail-template.component.html'
  })
export class EntityDetailTemplateComponent implements OnInit {
  @ViewChild(TemplateRef, {static: false}) template: TemplateRef<any>;
  constructor() { }

  ngOnInit() {
  }



}
