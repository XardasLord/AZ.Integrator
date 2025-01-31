import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { StockLogViewModel } from '../../../shared/graphql/graphql-integrator.schema';

export interface BarcodeScannerStateModel {
  graphqlQueryResponse: GraphQLResponseWithoutPaginationVo<StockLogViewModel[]>;
  logs: StockLogViewModel[];
}
