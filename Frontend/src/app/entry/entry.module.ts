import { NgModule } from '@angular/core';
import { RegistrationComponentComponent } from './registration-component/registration-component.component';
import { LoginComponentComponent } from './login-component/login-component.component';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../shared/shared.module';

const routes: Routes = [
    { path: 'login', component: LoginComponentComponent },
    { path: 'register', component: RegistrationComponentComponent },
];


@NgModule({
    imports: [
        SharedModule,
        RouterModule.forChild(routes)
    ],
    declarations: [
        RegistrationComponentComponent,
        LoginComponentComponent
    ]
})
export class EntryModule { }
