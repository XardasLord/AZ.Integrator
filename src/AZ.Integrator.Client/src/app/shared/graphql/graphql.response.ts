import { PageInfo } from './graphql-integrator.schema';

export class GraphQLResponseWithoutPaginationVo<T> {
  public result!: T;
}

export class GraphQLResponse<T> {
  public result!: GraphQLResponseValue<T>;
}

export class GraphQLResponseValue<T> {
  public nodes!: T;
  public totalCount!: number;
  public pageInfo!: PageInfo;
}
