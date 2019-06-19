import { Component, OnInit, Input } from '@angular/core';
import { Dialog } from 'primeng/dialog';

@Component({
  selector: 'eaf-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})
export class DialogComponent implements OnInit {
  @Input() isVisible = false;
  @Input() title = '';
  constructor() { }

  ngOnInit() {
  }

  dialogShowed(dialog: Dialog) {
    dialog.center();
    dialog.maximize();
  }

}
