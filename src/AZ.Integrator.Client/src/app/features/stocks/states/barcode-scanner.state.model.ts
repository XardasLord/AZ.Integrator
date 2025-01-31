import { GraphQLQueryVo } from '../../../shared/graphql/graphql.query';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { StockLogViewModel } from '../../../shared/graphql/graphql-integrator.schema';

export interface BarcodeScannerStateModel {
  graphqlQuery: GraphQLQueryVo;
  graphqlQueryResponse: GraphQLResponseWithoutPaginationVo<StockLogViewModel[]>;
  logs: StockLogViewModel[];
}
