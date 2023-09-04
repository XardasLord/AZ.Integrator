import { NgModule } from '@angular/core';
import { HttpLink } from 'apollo-angular/http';
import { InMemoryCache } from '@apollo/client/core';
import { APOLLO_OPTIONS, ApolloModule } from 'apollo-angular';
import { environment } from 'src/environments/environment';

@NgModule({
  imports: [ApolloModule],
  providers: [
    {
      provide: APOLLO_OPTIONS,
      useFactory: (httpLink: HttpLink) => {
        return {
          cache: new InMemoryCache(),
          link: httpLink.create({
            uri: environment.graphqlEndpoint,
          }),
          defaultOptions: {
            watchQuery: {
              fetchPolicy: 'no-cache',
            },
          },
        };
      },
      deps: [HttpLink],
    },
  ],
})
export class AppGraphQLModule {}

export const GraphQLIntegratorClientName = 'default';
