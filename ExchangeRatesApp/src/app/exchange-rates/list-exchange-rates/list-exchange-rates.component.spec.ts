import { TestBed } from '@angular/core/testing';
import { ListExchangeRatesComponent } from './list-exchange-rates.component';
import { AppModule } from 'src/app/app.module';

describe('VoteMainView', () => {

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
    }).compileComponents();
  });

  it('should be created', () => {
    const fixture = TestBed.createComponent(ListExchangeRatesComponent);
    const comp = fixture.componentInstance;
    expect(comp).toBeTruthy();
  });
});
