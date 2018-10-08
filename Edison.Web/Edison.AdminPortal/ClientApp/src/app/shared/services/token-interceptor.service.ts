import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Store, select } from '@ngrx/store';
import { AppState } from '../../reducers';
import { authTokenSelector } from '../../reducers/auth/auth.selectors';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class TokenInterceptorService implements HttpInterceptor {
  private token: string;

  constructor(private store: Store<AppState>, private authService: AuthService) {
    store.pipe(select(authTokenSelector)).subscribe(token => this.token = token);
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const req = request.clone({
      setHeaders: {
        Authorization: `Bearer ${this.token}`
      }
    });

    this.authService.refreshToken();

    return next.handle(req);
  }
}
