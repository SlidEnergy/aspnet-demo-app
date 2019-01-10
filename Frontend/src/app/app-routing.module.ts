import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotLoggedInGuard } from './core/not-logged-in-guard.service';
import { AuthGuard } from './core/auth-guard.service';

const routes: Routes = [
  { path: '', redirectTo: 'transactions', pathMatch: 'full' },
  { path: "entry", loadChildren: './entry/entry.module#EntryModule', canActivate: [NotLoggedInGuard] },
  { path: "transactions", loadChildren: './transactions/transactions.module#TransactionsModule', canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
