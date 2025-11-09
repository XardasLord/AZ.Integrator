import { PageEvent } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { GraphQLQueryVo } from './graphql.query';
import { WatchQueryOptionsAlone } from 'apollo-angular/types';
import { PageInfo } from './graphql-integrator.schema';

export class GraphQLHelper {
  public static getInitialPageEvent(pageSize = environment.defaultPageSize): PageEvent {
    const output = new PageEvent();

    output.pageIndex = 0;
    output.pageSize = pageSize;
    output.previousPageIndex = undefined;

    return output;
  }

  public static defaultGraphQLWatchQueryOptions: WatchQueryOptionsAlone = {};
  public static networkOnlyGraphQLWatchQueryOptions: WatchQueryOptionsAlone = {
    fetchPolicy: 'network-only',
  };
  public static cacheFirstGraphQLWatchQueryOptions: WatchQueryOptionsAlone = {
    fetchPolicy: 'cache-first',
  };

  public static getGraphQLQueryPageTableOptions<T>(graphQLQuery: GraphQLQueryVo, pageInfo: PageInfo): T {
    const filters = {
      first: graphQLQuery?.currentPage?.pageSize,
      order: graphQLQuery.currentOrder?.orders,
      before: pageInfo?.startCursor,
      after: pageInfo?.endCursor,
      last: graphQLQuery?.currentPage?.pageSize,
    };

    Object.assign(filters, {
      first: null,
      last: null,
      before: null,
      after: null,
    });

    // Move it to GraphQL helper and improve these if statements to be more readable
    if (graphQLQuery.currentPage.previousPageIndex === undefined) {
      filters.first = graphQLQuery.currentPage.pageSize;
    } else {
      if (
        graphQLQuery.currentPage.pageIndex === 0 ||
        graphQLQuery.currentPage.previousPageIndex === graphQLQuery.currentPage.pageIndex
      ) {
        // Page remains the same, page size changed, or we're back on page number 1
        filters.first = graphQLQuery.currentPage.pageSize;
      } else if (graphQLQuery.currentPage.previousPageIndex > graphQLQuery.currentPage.pageIndex) {
        if (pageInfo.hasPreviousPage) {
          filters.before = pageInfo.startCursor;
          filters.last = graphQLQuery.currentPage.pageSize;
        } else {
          filters.first = graphQLQuery.currentPage.pageSize;
        }
      } else {
        if (pageInfo.hasNextPage) {
          filters.after = pageInfo.endCursor;
          filters.first = graphQLQuery.currentPage.pageSize;
        } else {
          filters.first = graphQLQuery.currentPage.pageSize;
          filters.after = pageInfo.startCursor;
        }
      }
    }

    return filters as unknown as T;
  }

  public static addCommonSearchRule(
    searchPhrase: string,
    fields: string[]
  ): {
    or: {
      [x: string]: {
        contains: string;
      };
    }[];
  } {
    return {
      or: fields.map(field => ({ [field]: { contains: searchPhrase.toUpperCase() } })),
    };
  }
}
