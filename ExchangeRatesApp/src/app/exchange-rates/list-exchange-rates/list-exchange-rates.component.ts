import { ChangeDetectionStrategy, Component, OnDestroy, OnInit, signal } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ExchangeRatesApiService } from '@vote-app/api';
import { ExchangeRate, ExchangeRatesTable } from '@vote-app/models';
import { Subscriptions } from 'src/app/shared/utils/subscriptions';

@Component({
  selector: 'tbr-list-exchange-rates',
  templateUrl: './list-exchange-rates.component.html',
  styleUrls: ['./list-exchange-rates.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ListExchangeRatesComponent implements OnInit, OnDestroy {
  tables = signal<ExchangeRatesTable[]>([]);
  selectedTableIndex = signal<number>(0);
  displayedColumns: string[] = ['currency', 'code', 'mid'];
  dataSource = new MatTableDataSource<ExchangeRate>([]);

  private _subscripions = new Subscriptions();

  constructor(private exchangeRatesApiService: ExchangeRatesApiService) { }

  ngOnInit() {
    this.fetchExchangeRates();
  }

  fetchExchangeRates() {
    this.exchangeRatesApiService.listExchangeRatesForLast5Tables()
      .subscribe(response => {
        this.tables.set(response);
        if (response.length > 0) {
          this.updateTable(0);
        }
      });
  }

  updateTable(index: number) {
    this.selectedTableIndex.set(index);
    const selectedRates = this.tables()?.[index]?.rates || [];
    this.dataSource.data = selectedRates;
  }

  ngOnDestroy(): void {
    this._subscripions.unsubscribe();
  }
}
