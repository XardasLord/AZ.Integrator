import { AuthScopes } from './auth.scopes';

export class UserAuthModel {
  allegro_access_token: string | undefined;
  allegro_refresh_token: string | undefined;
  tenant_id: string | undefined;
  // role: string | undefined;
  auth_scopes: AuthScopes[] | undefined;
  access_token: string | undefined;

  constructor(init?: Partial<UserAuthModel>) {
    Object.assign(this, { ...init });
  }
}
