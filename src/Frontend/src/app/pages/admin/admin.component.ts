import { Component, OnInit } from '@angular/core';
import { AuthenticationClient, LoginModel } from '../../../client/apiClient';
import { CookieService } from 'ngx-cookie-service';

@Component({
  templateUrl: './admin.component.html'
})
export class AdminComponent  implements OnInit {
  isAuthenticated: boolean = false;

  loginModel: LoginModel = new LoginModel();

  tokenName: string  = 'geen_admin_auth';

  constructor(private authenticationClient: AuthenticationClient, private cookieService: CookieService) {
  }

  ngOnInit() { 
    this.isAuthenticated = !!this.cookieService.get(this.tokenName);
  }

  login() {
    this.authenticationClient.login(this.loginModel).subscribe(token => {    
      if(token){
        this.isAuthenticated = true;
      }      
    })
  }
}