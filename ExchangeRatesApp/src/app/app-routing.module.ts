import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './user-account/login/login.component';
import { AuthGuard } from './infrastructure/guards/auth.guard';
import { ListExchangeRatesComponent } from './exchange-rates/list-exchange-rates/list-exchange-rates.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: 'exchange-rates', pathMatch: 'full' },
  { path: 'exchange-rates', component: ListExchangeRatesComponent, canActivate: [AuthGuard], pathMatch: 'full' },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
