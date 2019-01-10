import { Optional, SkipSelf, NgModule } from '@angular/core';
import { ApiModule } from '../api';

import { apiConfigFactory } from './api-config-factory';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SharedModule } from '../shared/shared.module';
import { DateInterceptor } from './date-interceptor';
import { AuthService } from './auth.service';
import { AuthGuard } from './auth-guard.service';
import { HeaderComponent } from './header/header.component';

@NgModule({
    imports: [
        HttpClientModule,
        SharedModule,

        SharedModule,
        ApiModule.forRoot(apiConfigFactory),
    ],
    declarations: [
        HeaderComponent
    ],
    providers: [
        AuthService,
        AuthGuard,
        // Регистрируем interceptor, который разбирает строки в виде даты в ответе от сервера и преобразует их в даты.
        {
            provide: HTTP_INTERCEPTORS,
            useClass: DateInterceptor,
            multi: true
        }
    ],
    exports: [
        HeaderComponent
    ]
})
export class CoreModule {
    constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
        if (parentModule) {
            throw new Error(
                'CoreModule is already loaded. Import it in the AppModule only');
        }
    }
}
