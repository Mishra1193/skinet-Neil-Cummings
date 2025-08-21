import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject, model } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const snackbar = inject(SnackbarService);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      switch (err.status) {
        case 400:
          if (err.error.errors) {
            const modelStateErrors = [];
            for (const key in err.error.errors) {
              if (err.error.errors[key]) {
                modelStateErrors.push(err.error.errors[key]);
              }
            }
            throw modelStateErrors.flat();
          } else {
            snackbar.error('Bad request');
          }
          break;
        case 401:
          snackbar.error('Unauthorized request — please log in.');
          break;
        case 404:
          snackbar.error('Resource not found');
          router.navigateByUrl('/not-found');
          break;
        case 500:
          snackbar.error('Server error — please try again later.');
          snackbar.error('Server error - please try again later');

          const navigationExtras: NavigationExtras = {
            state: { error: err.error }, // 👈 Pass error object via state
          };

          router.navigateByUrl('/server-error');
          break;
        default:
          snackbar.error('Something went wrong.');
      }
      return throwError(() => err);
    })
  );
};
