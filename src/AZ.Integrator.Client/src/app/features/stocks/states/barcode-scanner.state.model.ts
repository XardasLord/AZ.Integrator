import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { StockLogViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { PendingScan } from '../models/pending-scan.model';

export interface BarcodeScannerStateModel {
  graphqlQueryResponse: GraphQLResponseWithoutPaginationVo<StockLogViewModel[]>;
  logs: StockLogViewModel[];
  pendingScans: PendingScan[]; // Skany oczekujące na synchronizację (PENDING, SYNCING, FAILED)
  syncedScans: PendingScan[]; // Historia zsynchronizowanych skanów (max MAX_SYNCED_HISTORY ostatnich)
  totalSyncedCount: number; // Łączna liczba zsynchronizowanych skanów (nie resetuje się)
}
