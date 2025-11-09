import { AuthRoles } from '../models/auth.roles';
import { KeycloakTokenParsed } from '../../../types/keycloak-js';

export function getAuthRolesFromToken(tokenParsed: KeycloakTokenParsed): AuthRoles[] {
  const rolesString = tokenParsed!['roles'] as string;

  return rolesString
    .slice(1, -1) // Usuwa pierwsze i ostatnie nawiasy kwadratowe
    .split(',') // Dzieli string po przecinku
    .map((role: string) => <AuthRoles>role.trim());
}
