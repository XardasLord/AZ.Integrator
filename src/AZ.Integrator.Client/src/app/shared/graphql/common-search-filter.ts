import { PageEvent } from '@angular/material/paginator';
import { GraphQLOrderVo, GraphQLQueryVo } from './graphql.query';
import { GraphQLResponse } from './graphql.response';
import { InputMaybe } from './graphql-integrator.schema';
import { GraphQLHelper } from './graphql.helper';

interface MinimalFilter {
  where?: any;
  order?: any;
}

export function applyChangePageFilter<T extends MinimalFilter, Q, R>(
  state: {
    graphQLQuery: GraphQLQueryVo;
    graphQLFilters: T;
    graphQLResponse: GraphQLResponse<Q[]>;
  },

  currentPage: PageEvent,
  order?: InputMaybe<Array<R>>
): {
  graphQLQuery: GraphQLQueryVo;
  graphQLFilters: T;
} {
  const queryOrder = order || state.graphQLFilters.order;

  const updatedGraphQLQuery = {
    ...state.graphQLQuery,
    currentPage,
  };

  if (updatedGraphQLQuery.currentOrder === undefined) {
    updatedGraphQLQuery.currentOrder = new GraphQLOrderVo();
  }

  if (updatedGraphQLQuery.currentOrder.orders == null) {
    updatedGraphQLQuery.currentOrder.orders = queryOrder;
  }

  const updatedGraphQLFilters = {
    ...state.graphQLFilters,
    ...GraphQLHelper.getGraphQLQueryPageTableOptions<T>(updatedGraphQLQuery, state.graphQLResponse.result?.pageInfo),
  };

  return {
    graphQLQuery: updatedGraphQLQuery,
    graphQLFilters: updatedGraphQLFilters,
  };
}

export function applyCommonSearchFilter<T extends MinimalFilter, Q, R>(
  state: {
    graphQLQuery: GraphQLQueryVo;
    graphQLFilters: T;
    graphQLResponse: GraphQLResponse<Q[]>;
  },
  searchText: string,
  searchFields: string[],
  order?: InputMaybe<Array<R>>
): {
  graphQLQuery: GraphQLQueryVo;
  graphQLFilters: T;
} {
  const queryOrder = order || state.graphQLFilters.order;

  const updatedGraphQLQuery = {
    ...state.graphQLQuery,
    searchText,
    currentPage: GraphQLHelper.getInitialPageEvent(state.graphQLQuery.currentPage.pageSize),
  };

  const updatedGraphQLFilters = {
    ...state.graphQLFilters,
    ...GraphQLHelper.getGraphQLQueryPageTableOptions<T>(updatedGraphQLQuery, state.graphQLResponse.result?.pageInfo),
    where: {
      ...state.graphQLFilters.where,
      ...GraphQLHelper.addCommonSearchRule(searchText, searchFields),
    },
    order: queryOrder,
  };

  return {
    graphQLQuery: updatedGraphQLQuery,
    graphQLFilters: updatedGraphQLFilters,
  };
}

export function applyGroupedSearchFilter<T extends MinimalFilter, Q, R>(
  state: {
    graphQLQuery: GraphQLQueryVo;
    graphQLFilters: T;
    graphQLResponse: GraphQLResponse<Q[]>;
  },
  searchText: string,
  groupSearchFields: string[],
  childProperty: string,
  childSearchFields: string[],
  order?: InputMaybe<Array<R>>
): {
  graphQLQuery: GraphQLQueryVo;
  graphQLFilters: T;
} {
  const queryOrder = order || state.graphQLFilters.order;

  const updatedGraphQLQuery = {
    ...state.graphQLQuery,
    searchText,
    currentPage: GraphQLHelper.getInitialPageEvent(state.graphQLQuery.currentPage.pageSize),
  };

  let updatedWhere = { ...state.graphQLFilters.where };

  const groupFilters = groupSearchFields.map(field => ({
    [field]: { contains: searchText },
  }));

  const childFieldFilters = childSearchFields.map(field => ({
    [field]: { contains: searchText },
  }));

  const childFilter = {
    [childProperty]: {
      some: {
        or: childFieldFilters,
      },
    },
  };

  updatedWhere = {
    ...updatedWhere,
    or: [...groupFilters, childFilter],
  };

  const updatedGraphQLFilters = {
    ...state.graphQLFilters,
    ...GraphQLHelper.getGraphQLQueryPageTableOptions<T>(updatedGraphQLQuery, state.graphQLResponse.result?.pageInfo),
    where: updatedWhere,
    order: queryOrder,
  };

  return {
    graphQLQuery: updatedGraphQLQuery,
    graphQLFilters: updatedGraphQLFilters,
  };
}
