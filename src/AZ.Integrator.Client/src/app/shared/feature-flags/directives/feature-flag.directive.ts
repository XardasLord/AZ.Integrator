import { Directive, inject, Input, signal, TemplateRef, ViewContainerRef } from '@angular/core';
import { FeatureFlagsService } from '../services/feature-flags.service';

@Directive({
  selector: '[ff]',
  standalone: true,
})
export class FeatureFlagDirective {
  templateRef = inject(TemplateRef);
  viewContainer = inject(ViewContainerRef);
  featureFlagService = inject(FeatureFlagsService);
  hasView = signal(false);

  @Input() set ff(featureFlag: string | undefined) {
    if (!featureFlag && !this.hasView()) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasView.set(true);
      return;
    }

    if (this.featureFlagService.isEnabled(featureFlag!) && !this.hasView()) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasView.set(true);
    } else {
      this.viewContainer.clear();
      this.hasView.set(false);
    }
  }
}
