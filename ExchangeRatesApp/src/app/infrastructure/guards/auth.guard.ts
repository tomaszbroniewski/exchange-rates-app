import { Injectable } from '@angular/core';
import { CanActivate, GuardResult, MaybeAsync, Router } from '@angular/router';
import { AuthStorageService } from '../services/auth-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private authStorage: AuthStorageService) { }

  canActivate(): MaybeAsync<GuardResult> {
    const isAuthenticated = this.authStorage.isAuthenticated();

    if (!isAuthenticated) {
      this.router.navigate(['/login']);
      return false;
    }
    return true;
  }

}
