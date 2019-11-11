import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule } from '@angular/router';
import { ReplyItemComponent } from './item/reply.item.component';
import { ReplyListComponent } from './list/reply.list.component';
import { ReplyCreateComponent } from './create/reply.create.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    ReplyItemComponent,
    ReplyListComponent,
    ReplyCreateComponent
  ],
  exports:[
    ReplyItemComponent,
    ReplyListComponent,
    ReplyCreateComponent
  ],
  imports: [
    BrowserModule,
    SharedModule,
    RouterModule,
    FormsModule
  ]
})
export class ReplyModule { }
