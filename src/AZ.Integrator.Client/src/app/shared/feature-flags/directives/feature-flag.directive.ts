import { Directive, inject, Input, OnDestroy, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { Store } from '@ngxs/store';
import { Subject, takeUntil } from 'rxjs';
import { FeatureFlagsState } from '../store/feature-flags.state.ts';
import { FeatureFlagsService } from '../services/feature-flags.service';

@Directive({
  selector: '[appFf]',
  standalone: true,
})
export class FeatureFlagDirective implements OnInit, OnDestroy {
  private templateRef = inject(TemplateRef);
  private viewContainer = inject(ViewContainerRef);
  private store = inject(Store);
  private featureFlagsService = inject(FeatureFlagsService);
  private destroy$ = new Subject<void>();
  private hasView = false;
  private featureFlag?: string;

  @Input() set appFf(featureFlag: string | undefined) {
    this.featureFlag = featureFlag;
    this.updateView();
  }

  ngOnInit(): void {
    // Listens for changes in store and updates the view accordingly
    this.store
      .select(FeatureFlagsState.getFeatureFlags)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.updateView();
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private updateView(): void {
    // No feature flag provided, always show the view
    if (!this.featureFlag && !this.hasView) {
      if (!this.hasView) {
        this.showView();
      }
      return;
    }

    const isEnabled = this.featureFlagsService.isEnabled(this.featureFlag!);

    if (isEnabled && !this.hasView) {
      this.showView();
    } else if (!isEnabled && this.hasView) {
      this.hideView();
    }
  }

  private showView(): void {
    this.viewContainer.createEmbeddedView(this.templateRef);
    this.hasView = true;
  }

  private hideView(): void {
    this.viewContainer.clear();
    this.hasView = false;
  }
}
