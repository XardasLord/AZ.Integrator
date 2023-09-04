import { User } from 'oidc-client';
import { UserAuthModel } from '../models/user-auth.model';
import { AuthScopes } from '../models/auth.scopes';
import { JwtHelperService } from '@auth0/angular-jwt';

export class UserAuthHelper {
  public static parseUserAuthData(user: User): UserAuthModel | null {
    if (!user) {
      console.error('UserData not defined!');
      return null;
    }

    const helper = new JwtHelperService();

    const decodedToken = helper.decodeToken<UserAuthModel>(user.access_token);

    return decodedToken;
  }

  public static getScopes(keys: string[]): number[] {
    if (keys == null || keys.length === 0) {
      return [];
    }

    return keys.map(x => (AuthScopes as any)[x]);
  }
}
