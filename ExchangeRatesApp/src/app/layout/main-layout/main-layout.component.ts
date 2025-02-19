import { Component, computed } from '@angular/core';
import { UserAccountApiService } from '@vote-app/api';
import { AuthStorageService } from 'src/app/infrastructure/services/auth-storage.service';

@Component({
  selector: 'tbr-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss']
})
export class MainLayoutComponent {
  title = "Exchange Rates App";
  username = computed(() => this.authStorage.username());

  constructor(private userAccountApiService: UserAccountApiService, private authStorage: AuthStorageService) {
  }

  logout(): void {
    this.userAccountApiService.logout();
  }
}
