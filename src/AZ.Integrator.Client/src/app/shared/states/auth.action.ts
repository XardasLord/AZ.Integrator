import { UserAuthModel } from '../auth/models/user-auth.model';

const prefix = '[Auth]';

export class Login {
  static readonly type = `${prefix} ${Login.name}`;
  constructor(public login: string, public password: string) {}
}

export class LoginCompleted {
  static readonly type = `${prefix} ${LoginCompleted.name}`;

  constructor(public response: UserAuthModel) {}
}

export class Logout {
  static readonly type = `${prefix} ${Logout.name}`;
}

export class NotAuthorized {
  static readonly type = `${prefix} ${NotAuthorized.name}`;
}
