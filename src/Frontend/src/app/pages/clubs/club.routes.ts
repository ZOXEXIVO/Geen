import { Routes } from '@angular/router';
import { ClubViewComponent } from './view/club.view.component';
import { ClubMentionComponent } from './view/mentions/club.mentions.component';
import { ClubListComponent } from './view/list/club.list.component';
import { ClubPlayerComponent } from './view/players/club.players.component';

export const clubRoutes: Routes = [
  {
    path: 'clubs',
    component: ClubListComponent,
  },
  {
    path: 'club/:urlName',
    component: ClubViewComponent,
    children: [
      { path: '', component: ClubMentionComponent },
      { path: 'page/:page', component: ClubMentionComponent },
      { path: 'players', component: ClubPlayerComponent },
    ]
  }
];