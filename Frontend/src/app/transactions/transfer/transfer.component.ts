import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { TransactionsService, UsersService } from 'src/app/api';
import { MatSnackBar } from '@angular/material';

import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith, switchMap, filter, combineLatest } from 'rxjs/operators';
import { AuthService } from 'src/app/core/auth.service';
import { MatProgressButtonOptions } from 'mat-progress-buttons';

@Component({
  selector: 'app-transfer',
  templateUrl: './transfer.component.html',
  styleUrls: ['./transfer.component.scss']
})
export class TransferComponent implements OnInit {
  model: { recipient: string, amount: number } = { recipient: '', amount: 0 };

  formControl = new FormControl();

  // список всех пользователей в системе
  users: Observable<string[]>;

  // отфильтрованный список после ввода пользователя
  filteredOptions: Observable<string[]>;

  // платеж отправлен, нужно обновить список транзакций
  @Output() onSent = new EventEmitter();

  spinnerButtonOptions: MatProgressButtonOptions = {
    active: false,
    text: 'Отправить',
    spinnerSize: 18,
    raised: true,
    stroked: false,
    buttonColor: 'primary',
    spinnerColor: 'accent',
    fullWidth: false,
    disabled: false,
    mode: 'indeterminate'
  }

  constructor(
    private transactionsService: TransactionsService,
    private usersService: UsersService,
    private snackBar: MatSnackBar,
    private authService: AuthService
  ) { }

  ngOnInit() {
    // фильтруем список после ввода пользователем
    this.filteredOptions = this.formControl.valueChanges
      .pipe(
        startWith(''),
        // информация о текущем пользователе, для исключения его из списка
        combineLatest(this.authService.currentUser),
        switchMap(([value, currentUser]) => this.users.pipe(map(users =>
          users.filter(user => user.toLowerCase().includes(value.toLowerCase()) && user !== currentUser.userName)
        )))
      );

    // получаем список всех пользователей
    this.users = this.usersService.getList();
  }

  onSelectionChanged(event$) {
    // запоминаем выбранного пользователя
    this.model.recipient = event$.option.value;
  }

  send() {
    // если операция уже выполняется выходим из функции
    if (this.spinnerButtonOptions.active)
      return;

    // проверяем входные данные
    if (!this.validate()) {
      return;
    }

    // показываем индикатор загрузки
    this.spinnerButtonOptions.active = true;

    this.transactionsService.postTransaction({ userName: this.model.recipient, amount: this.model.amount })
      .subscribe(user => {
        // скрываем индикатор загрузки
        this.spinnerButtonOptions.active = false;

        // обновляем информацию пользователя
        this.authService.refreshUserInfo();
        this.snackBar.open('PW успешно отправлены.',
          undefined, { duration: 5000, panelClass: ['background-green'] });
        this.onSent.emit();
      }, error => {
        // скрываем индикатор загрузки
        this.spinnerButtonOptions.active = false;

        let message = 'Не удалось отправить PW.';

        if (error.status && error.status == 404)
          message += ' ' + ' Пользователь не найден';

        if (error.status && error.status == 400)
          message += ' ' + ' Данные для перевода заданы не верно';

        this.snackBar.open(message,
          undefined, { duration: 5000, panelClass: ['background-red'] });
      });
  }

  validate(): boolean {
    // проверяем имя пользователя и количество

    if (!this.model.recipient || this.model.recipient.length <= 0) {
      this.snackBar.open('Имя пользователя не задано.',
        undefined, { duration: 5000, panelClass: ['background-red'] });
      return false;
    }

    if (!this.model.amount || this.model.amount <= 0) {
      this.snackBar.open('Баланс задан не верно.',
        undefined, { duration: 5000, panelClass: ['background-red'] });
      return false;
    }

    return true;
  }
}
