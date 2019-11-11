import { NgModule } from '@angular/core';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { AdminClubListComponent } from './clubs/admin.club.list.component';
import { AdminComponent } from './admin.component';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { adminRouting } from './admin.routes';
import { CookieService } from 'ngx-cookie-service';
import { FormsModule } from '@angular/forms';
import { AdminClubEditComponent } from './clubs/admin.club.edit.component';
import { AdminLeagueEditComponent } from './leagues/admin.league.edit.component';
import { AdminLeagueListComponent } from './leagues/admin.league.list.component';
import { AdminCountryListComponent } from './countries/admin.country.list.component';
import { AdminCountryEditComponent } from './countries/admin.country.edit.component';
import { AdminPlayerListComponent } from './players/admin.player.list.component';
import { AdminPlayerEditComponent } from './players/admin.player.edit.component';
import { AdminTitlesComponent } from './titles/admin.titles.component';
import { AdminMentionsComponent } from './mentions/admin.mention.component';
import { AdminRepliesComponent } from './replies/admin.reply.component';

@NgModule({
  declarations: [
    AdminComponent,
    AdminClubListComponent,
    AdminClubEditComponent,
    AdminLeagueListComponent,
    AdminLeagueEditComponent,
    AdminCountryListComponent,
    AdminCountryEditComponent,
    AdminPlayerListComponent,
    AdminPlayerEditComponent,
    AdminTitlesComponent,
    AdminMentionsComponent,
    AdminRepliesComponent
  ],
  imports: [
    RouterModule,
    CommonModule,
    FormsModule,
    InfiniteScrollModule,
    adminRouting
  ],
  providers: [
    CookieService
  ]
})
export class AdminModule { }
