import { UserAuthModel } from '../auth/models/user-auth.model';

const prefix = '[Auth]';

export class Login {
  static readonly type = `${prefix} ${Login.name}`;
}
export class LoginViaErli {
  static readonly type = `${prefix} ${LoginViaErli.name}`;

  constructor(public tenantId: string) {}
}

export class LoginCompleted {
  static readonly type = `${prefix} ${LoginCompleted.name}`;

  constructor(public user: UserAuthModel) {}
}

export class Logout {
  static readonly type = `${prefix} ${Logout.name}`;
}

export class NotAuthorized {
  static readonly type = `${prefix} ${NotAuthorized.name}`;
}

export class Relog {
  static readonly type = `${prefix} ${Relog.name}`;
}
