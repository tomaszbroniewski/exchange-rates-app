import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ExchangeRatesTable } from '@vote-app/models';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ExchangeRatesApiService {
  private _baseUrl = `${environment.apiUrl}/exchangerates`;

  constructor(private _http: HttpClient) { }

  listExchangeRatesForLast5Tables(): Observable<ExchangeRatesTable[]> {
    const url = `${this._baseUrl}/`;
    return this._http.get<ExchangeRatesTable[]>(url, {});
  }
}
