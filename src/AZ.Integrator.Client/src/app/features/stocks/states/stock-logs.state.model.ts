import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { StockLogViewModel } from '../../../shared/graphql/graphql-integrator.schema';
import { GraphQLQueryVo } from '../../../shared/graphql/graphql.query';

export interface StockLogsStateModel {
  graphqlQuery: GraphQLQueryVo;
  graphqlQueryResponse: GraphQLResponseWithoutPaginationVo<StockLogViewModel[]>;
  dateFilter: { from: Date; to: Date };
  logs: StockLogViewModel[];
}
