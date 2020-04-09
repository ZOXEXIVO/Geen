import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { VoteComponent } from './vote.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    VoteComponent,
  ],
  exports:[
    VoteComponent,
  ],
  imports: [
    BrowserModule,
    SharedModule,
    FormsModule
  ]
})
export class VoteModule { }
