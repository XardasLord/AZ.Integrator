import { UserAuthModel } from '../auth/models/user-auth.model';

export interface AuthStateModel {
  user: UserAuthModel | null;
}
