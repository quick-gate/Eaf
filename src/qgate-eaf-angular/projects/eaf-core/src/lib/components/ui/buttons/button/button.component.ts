import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'eaf-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.css']
})
export class ButtonComponent implements OnInit {
  @Input() icon: string;
  @Input() caption: string;
  constructor() { }

  ngOnInit() {
  }

}
