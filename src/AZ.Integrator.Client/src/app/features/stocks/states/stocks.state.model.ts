import { GraphQLQueryVo } from '../../../shared/graphql/graphql.query';
import { GraphQLResponseWithoutPaginationVo } from '../../../shared/graphql/graphql.response';
import { StockViewModel } from '../../../shared/graphql/graphql-integrator.schema';

export interface StocksStateModel {
  graphqlQuery: GraphQLQueryVo;
  graphqlQueryResponse: GraphQLResponseWithoutPaginationVo<StockViewModel[]>;
  stocks: StockViewModel[];
}
