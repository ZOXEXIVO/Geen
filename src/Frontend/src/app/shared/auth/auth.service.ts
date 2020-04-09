
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, Subject } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    private userKey: string = "geen_user";
    private replaySubject: Subject<string> = new ReplaySubject(1);

    constructor(private cookieService: CookieService) {
        let user = this.cookieService.get(this.userKey);
        this.setUser(user);
    }

    getUser(): Observable<string> {
        return this.replaySubject;
    }

    setUser(user: string) {
        var currentUser = user;

        this.updateCookie(currentUser);
        this.replaySubject.next(currentUser);
    }

    updateCookie(user: string){
        this.cookieService.set(this.userKey, user, 365 * 100, '/');
    }
}