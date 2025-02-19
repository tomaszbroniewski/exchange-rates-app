import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA, Injector } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from './shared/shared.module';
import { ErrorInterceptor } from './infrastructure/interceptors/error-interceptor';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { ExchangeRatesApiService } from './api';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './user-account/login/login.component';
import { ListExchangeRatesComponent } from './exchange-rates/list-exchange-rates/list-exchange-rates.component';
import { CredentialsInterceptor } from './infrastructure/interceptors/credentials-interceptor';

@NgModule({
    declarations: [
        AppComponent,
        MainLayoutComponent,
        ListExchangeRatesComponent,
        LoginComponent
    ],
    bootstrap: [AppComponent],
    schemas: [CUSTOM_ELEMENTS_SCHEMA],
    imports: [
        CommonModule,
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        FormsModule,
        ReactiveFormsModule,
        SharedModule,
        MatTableModule,
        MatButtonModule,
        MatIconModule,
        MatDialogModule,
        MatSelectModule,
        MatFormFieldModule,
        MatInputModule,
        MatCardModule
    ],
    providers: [
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: CredentialsInterceptor, multi: true },
        ExchangeRatesApiService,
        provideHttpClient(withInterceptorsFromDi())
    ]
})

export class AppModule {
    static injector: Injector;

    constructor(injector: Injector) {
        AppModule.injector = injector;
    }
}