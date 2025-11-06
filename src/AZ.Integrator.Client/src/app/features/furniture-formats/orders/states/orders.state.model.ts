import { GraphQLQueryVo } from '../../../../shared/graphql/graphql.query';
import { GraphQLResponse } from '../../../../shared/graphql/graphql.response';
import { PartDefinitionsOrderViewModel } from '../../../../shared/graphql/graphql-integrator.schema';

export interface OrdersStateModel {
  orders: PartDefinitionsOrderViewModel[];
  graphQLQuery: GraphQLQueryVo;
  graphQLResponse: GraphQLResponse<PartDefinitionsOrderViewModel[]>;
  graphQLFilters: Record<string, unknown>;
}
