import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { TextBox } from '../../../../dtos/QGate/Eaf/Domain/Components/Editors/TextBox.model';

@Component({
  selector: 'eaf-text-box',
  templateUrl: './text-box.component.html',
  styleUrls: ['./text-box.component.css'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: TextBoxComponent, multi: true }
  ]
})
export class TextBoxComponent implements OnInit {
  @Input() model: any;
  @Input() component: TextBox;
  @Output() modelChange = new EventEmitter<string>();
  constructor() { }

  ngOnInit() {

  }

  //TODO move to base class
  get name(): string {
    return this.component.Binding.PropertyPath.join('.');
  }

}
