import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/core/auth.service';
import { MatSnackBar } from '@angular/material';
import { MatProgressButtonOptions } from 'mat-progress-buttons';

@Component({
  selector: 'app-login-component',
  templateUrl: './login-component.component.html',
  styleUrls: ['./login-component.component.scss']
})
export class LoginComponentComponent implements OnInit {
  model: { userName: string, password: string } = { userName: '', password: '' };
  returnUrl = '/';

  // Показываем/скрываем пароль
  hide = true;

  spinnerButtonOptions: MatProgressButtonOptions = {
    active: false,
    text: 'Войти',
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
    // Берем returnUrl
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  login(): void {
    // Исли идет операция входа, выходим из функции
    if (this.spinnerButtonOptions.active)
      return;

    // проверяем входные параметры
    if (!this.validate()) {
      return;
    }

    // Показываем индикатор загрузки
    this.spinnerButtonOptions.active = true;

    this.authService.login(this.model.userName, this.model.password)
      .subscribe(user => {
        // скрываем индикатор загрузки
        this.spinnerButtonOptions.active = false;

        this.snackBar.open('Вы успешно зашли.',
          undefined, { duration: 5000, panelClass: ['background-green'] });
        this.router.navigate([this.returnUrl]);
      }, error => {
        // скрываем индикатор загрузки
        this.spinnerButtonOptions.active = false;

        let message = 'Не удалось войти в систему.';

        // Если ошибка badRequest показывает текст сообщения
        if (error.status && error.status == 400 && error.error && error.error.error_description)
          message += ' ' + error.error.error_description;

        this.snackBar.open(message,
          undefined, { duration: 5000, panelClass: ['background-red'] });
      });
  }

  validate(): boolean {
    // проверяем имя входа и пароль

    if (!this.model.userName || this.model.userName.length <= 0) {
      this.snackBar.open('Имя пользователя не задано.',
        undefined, { duration: 5000, panelClass: ['background-red'] });
      return false;
    }

    if (!this.model.password || this.model.password.length <= 0) {
      this.snackBar.open('Пароль не задан.',
        undefined, { duration: 5000, panelClass: ['background-red'] });
      return false;
    }

    return true;
  }
}
