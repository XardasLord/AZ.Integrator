import { AuthRoles } from '../models/auth.roles';
import { KeycloakTokenParsed } from '../../../types/keycloak-js';

export function getAuthRolesFromToken(tokenParsed: KeycloakTokenParsed): AuthRoles[] {
  return <AuthRoles[]>tokenParsed['roles'];
}
