import { Component } from '@angular/core';
import { Router, NavigationEnd } from '../../node_modules/@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  constructor(private router: Router) {
    router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        window.scroll(0, 0);
        if (event.url) {
          this.setPageView(event.url);
        }
      }
    });
  }

  setPageView(url: string) {
    if(url && url.indexOf('admin') != -1){
      console.log('admin skipped');
      return;
    }

    try {
      (<any>window).ga('set', 'page', url);

      if ((<any>window).yaCounter45096147) {
        (<any>window).yaCounter45096147.hit(url);
      }
    }
    catch (ex) {
    }
  }
}
