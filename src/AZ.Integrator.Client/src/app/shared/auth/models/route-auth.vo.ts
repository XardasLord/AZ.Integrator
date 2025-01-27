import { AuthRoles } from './auth.roles';

export interface UserAllowTerms {
  allowScopes?: number[];
  allowRoles?: AuthRoles[];
}

export class RouteAuthVo implements UserAllowTerms {
  public allowScopes?: number[];
  public allowRoles?: AuthRoles[];

  constructor(init?: Partial<RouteAuthVo>) {
    Object.assign(this, { ...init });
  }
}
