import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, catchError, map, Observable, switchMap, tap } from 'rxjs';
import { TokenStorage } from './token-storage.service';
import { UserModel } from './user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  public readonly ApiUrl = 'https://localhost:7039';
  private userSubject: BehaviorSubject<any>;
  public user: Observable<any>;

  constructor(
    private http: HttpClient,
    private tokenStorage: TokenStorage
  ) {
    this.userSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('user') ?? ''));

    this.user = this.userSubject.asObservable();
  }

  public get userValue(): any {
    return this.userSubject.value;
  }

  public isAuthorized(): Observable<boolean> {
    return this.tokenStorage
      .getAccessToken()
      .pipe(map(token => !!token));
  }

  public getAccessToken(): Observable<string> {
    return this.tokenStorage.getAccessToken();
  }

  public refreshShouldHappen(response: HttpErrorResponse): boolean {
    return response.status === 401
  }

  public login(_username: string, _password: string): Observable<UserModel> {
    return this.http
      .post<UserModel>(`${this.ApiUrl}/api/auth/login`, { name: _username, password: _password })
      .pipe(
        tap((user: UserModel) => {
          this.saveAccessData(user.token);
          localStorage.setItem("user", JSON.stringify(user));
          this.userSubject = new BehaviorSubject<UserModel>(JSON.parse(localStorage.getItem('user') ?? ''));
      }));
  }

  public logout(): void {
    this.tokenStorage.clear();
    localStorage.removeItem("user");
    this.userSubject.next(null);
    location.reload();
  }

  private saveAccessData(accessToken: string): void {
    this.tokenStorage
      .setAccessToken(accessToken);
  }
}
