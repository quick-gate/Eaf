import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';


@Component({
  selector: 'eaf-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css'],
  providers: [ConfirmationService]
})
export class ConfirmDialogComponent implements OnInit {
  @Input() message: string;
  @Output() closed = new EventEmitter<boolean>();
  constructor(private confirmationService: ConfirmationService) {

  }


  @Input()
  set isVisible(value: boolean) {
    if (!value) {
      return;
    }

    this.confirmationService.confirm({
      message: this.message,
      accept: () => {
        this.closeEventInvoke(true);
      },
      reject: () => {
        this.closeEventInvoke(false);
      }
    });
  }


  ngOnInit() {

  }

  closeEventInvoke(accept: boolean) {
    this.closed.emit(accept);
  }

}
