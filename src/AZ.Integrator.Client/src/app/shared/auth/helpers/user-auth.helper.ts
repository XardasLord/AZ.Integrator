import { UserAuthModel } from '../models/user-auth.model';
import { AuthScopes } from '../models/auth.scopes';
import { JwtHelperService } from '@auth0/angular-jwt';

export class UserAuthHelper {
  public static parseAccessToken(accessToken: string): UserAuthModel | null {
    if (!accessToken) {
      console.error('Access Token not defined!');
      return null;
    }

    const helper = new JwtHelperService();

    const user = helper.decodeToken<UserAuthModel>(accessToken);

    if (user) {
      user.access_token = accessToken;
    }

    return user;
  }

  public static getScopes(keys: string[]): number[] {
    if (keys == null || keys.length === 0) {
      return [];
    }

    return keys.map(x => (AuthScopes as any)[x]);
  }
}
