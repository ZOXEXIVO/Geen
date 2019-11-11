import { Routes } from '@angular/router';
import { PlayerViewComponent } from './view/player.view.component';

export const playerRoutes: Routes = [
  {
    path: 'player/:urlName',
    component: PlayerViewComponent
  },
  {
    path: 'player/:urlName/page/:page',
    component: PlayerViewComponent
  } 
];