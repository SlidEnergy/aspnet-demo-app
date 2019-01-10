import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TransferComponent } from './transfer/transfer.component';
import { HistoryComponent } from './history/history.component';

@NgModule({
  declarations: [TransferComponent, HistoryComponent],
  imports: [
    CommonModule
  ]
})
export class PaymentsModule { }
