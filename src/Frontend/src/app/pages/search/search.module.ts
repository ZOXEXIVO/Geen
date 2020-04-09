import { NgModule } from '@angular/core';
import { SearchResultComponent } from './results/search.results.component';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    SearchResultComponent
  ],
  imports: [  
    CommonModule,
    FormsModule,
    RouterModule,
    SharedModule
  ],
  providers: [
        
  ]
})
export class SearchModule { }
