import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserAccountApiService } from '@vote-app/api';
import { AuthStorageService } from 'src/app/infrastructure/services/auth-storage.service';

@Component({
  selector: 'tbr-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  errorMessage = '';
  isPending = signal(false);

  constructor(private fb: FormBuilder, private userAccountApiService: UserAccountApiService, private authStorage: AuthStorageService, private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    const isAuthenticated = this.authStorage.isAuthenticated();

    if (isAuthenticated) {
      this.router.navigate(['/']);
    }
  }

  login(): void {
    if (this.loginForm.valid) {
      this.isPending.set(true);
      this.userAccountApiService.login(this.loginForm.value).subscribe({
        next: () => this.router.navigate(['/']),
        error: () => {
          this.isPending.set(false);
          this.errorMessage = 'Invalid credentials';
        }
      });
    }
  }
}
