import { Routes } from '@angular/router';
import { SearchResultComponent } from './results/search.results.component';

export const searchRoutes: Routes = [
  {
    path: 'search/:searchText',
    component: SearchResultComponent
  } 
];