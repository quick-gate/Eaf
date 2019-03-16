import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'eaf-text-box',
  templateUrl: './text-box.component.html',
  styleUrls: ['./text-box.component.css'],
  providers: [
    {provide: NG_VALUE_ACCESSOR, useExisting: TextBoxComponent, multi: true}
  ]
})
export class TextBoxComponent implements OnInit {
  @Input() model: any;
  @Output() modelChange = new EventEmitter<string>();
  constructor() { }

  ngOnInit() {
  }

}
