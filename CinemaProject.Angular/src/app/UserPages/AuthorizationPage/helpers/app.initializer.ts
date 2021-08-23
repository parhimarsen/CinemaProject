import { ACCESS_TOKEN_KEY, AuthService } from '../services/auth.service';

export function appInitializer(authenticationService: AuthService) {
  return () =>
    new Promise((resolve) => {
      if (!localStorage.getItem(ACCESS_TOKEN_KEY)) {
        resolve('Need to authenticate');
      } else {
        const jwtToken = JSON.parse(
          atob(localStorage.getItem(ACCESS_TOKEN_KEY)!.split('.')[1])
        );
        const expires = new Date(jwtToken.exp * 1000);
        const timeout = expires.getTime() - Date.now();
        if (timeout <= 0) {
          authenticationService.refreshToken().subscribe().add(resolve);
        } else {
          authenticationService.startRefreshTokenTimer();
          resolve('Access_Token is not expired');
        }
      }
    });
}
