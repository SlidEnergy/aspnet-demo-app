import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { AuthService } from 'src/app/core/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatProgressButtonOptions } from 'mat-progress-buttons';

@Component({
  selector: 'app-registration-component',
  templateUrl: './registration-component.component.html',
  styleUrls: ['./registration-component.component.scss']
})
export class RegistrationComponentComponent implements OnInit {
  model: { userName: string, email: string, password: string, confirmPassword: string } = { userName: '', email: '', password: '', confirmPassword: '' };

  // показываем/скрываем пароль
  hide = true;
  hide2 = true;

  spinnerButtonOptions: MatProgressButtonOptions = {
    active: false,
    text: 'Регистрация',
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
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar,
    private authService: AuthService
  ) { }

  ngOnInit() {
  }

  register() {
    // Выходим из функции если уже запущена операция регистрации
    if (this.spinnerButtonOptions.active)
      return;

    // проверяем входные параметры
    if (!this.validate()) {
      return;
    }

    // показываем индикатор загрузки
    this.spinnerButtonOptions.active = true;

    this.authService.register(this.model.userName, this.model.email, this.model.password, this.model.confirmPassword)
      .subscribe(user => {
        // прячем индикатор загрузки
        this.spinnerButtonOptions.active = false;

        this.snackBar.open('Пользователь зарегестрирован.',
          undefined, { duration: 5000, panelClass: ['background-green'] });
        this.router.navigate(["entry/login"]);
      }, error => {
        // прячем индикатор загрузки
        this.spinnerButtonOptions.active = false;

        let message = 'Не удалось зарегестрироваться в системе.';

        // если получена ошибка от сервера badrequest показываем сообщение от сервера
        if (error.status && error.status == 400 && error.error && error.error.modelState && error.error.modelState[''])
          message += ' ' + error.error.modelState[''].join(' ');

        this.snackBar.open(message,
          undefined, { duration: 5000, panelClass: ['background-red'] });
      });
  }

  validate(): boolean {
    // проверяем имя пользователя, email и пароль
    if (!this.model.userName || this.model.userName.length <= 0) {
      this.snackBar.open('Имя пользователя не задано.',
        undefined, { duration: 5000, panelClass: ['background-red'] });
      return false;
    }

    if (!this.model.email || this.model.email.length <= 0) {
      this.snackBar.open('Email не задан.',
        undefined, { duration: 5000, panelClass: ['background-red'] });
      return false;
    }

    if (!this.model.password || this.model.password.length <= 0) {
      this.snackBar.open('Пароль не задан.',
        undefined, { duration: 5000, panelClass: ['background-red'] });
      return false;
    }

    if (this.model.password != this.model.confirmPassword) {
      this.snackBar.open('Пароль не совпадает.',
        undefined, { duration: 5000, panelClass: ['background-red'] });
      return false;
    }

    return true;
  }
}
