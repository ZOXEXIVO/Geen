import { Component, OnInit } from '@angular/core';
import { Client, LoginModel } from '../../../client/apiClient';
import { CookieService } from 'ngx-cookie-service';

@Component({
  templateUrl: './admin.component.html'
})
export class AdminComponent  implements OnInit {
  isAuthenticated: boolean = false;

  loginModel: LoginModel = new LoginModel();

  tokenName: string  = 'geen_admin_auth';

  constructor(private client: Client, private cookieService: CookieService) {
  }

  ngOnInit() { 
    this.isAuthenticated = !!this.cookieService.get(this.tokenName);
  }

  login() {
    this.client.apiAuthenticationLogin(this.loginModel).subscribe(token => {    
      if(token){
        this.isAuthenticated = true;
      }      
    })
  }
}