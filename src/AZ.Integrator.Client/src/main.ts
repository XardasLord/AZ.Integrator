import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { enableDebugTools } from '@angular/platform-browser';
import { ApplicationRef } from '@angular/core';

import { AppModule } from './app/app.module';

platformBrowserDynamic()
  .bootstrapModule(AppModule)
  .then(module =>
    enableDebugTools(module.injector.get(ApplicationRef).components[0])
  )
  .catch(err => console.error(err));
