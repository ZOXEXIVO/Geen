import { ModuleWithProviders }  from '@angular/core';  
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { clubRoutes } from './pages/clubs/club.routes';
import { playerRoutes } from './pages/players/player.routes';
import { mentionViewRoutes } from './pages/mentions/mention.view.routes';
import { searchRoutes } from './pages/search/search.routes';
import { AppModule } from './app.module';

export const routes: Routes = [  
    {  
      path: '', 
      component: HomeComponent,
    },
    {
      path: 'admin',
      loadChildren: './pages/admin/admin.module#AdminModule'
    },
    {  
      path: 'page/:page', 
      component: HomeComponent,
    },
    ...clubRoutes,
    ...playerRoutes,
    ...mentionViewRoutes,
    ...searchRoutes
  ];  
    
  export const routing: ModuleWithProviders<AppModule> = RouterModule.forRoot(routes);  