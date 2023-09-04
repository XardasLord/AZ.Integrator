import { AuthScopes } from './auth.scopes';

export class UserAuthModel {
  unique_name!: string;
  userName!: string;
  fullName: string | undefined;
  email: string | undefined;
  role: string | undefined;
  auth_scopes: AuthScopes[] | undefined;
  regions: number[] | undefined;
  groups: number | undefined;
  init_objecttypefilters!: number;
  init_devicefilters!: number[];
  init_regionfilters!: number[];
  init_statusfilters!: number[];
  init_ipfilters!: number[];

  constructor(init?: Partial<UserAuthModel>) {
    Object.assign(this, { ...init });
  }
}
