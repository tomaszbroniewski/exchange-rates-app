import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthStorageService {
  private readonly AUTH_KEY = 'isAuthenticated';
  private readonly USERNAME_KEY = 'username';

  username = signal<string | null>(this.getUsername());

  setAuthData(username: string): void {
    sessionStorage.setItem(this.AUTH_KEY, 'true');
    sessionStorage.setItem(this.USERNAME_KEY, username);
    this.username.set(username);
  }

  clearAuthData(): void {
    sessionStorage.removeItem(this.AUTH_KEY);
    sessionStorage.removeItem(this.USERNAME_KEY);
    this.username.set(null);
  }

  isAuthenticated(): boolean {
    return sessionStorage.getItem(this.AUTH_KEY) === 'true';
  }

  getUsername(): string | null {
    return sessionStorage.getItem(this.USERNAME_KEY);
  }
}
