import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class SnackbarService {
  private snackBar = inject(MatSnackBar);

  error(message: string, action = 'Close', duration = 3000) {
    this.snackBar.open(message, action, {
      duration,
      panelClass: ['snack-error'],       // <-- match CSS below
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }

  success(message: string, action = 'Close', duration = 3000) {
    this.snackBar.open(message, action, {
      duration,
      panelClass: ['snack-success'],     // <-- match CSS below
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }
}
