import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { routing } from './app.routes';

import { AppComponent } from './app.component';
import { GlobalErrorHandler } from './shared/errorHandling/globalErrorHandler';
import { HttpClientModule } from '@angular/common/http';

import { HomeModule } from './pages/home/home.module';
import { ClubModule } from './pages/clubs/club.module';
import { RouterModule } from '@angular/router';
import { PlayerModule } from './pages/players/player.module';
import { MentionViewModule } from './pages/mentions/mention.view.module';
import { VoteModule } from './components/votes/vote.module';
import { AuthService } from './shared/auth/auth.service';
import { CookieService } from 'ngx-cookie-service';
import { SharedModule } from './shared/shared.module';
import { MenuComponent } from './shared/menu/menu.component';
import { FormsModule } from '@angular/forms';
import { SearchModule } from './pages/search/search.module';

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent
  ],
  imports: [   
    HomeModule,
    ClubModule,
    PlayerModule,
    MentionViewModule,
    VoteModule,
    BrowserModule,
    HttpClientModule,
    RouterModule,
    SharedModule,
    FormsModule,
    SearchModule,
    routing
  ], 
  providers: [   
    {
      provide: ErrorHandler, 
      useClass: GlobalErrorHandler
    },
    AuthService,
    CookieService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
