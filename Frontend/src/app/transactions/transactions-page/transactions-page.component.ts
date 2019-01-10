import { Component, OnInit } from '@angular/core';
import { Transaction, TransactionsService } from 'src/app/api';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-transactions-page',
  templateUrl: './transactions-page.component.html',
  styleUrls: ['./transactions-page.component.scss']
})
export class TransactionsPageComponent implements OnInit {
  transactions: Observable<Transaction[]>;

  constructor(private transactionsService: TransactionsService) { }

  ngOnInit() {
    this.update();
  }

  // обновляем список транзакций при инициализации и после платежа
  update() {
    this.transactions = this.transactionsService.getTransactions();
  }
}
