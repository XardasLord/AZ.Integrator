import { GraphQLQueryVo } from '../../../shared/graphql/graphql.query';
import { GraphQLResponse } from '../../../shared/graphql/graphql.response';
import {
  IntegratorQueryTagParcelTemplatesArgs,
  TagParcelTemplateViewModel,
} from '../../../shared/graphql/graphql-integrator.schema';

export interface ParcelTemplatesStateModel {
  graphQLQuery: GraphQLQueryVo;
  graphQLResponse: GraphQLResponse<TagParcelTemplateViewModel[]>;
  graphQLFilters: IntegratorQueryTagParcelTemplatesArgs;
  templates: TagParcelTemplateViewModel[];
}
