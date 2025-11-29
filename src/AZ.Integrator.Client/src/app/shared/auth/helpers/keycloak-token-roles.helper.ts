import { AuthRoles } from '../models/auth.roles';
import { AppKeycloakTokenParsed } from '../../../types/keycloak-js';

export function getAuthRolesFromToken(tokenParsed: AppKeycloakTokenParsed): AuthRoles[] {
  return <AuthRoles[]>tokenParsed['roles'];
}
