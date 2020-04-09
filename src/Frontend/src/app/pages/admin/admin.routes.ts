import { Routes, RouterModule } from '@angular/router';
import { AdminClubListComponent } from './clubs/admin.club.list.component';
import { AdminComponent } from './admin.component';
import { ModuleWithProviders } from '@angular/core';
import { AdminClubEditComponent } from './clubs/admin.club.edit.component';
import { AdminLeagueListComponent } from './leagues/admin.league.list.component';
import { AdminLeagueEditComponent } from './leagues/admin.league.edit.component';
import { AdminCountryEditComponent } from './countries/admin.country.edit.component';
import { AdminCountryListComponent } from './countries/admin.country.list.component';
import { AdminPlayerListComponent } from './players/admin.player.list.component';
import { AdminPlayerEditComponent } from './players/admin.player.edit.component';
import { AdminTitlesComponent } from './titles/admin.titles.component';
import { AdminMentionsComponent } from './mentions/admin.mention.component';
import { AdminRepliesComponent } from './replies/admin.reply.component';

export const adminRoutes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      //clubs
      {
        path: 'club/list',
        component: AdminClubListComponent
      },
      {
        path: 'club/create',
        component: AdminClubEditComponent
      },
      {
        path: 'club/edit/:id',
        component: AdminClubEditComponent
      },
      //leagues
      {
        path: 'league/list',
        component: AdminLeagueListComponent
      },
      {
        path: 'league/create',
        component: AdminLeagueEditComponent
      },
      {
        path: 'league/edit/:id',
        component: AdminLeagueEditComponent
      },
      //Counteries
      {
        path: 'country/list',
        component: AdminCountryListComponent
      },
      {
        path: 'country/create',
        component: AdminCountryEditComponent
      },
      {
        path: 'country/edit/:id',
        component: AdminCountryEditComponent
      },
      //Players
      {
        path: 'player/list',
        component: AdminPlayerListComponent
      },
      {
        path: 'player/create',
        component: AdminPlayerEditComponent
      },
      {
        path: 'player/edit/:id',
        component: AdminPlayerEditComponent
      },
      //Titles
      {
        path: 'titles',
        component: AdminTitlesComponent
      },
      //Mentions
      {
        path: 'mentions',
        component: AdminMentionsComponent
      },
      //Mentions
      {
        path: 'replies',
        component: AdminRepliesComponent
      }
    ]
  }
];

export const adminRouting: ModuleWithProviders = RouterModule.forChild(adminRoutes);  