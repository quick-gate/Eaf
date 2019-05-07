import { Component, OnInit, Input } from '@angular/core';
import { ToolbarItemGroupAlign } from './toolbar-item-group-align.enum';

@Component({
  selector: 'eaf-toolbar-item-group',
  templateUrl: './toolbar-item-group.component.html',
  styleUrls: ['./toolbar-item-group.component.css']
})
export class ToolbarItemGroupComponent implements OnInit {
  @Input() groupAlign = ToolbarItemGroupAlign.left;

  constructor() { }

  ngOnInit() {
  }

  get className(): string {
    const leftAlign = 'ui-toolbar-group-left';
    switch (this.groupAlign) {
      case ToolbarItemGroupAlign.left:
      return leftAlign;
      case ToolbarItemGroupAlign.right:
      return 'ui-toolbar-group-right';
    }

    return leftAlign;
}

}
