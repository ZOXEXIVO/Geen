import { Routes } from '@angular/router';
import { MentionPlayerViewComponent } from './view/player/mention.player.view';
import { MentionClubViewComponent } from './view/club/mention.club.view';

export const mentionViewRoutes: Routes = [
  {
    path: 'player/:urlName/:mentionId',
    component: MentionPlayerViewComponent
  },
  {
    path: 'player/:urlName/:mentionId/page/:page',
    component: MentionPlayerViewComponent
  },
  {
    path: 'club/:urlName/:mentionId',
    component: MentionClubViewComponent
  },
  {
    path: 'club/:urlName/:mentionId/page/:page',
    component: MentionClubViewComponent
  } 
];