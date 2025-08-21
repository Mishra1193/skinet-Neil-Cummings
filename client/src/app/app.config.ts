// src/app/app.config.ts
import {
  ApplicationConfig,
  provideAppInitializer,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection,
  inject,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { MAT_DIALOG_DEFAULT_OPTIONS } from '@angular/material/dialog';
import { errorInterceptor } from './core/interceptors/error-interceptor';
import { InitService } from './core/services/init.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([errorInterceptor])),

    provideAppInitializer(() => {
      const initService = inject(InitService);
      return initService.init().finally(() => {
        const splash = document.getElementById('initial-splash');
        if (splash) splash.remove();
      });
    }),

    {
      provide: MAT_DIALOG_DEFAULT_OPTIONS,
      useValue: { autoFocus: 'dialog', restoreFocus: true },
    },
  ],
};
