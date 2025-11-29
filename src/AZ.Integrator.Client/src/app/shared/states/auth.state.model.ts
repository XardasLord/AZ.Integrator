import { AppKeycloakProfile } from '../../types/keycloak-js';
import { AuthScopes } from '../auth/models/auth.scopes';
import { AuthRoles } from '../auth/models/auth.roles';

export interface AuthStateModel {
  profile: AppKeycloakProfile | null;
  isLoggedIn: boolean;
  authScopes: AuthScopes[];
  authRoles: AuthRoles[];
  redirectUrl?: string;
}
