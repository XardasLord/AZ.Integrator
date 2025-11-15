import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { StockLogViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { PendingScan } from '../models/pending-scan.model';

export interface BarcodeScannerStateModel {
  graphqlQueryResponse: GraphQLResponseWithoutPaginationVo<StockLogViewModel[]>;
  logs: StockLogViewModel[];
  pendingScans: PendingScan[]; // Lokalna kolejka skanów
}
