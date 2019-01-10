import { Injectable } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";

import { AuthService } from "./auth.service";
import { map } from "rxjs/operators";

@Injectable()
export class NotLoggedInGuard implements CanActivate {
	constructor(
		private auth: AuthService,
		private router: Router
	) { }

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
		// Проверяет если пользователь авторизован, тогда перенаправляем со страницы входа и авторизации на основную страницу системы
		const returnUrl = route.queryParams['returnUrl'] || '/';

		this.auth.isLoggedIn.subscribe(
			result => {
				if (result) {
					this.router.navigate([returnUrl])
				}
			}
		)

		return this.auth.isLoggedIn.pipe(map(result => !result));
	}
}