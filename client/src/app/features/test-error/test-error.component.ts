import { Component, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-test-error',
  standalone: true,
  imports: [MatButtonModule],
  templateUrl: './test-error.component.html',
})
export class TestErrorComponent {
  // Hardcode for this training step (as per transcript)
  // Use 5000/5001 based on your API (Kestrel typically serves both HTTP/HTTPS)
  private baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);
  validationErrors?: string[]; 

  get404Error() {
    this.http.get(this.baseUrl + 'buggy/notfound').subscribe({
      next: (res) => console.log(res),
      error: (err) => console.log(err),
    });
  }

  get400Error() {
    this.http.get(this.baseUrl + 'buggy/badrequest').subscribe({
      next: (res) => console.log(res),
      error: (err) => console.log(err),
    });
  }

  get401Error() {
    // Match controller's UK spelling: unauthorised
    this.http.get(this.baseUrl + 'buggy/unauthorised').subscribe({
      next: (res) => console.log(res),
      error: (err) => console.log(err),
    });
  }

  get500Error() {
    this.http.get(this.baseUrl + 'buggy/internalerror').subscribe({
      next: (res) => console.log(res),
      error: (err) => console.log(err),
    });
  }

  get400ValidationError() {
    // Must be POST to see validation errors; send empty payload
    this.http.post(this.baseUrl + 'buggy/validationerror', {}).subscribe({
      next: (res) => console.log(res),
      error: (err) => this.validationErrors = err.errors,
    });
  }
}
