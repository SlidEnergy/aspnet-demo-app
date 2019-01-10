import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  isLoggedIn = this.authService.isLoggedIn;
  currentUser = this.authService.currentUser;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
  }

  logout() {
    // выходим и перенаправляем на страницу входа

    this.authService.logout();

    this.router.navigate(['entry/login']);
  }
}
