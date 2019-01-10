import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { Transaction } from 'src/app/api';
import { MatTable } from '@angular/material';

@Component({
  selector: 'app-transactions-history',
  templateUrl: './transactions-history.component.html',
  styleUrls: ['./transactions-history.component.scss']
})
export class TransactionsHistoryComponent implements OnInit {

  // список транзакций пользователя
  transactions: Transaction[];
  @Input('transactions') set transactionsInput(value: Transaction[]) {
    if (value) {
      this.loadingVisible = false;
      this.transactions = value;
    }
  }
  // ссылка на таблицу, для обновления
  @ViewChild(MatTable) table: MatTable<any>;
  // Список колонок, которые нужно показать в таблице
  columnsToDisplay = ['dateTime', 'correspondentName', 'senderName', 'amount', 'balance'];
  loadingVisible = true;

  constructor() { }

  ngOnInit() {
  }
}
