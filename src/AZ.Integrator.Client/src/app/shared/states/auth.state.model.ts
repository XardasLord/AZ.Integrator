import { KeycloakProfile } from 'keycloak-js/lib/keycloak';
import { AuthScopes } from '../auth/models/auth.scopes';
import { AuthRoles } from '../auth/models/auth.roles';

export interface AuthStateModel {
  profile: KeycloakProfile | null;
  isLoggedIn: boolean;
  authScopes: AuthScopes[];
  authRoles: AuthRoles[];
}
