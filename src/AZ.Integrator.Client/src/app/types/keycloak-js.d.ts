// Extended type definitions for keycloak-js compatibility
export interface KeycloakProfile {
  id?: string;
  username?: string;
  email?: string;
  firstName?: string;
  lastName?: string;
  enabled?: boolean;
  emailVerified?: boolean;
  totp?: boolean;
  createdTimestamp?: number;
}

export interface KeycloakTokenParsed {
  exp?: number;
  iat?: number;
  nonce?: string;
  sub?: string;
  session_state?: string;
  realm_access?: { roles: string[] };
  resource_access?: { [key: string]: { roles: string[] } };
  [key: string]: unknown;
}
