import { inject, Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Action, Selector, State, StateContext, StateToken, Store } from '@ngxs/store';
import { catchError, Observable, of, switchMap, tap } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { BarcodeScannerStateModel } from './barcode-scanner.state.model';
import {
  IntegratorQueryBarcodeScannerLogsArgs,
  SortEnumType,
  StockLogViewModel,
} from '../../../shared/graphql/graphql-integrator.schema';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { StocksService } from '../services/stocks.service';
import {
  AddScan,
  ClearSynced,
  LoadLogs,
  LoadPendingScans,
  ProcessQueue,
  ProcessSingleScan,
  RemoveScan,
  ResetCounters,
  RetryFailed,
  RevertScan,
} from './barcode-scanner.action';
import { AuthState } from '../../../shared/states/auth.state';
import { PendingScan, ScanStatus, ScanType } from '../models/pending-scan.model';

const BARCODE_SCANNER_STATE_TOKEN = new StateToken<BarcodeScannerStateModel>('barcodeScanner');
const PENDING_SCANS_STORAGE_KEY = 'pending_scans';
const SYNCED_SCANS_STORAGE_KEY = 'synced_scans';
const TOTAL_SYNCED_COUNT_STORAGE_KEY = 'total_synced_count';
const MAX_RETRIES = 5;
const MAX_SYNCED_HISTORY = 50; // Maksymalna liczba przechowywanych zsynchronizowanych skanów

@State<BarcodeScannerStateModel>({
  name: BARCODE_SCANNER_STATE_TOKEN,
  defaults: {
    graphqlQueryResponse: new GraphQLResponseWithoutPaginationVo<StockLogViewModel[]>(),
    logs: [],
    pendingScans: [],
    syncedScans: [],
    totalSyncedCount: 0,
  },
})
@Injectable()
export class BarcodeScannerState {
  private store = inject(Store);
  private stocksService = inject(StocksService);
  private toastService = inject(ToastrService);
  private autoProcessorSubscription?: ReturnType<typeof setInterval>;

  @Selector([BARCODE_SCANNER_STATE_TOKEN])
  static logs(state: BarcodeScannerStateModel): StockLogViewModel[] {
    return state?.logs || [];
  }

  @Selector([BARCODE_SCANNER_STATE_TOKEN])
  static pendingScans(state: BarcodeScannerStateModel): PendingScan[] {
    return state?.pendingScans || [];
  }

  @Selector([BARCODE_SCANNER_STATE_TOKEN])
  static syncedScans(state: BarcodeScannerStateModel): PendingScan[] {
    return state?.syncedScans || [];
  }

  @Selector([BARCODE_SCANNER_STATE_TOKEN])
  static totalSyncedCount(state: BarcodeScannerStateModel): number {
    return state?.totalSyncedCount || 0;
  }

  @Action(LoadLogs)
  loadLogs(ctx: StateContext<BarcodeScannerStateModel>) {
    const filters: IntegratorQueryBarcodeScannerLogsArgs = {};

    filters.where = {
      createdBy: { eq: this.store.selectSnapshot(AuthState.getProfile)?.username },
    };

    filters.order = [
      {
        id: SortEnumType.Desc,
      },
    ];

    return this.stocksService.getBarcodeScannerLogs(filters).pipe(
      tap(response => {
        ctx.patchState({
          logs: response.result,
        });
      })
    );
  }

  @Action(LoadPendingScans)
  loadPendingScans(ctx: StateContext<BarcodeScannerStateModel>) {
    const pendingScans = this.loadPendingFromStorage();
    const syncedScans = this.loadSyncedFromStorage();
    const totalSyncedCount = this.loadTotalSyncedCountFromStorage();

    ctx.patchState({
      pendingScans,
      syncedScans,
      totalSyncedCount,
    });

    // Uruchom auto-processor po załadowaniu
    this.startAutoProcessor(ctx);
  }

  @Action(AddScan)
  addScan(ctx: StateContext<BarcodeScannerStateModel>, action: AddScan) {
    const scan: PendingScan = {
      id: this.generateId(),
      barcode: action.barcode.trim(),
      type: action.type,
      changeQuantity: action.type === ScanType.IN ? 1 : -1,
      status: ScanStatus.PENDING,
      timestamp: Date.now(),
      retryCount: 0,
    };

    const currentScans = ctx.getState().pendingScans;
    const updatedScans = [scan, ...currentScans];

    ctx.patchState({ pendingScans: updatedScans });
    this.savePendingToStorage(updatedScans);

    // Natychmiast próbuj przetworzyć
    return ctx.dispatch(new ProcessSingleScan(scan.id));
  }

  @Action(RemoveScan)
  removeScan(ctx: StateContext<BarcodeScannerStateModel>, action: RemoveScan) {
    const state = ctx.getState();
    const updatedPendingScans = state.pendingScans.filter(s => s.id !== action.scanId);
    const updatedSyncedScans = state.syncedScans.filter(s => s.id !== action.scanId);

    ctx.patchState({
      pendingScans: updatedPendingScans,
      syncedScans: updatedSyncedScans,
    });

    this.savePendingToStorage(updatedPendingScans);
    this.saveSyncedToStorage(updatedSyncedScans);
  }

  @Action(ProcessSingleScan)
  processSingleScan(ctx: StateContext<BarcodeScannerStateModel>, action: ProcessSingleScan) {
    const scan = ctx.getState().pendingScans.find(s => s.id === action.scanId);

    if (!scan || scan.status === ScanStatus.SYNCING || scan.retryCount >= MAX_RETRIES) {
      return of(null);
    }

    // Oznacz jako SYNCING
    this.updatePendingScan(ctx, action.scanId, { status: ScanStatus.SYNCING });

    return this.stocksService.updateStockQuantity(scan.barcode, scan.changeQuantity, scan.id).pipe(
      tap(() => {
        // Sukces - przenieś do syncedScans
        const syncedScan: PendingScan = { ...scan, status: ScanStatus.SYNCED };
        this.moveScanToSynced(ctx, syncedScan);
        this.toastService.success(`Kod ${scan.barcode} zsynchronizowany`, 'Sukces', { timeOut: 2000 });
      }),
      catchError((error: HttpErrorResponse) => {
        // Błąd
        const newRetryCount = scan.retryCount + 1;
        const newStatus = newRetryCount >= MAX_RETRIES ? ScanStatus.FAILED : ScanStatus.PENDING;

        this.updatePendingScan(ctx, action.scanId, {
          status: newStatus,
          retryCount: newRetryCount,
          lastError: error?.error?.Message || error.message,
        });

        if (newStatus === ScanStatus.FAILED) {
          this.toastService.error(
            `Nie udało się zsynchronizować kodu ${scan.barcode} po ${MAX_RETRIES} próbach`,
            'Błąd synchronizacji'
          );
        }

        return of(null);
      })
    );
  }

  @Action(ProcessQueue)
  processQueue(ctx: StateContext<BarcodeScannerStateModel>) {
    const pendingScans = ctx
      .getState()
      .pendingScans.filter(s => s.status === ScanStatus.PENDING && s.retryCount < MAX_RETRIES);

    if (pendingScans.length === 0) {
      return of(null);
    }

    // Przetwarzaj sekwencyjnie
    return pendingScans.reduce(
      (acc, scan) => acc.pipe(switchMap(() => ctx.dispatch(new ProcessSingleScan(scan.id)))),
      of(null) as Observable<null | void>
    );
  }

  @Action(RetryFailed)
  retryFailed(ctx: StateContext<BarcodeScannerStateModel>) {
    const currentScans = ctx.getState().pendingScans;
    const updatedScans = currentScans.map(scan =>
      scan.status === ScanStatus.FAILED ? { ...scan, status: ScanStatus.PENDING, retryCount: 0 } : scan
    );

    ctx.patchState({ pendingScans: updatedScans });
    this.savePendingToStorage(updatedScans);

    return ctx.dispatch(new ProcessQueue());
  }

  @Action(ClearSynced)
  clearSynced(ctx: StateContext<BarcodeScannerStateModel>) {
    // Wyczyść całą historię zsynchronizowanych skanów
    ctx.patchState({ syncedScans: [] });
    this.saveSyncedToStorage([]);
  }

  @Action(ResetCounters)
  resetCounters(ctx: StateContext<BarcodeScannerStateModel>) {
    // Resetuj licznik zsynchronizowanych skanów
    ctx.patchState({ totalSyncedCount: 0 });
    this.saveTotalSyncedCountToStorage(0);
  }

  @Action(RevertScan)
  revertScan(ctx: StateContext<BarcodeScannerStateModel>, action: RevertScan) {
    const scan = ctx.getState().syncedScans.find(s => s.id === action.scanId);

    if (!scan) {
      this.toastService.error('Nie znaleziono zsynchronizowanego skanu', 'Błąd');
      return of(null);
    }

    // Pobierz najnowszy log dla tego barcode'u
    return this.stocksService
      .getBarcodeScannerLogs({
        where: {
          packageCode: { eq: scan.barcode },
          changeQuantity: { eq: scan.changeQuantity },
          scanId: { eq: scan.id },
        },
        order: [{ id: SortEnumType.Desc }],
      })
      .pipe(
        switchMap(response => {
          const logs = response.result;
          if (!logs || logs.length === 0) {
            this.toastService.error('Nie znaleziono logu do cofnięcia', 'Błąd');
            return of(null);
          }

          const logToRevert = logs[0];
          return this.stocksService.revertScan(scan.barcode, logToRevert.id).pipe(
            tap(() => {
              ctx.dispatch(new RemoveScan(scan.id));
              this.toastService.success(`Skan dla kodu ${scan.barcode} został poprawnie cofnięty`, 'Sukces');
            }),
            catchError((error: HttpErrorResponse) => {
              this.toastService.error(
                `Wystąpił błąd podczas cofania skanu - ${error?.error?.Message || error.message}`,
                'Błąd'
              );
              return of(null);
            })
          );
        }),
        catchError((error: HttpErrorResponse) => {
          this.toastService.error(
            `Wystąpił błąd podczas pobierania logów - ${error?.error?.Message || error.message}`,
            'Błąd'
          );
          return of(null);
        })
      );
  }

  // === Helper Methods ===

  private updatePendingScan(
    ctx: StateContext<BarcodeScannerStateModel>,
    scanId: string,
    updates: Partial<PendingScan>
  ) {
    const currentScans = ctx.getState().pendingScans;
    const updatedScans = currentScans.map(s => (s.id === scanId ? { ...s, ...updates } : s));

    ctx.patchState({ pendingScans: updatedScans });
    this.savePendingToStorage(updatedScans);
  }

  private moveScanToSynced(ctx: StateContext<BarcodeScannerStateModel>, syncedScan: PendingScan) {
    const state = ctx.getState();

    // Usuń z pendingScans
    const updatedPendingScans = state.pendingScans.filter(s => s.id !== syncedScan.id);

    // Dodaj do syncedScans na początku i ogranicz do MAX_SYNCED_HISTORY
    const updatedSyncedScans = [syncedScan, ...state.syncedScans].slice(0, MAX_SYNCED_HISTORY);

    // Zwiększ licznik zsynchronizowanych
    const newTotalSyncedCount = state.totalSyncedCount + 1;

    ctx.patchState({
      pendingScans: updatedPendingScans,
      syncedScans: updatedSyncedScans,
      totalSyncedCount: newTotalSyncedCount,
    });

    this.savePendingToStorage(updatedPendingScans);
    this.saveSyncedToStorage(updatedSyncedScans);
    this.saveTotalSyncedCountToStorage(newTotalSyncedCount);
  }

  private savePendingToStorage(scans: PendingScan[]) {
    try {
      localStorage.setItem(PENDING_SCANS_STORAGE_KEY, JSON.stringify(scans));
    } catch (error) {
      console.error('Failed to save pending scans to localStorage', error);
    }
  }

  private saveSyncedToStorage(scans: PendingScan[]) {
    try {
      // Automatycznie przytnij do MAX_SYNCED_HISTORY przed zapisem
      const limitedScans = scans.slice(0, MAX_SYNCED_HISTORY);
      localStorage.setItem(SYNCED_SCANS_STORAGE_KEY, JSON.stringify(limitedScans));
    } catch (error) {
      console.error('Failed to save synced scans to localStorage', error);
    }
  }

  private loadPendingFromStorage(): PendingScan[] {
    try {
      const stored = localStorage.getItem(PENDING_SCANS_STORAGE_KEY);

      return stored ? (JSON.parse(stored) as PendingScan[]) : [];
    } catch (error) {
      console.error('Failed to load pending scans from localStorage', error);
      return [];
    }
  }

  private loadSyncedFromStorage(): PendingScan[] {
    try {
      const stored = localStorage.getItem(SYNCED_SCANS_STORAGE_KEY);
      const scans = stored ? (JSON.parse(stored) as PendingScan[]) : [];

      return scans.slice(0, MAX_SYNCED_HISTORY);
    } catch (error) {
      console.error('Failed to load synced scans from localStorage', error);
      return [];
    }
  }

  private saveTotalSyncedCountToStorage(count: number) {
    try {
      localStorage.setItem(TOTAL_SYNCED_COUNT_STORAGE_KEY, count.toString());
    } catch (error) {
      console.error('Failed to save total synced count to localStorage', error);
    }
  }

  private loadTotalSyncedCountFromStorage(): number {
    try {
      const stored = localStorage.getItem(TOTAL_SYNCED_COUNT_STORAGE_KEY);
      return stored ? parseInt(stored, 10) : 0;
    } catch (error) {
      console.error('Failed to load total synced count from localStorage', error);
      return 0;
    }
  }

  private generateId(): string {
    return `${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
  }

  private startAutoProcessor(ctx: StateContext<BarcodeScannerStateModel>) {
    if (this.autoProcessorSubscription) {
      return;
    }

    // Auto-process co 10 sekund
    this.autoProcessorSubscription = setInterval(() => {
      // Sprawdź czy są jakieś skany do przetworzenia
      const state = ctx.getState();
      const hasPendingScans = state.pendingScans.some(
        s => s.status === ScanStatus.PENDING && s.retryCount < MAX_RETRIES
      );

      if (hasPendingScans) {
        ctx.dispatch(new ProcessQueue());
      }
    }, 10000);
  }

  private stopAutoProcessor() {
    if (this.autoProcessorSubscription) {
      clearInterval(this.autoProcessorSubscription);
      this.autoProcessorSubscription = undefined;
    }
  }
}
