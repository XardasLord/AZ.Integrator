// Extended type definitions for keycloak-js compatibility
export interface AppKeycloakProfile {
  id?: string;
  username?: string;
  email?: string;
  firstName?: string;
  lastName?: string;
  enabled?: boolean;
  emailVerified?: boolean;
  totp?: boolean;
  createdTimestamp?: number;
  attributes?: AppKeycloakUserAttributes;
}

export interface AppKeycloakUserAttributes {
  tenant_id?: string[];
  tenant_name?: string[];
  [key: string]: string[] | undefined;
}

export interface AppKeycloakTokenParsed {
  exp?: number;
  iat?: number;
  nonce?: string;
  sub?: string;
  session_state?: string;
  realm_access?: { roles: string[] };
  resource_access?: { [key: string]: { roles: string[] } };
  [key: string]: unknown;
}
