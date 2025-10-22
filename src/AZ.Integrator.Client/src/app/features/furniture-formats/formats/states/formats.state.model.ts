import { GraphQLQueryVo } from '../../../../shared/graphql/graphql.query';
import { GraphQLResponse } from '../../../../shared/graphql/graphql.response';
import { FurnitureModelViewModel } from '../../../../shared/graphql/graphql-integrator.schema';

export interface FormatsStateModel {
  furnitureDefinitions: FurnitureModelViewModel[];
  graphQLQuery: GraphQLQueryVo;
  graphQLResponse: GraphQLResponse<FurnitureModelViewModel[]>;
  graphQLFilters: Record<string, unknown>;
}
