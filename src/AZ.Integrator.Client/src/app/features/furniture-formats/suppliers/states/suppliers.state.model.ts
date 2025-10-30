import { GraphQLQueryVo } from '../../../../shared/graphql/graphql.query';
import { GraphQLResponse } from '../../../../shared/graphql/graphql.response';
import { SupplierViewModel } from '../../../../shared/graphql/graphql-integrator.schema';

export interface SuppliersStateModel {
  suppliers: SupplierViewModel[];
  graphQLQuery: GraphQLQueryVo;
  graphQLResponse: GraphQLResponse<SupplierViewModel[]>;
  graphQLFilters: Record<string, unknown>;
}
