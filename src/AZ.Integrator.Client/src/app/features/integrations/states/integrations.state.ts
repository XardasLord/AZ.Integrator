import { inject, Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, tap, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { IntegrationsStateModel } from './integrations.state.model';
import {
  AddIntegration,
  DeleteIntegration,
  LoadIntegrations,
  TestIntegrationConnection,
  ToggleIntegrationStatus,
} from './integrations.action';
import { IntegrationsService } from '../services/integrations.service';
import { IntegrationWithType } from '../models/integration.model';
import { IntegrationType } from '../models/integration-type.enum';

const INTEGRATIONS_STATE_TOKEN = new StateToken<IntegrationsStateModel>('integrations');

@State<IntegrationsStateModel>({
  name: INTEGRATIONS_STATE_TOKEN,
  defaults: {
    integrations: [],
    loading: false,
  },
})
@Injectable()
export class IntegrationsState {
  private integrationsService = inject(IntegrationsService);
  private toastService = inject(ToastrService);

  @Selector([INTEGRATIONS_STATE_TOKEN])
  static integrations(state: IntegrationsStateModel): IntegrationWithType[] {
    return state.integrations;
  }

  @Selector([INTEGRATIONS_STATE_TOKEN])
  static loading(state: IntegrationsStateModel): boolean {
    return state.loading;
  }

  @Selector([INTEGRATIONS_STATE_TOKEN])
  static allegroIntegrations(state: IntegrationsStateModel): IntegrationWithType[] {
    return state.integrations.filter(i => i.type === IntegrationType.Allegro);
  }

  @Selector([INTEGRATIONS_STATE_TOKEN])
  static erliIntegrations(state: IntegrationsStateModel): IntegrationWithType[] {
    return state.integrations.filter(i => i.type === IntegrationType.Erli);
  }

  @Selector([INTEGRATIONS_STATE_TOKEN])
  static shopifyIntegrations(state: IntegrationsStateModel): IntegrationWithType[] {
    return state.integrations.filter(i => i.type === IntegrationType.Shopify);
  }

  @Selector([INTEGRATIONS_STATE_TOKEN])
  static fakturowniaIntegrations(state: IntegrationsStateModel): IntegrationWithType[] {
    return state.integrations.filter(i => i.type === IntegrationType.Fakturownia);
  }

  @Selector([INTEGRATIONS_STATE_TOKEN])
  static inpostIntegrations(state: IntegrationsStateModel): IntegrationWithType[] {
    return state.integrations.filter(i => i.type === IntegrationType.Inpost);
  }

  @Selector([INTEGRATIONS_STATE_TOKEN])
  static dpdIntegrations(state: IntegrationsStateModel): IntegrationWithType[] {
    return state.integrations.filter(i => i.type === IntegrationType.Dpd);
  }

  @Action(LoadIntegrations)
  loadIntegrations(ctx: StateContext<IntegrationsStateModel>) {
    ctx.patchState({ loading: true });

    return this.integrationsService.getAllIntegrations().pipe(
      tap(integrations => {
        ctx.patchState({
          integrations,
          loading: false,
        });
      }),
      catchError(error => {
        ctx.patchState({ loading: false });
        this.toastService.error('Błąd podczas wczytywania integracji');
        return throwError(() => error);
      })
    );
  }

  @Action(AddIntegration)
  addIntegration(ctx: StateContext<IntegrationsStateModel>, action: AddIntegration) {
    const state = ctx.getState();
    ctx.patchState({
      integrations: [...state.integrations, action.integration],
    });
  }

  @Action(ToggleIntegrationStatus)
  toggleIntegrationStatus(ctx: StateContext<IntegrationsStateModel>, action: ToggleIntegrationStatus) {
    return this.integrationsService
      .updateIntegrationStatus(action.integrationType, action.sourceSystemId, action.isEnabled)
      .pipe(
        tap(() => {
          const state = ctx.getState();
          const integrations = state.integrations.map(i => {
            const integration = i.integration as Record<string, unknown>;
            if (
              integration['sourceSystemId'] === action.sourceSystemId ||
              integration['organizationId']?.toString() === action.sourceSystemId
            ) {
              return {
                ...i,
                integration: {
                  ...i.integration,
                  isEnabled: action.isEnabled,
                },
              };
            }
            return i;
          });

          ctx.patchState({ integrations });
          this.toastService.success('Status integracji został zaktualizowany');
        }),
        catchError(error => {
          this.toastService.error('Błąd podczas aktualizacji statusu integracji');
          return throwError(() => error);
        })
      );
  }

  @Action(DeleteIntegration)
  deleteIntegration(ctx: StateContext<IntegrationsStateModel>, action: DeleteIntegration) {
    return this.integrationsService.deleteIntegration(action.integrationType, action.sourceSystemId).pipe(
      tap(() => {
        const state = ctx.getState();
        const integrations = state.integrations.filter(i => {
          const integration = i.integration as Record<string, unknown>;
          return (
            integration['sourceSystemId'] !== action.sourceSystemId &&
            integration['organizationId']?.toString() !== action.sourceSystemId
          );
        });

        ctx.patchState({ integrations });
        this.toastService.success('Integracja została usunięta');
      }),
      catchError(error => {
        this.toastService.error('Błąd podczas usuwania integracji');
        return throwError(() => error);
      })
    );
  }

  @Action(TestIntegrationConnection)
  testIntegrationConnection(ctx: StateContext<IntegrationsStateModel>, action: TestIntegrationConnection) {
    return this.integrationsService.testConnection(action.integrationType, action.sourceSystemId).pipe(
      tap(response => {
        if (response.isValid) {
          this.toastService.success('Połączenie działa poprawnie!', 'Test połączenia');
        } else {
          this.toastService.warning(response.message || 'Połączenie nie działa poprawnie', 'Test połączenia');
        }
      }),
      catchError(error => {
        this.toastService.error('Nie udało się przetestować połączenia', 'Test połączenia');
        return throwError(() => error);
      })
    );
  }
}
