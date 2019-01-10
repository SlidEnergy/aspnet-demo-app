import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

import { AuthService } from './auth.service';

@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild {
	constructor(
		private auth: AuthService,
		private router: Router
	) { }

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
		// Разрешаем маршрут если пользователь авторизован, иначе отправляем на страницу входа
		this.auth.isLoggedIn.subscribe(
			result => {
				if (!result) {
					this.router.navigate(['entry/login'], { queryParams: { returnUrl: state.url } });
				}
			}
		);

		return this.auth.isLoggedIn;
	}

	canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
		// Разрешаем маршрут если пользователь авторизован, иначе отправляем на страницу входа
		this.auth.isLoggedIn.subscribe(
			result => {
				if (!result) {
					this.router.navigate(['entry/login'], { queryParams: { returnUrl: state.url } });
				}
			}
		);

		return this.auth.isLoggedIn;
	}
}
