import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { LoginToUserAccountResponse } from '@vote-app/models';
import { Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthStorageService } from '../infrastructure/services/auth-storage.service';

@Injectable({
  providedIn: 'root'
})
export class UserAccountApiService {
  private _baseUrl = `${environment.apiUrl}/useraccount`;

  constructor(private _http: HttpClient, private _router: Router, private _authStorage: AuthStorageService) {
  }

  login(credentials: { username: string; password: string }): Observable<LoginToUserAccountResponse> {
    return this._http.post<LoginToUserAccountResponse>(`${this._baseUrl}/login`, credentials).pipe(
      tap((response) => {
        this._authStorage.setAuthData(response.username);
      })
    );
  }

  logout(): void {
    this._http.post<void>(`${this._baseUrl}/logout`, {}).subscribe(
      () => {
        this._authStorage.clearAuthData();
        this._router.navigate(['/login']);
      });
  }
}
