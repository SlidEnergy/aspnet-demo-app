import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { TransferComponent } from './transfer/transfer.component';
import { TransactionsPageComponent } from './transactions-page/transactions-page.component';
import { TransactionsHistoryComponent } from './transactions-history/transactions-history.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', component: TransactionsPageComponent },
];

@NgModule({
  declarations: [TransferComponent, TransactionsPageComponent, TransactionsHistoryComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes)
  ]
})
export class TransactionsModule { }
