import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'menu',
  styleUrls: ['./menu.component.css'],
  templateUrl: './menu.component.html',
})
export class MenuComponent {
  isRoot = false;
  searchTextField: string;

  constructor(private router: Router) {
    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.isRoot = event.url == '/';
      }
    });
  }

  getLiveClass() {
    if (this.isRoot) {
      return 'router-link-exact-active';
    }
    return '';
  }

  searchClick(){
    if(!this.isSearchValid()){
      return;
    }

    const url = "/search/" + encodeURIComponent(this.searchTextField);

    this.router.navigateByUrl(url, {skipLocationChange: false});
  }

  isSearchValid(){
    if(!this.searchTextField){
      return false;
    }

    if(this.searchTextField.length < 2){
      return false;
    }

    return true;
  }
}