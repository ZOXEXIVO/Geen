import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MentionListComponent } from './list/mention.list.component';
import { MentionItemComponent } from './item/mention.item.component';
import { MentionLikeComponent } from './likes/mention.like.component';
import { MentionCommentComponent } from './comment/mention.comment.component';
import { SharedModule } from '../../shared/shared.module';
import { MentionHeaderComponent } from './header/mention.header.component';
import { RouterModule } from '@angular/router';
import { MentionCreateComponent } from './create/mention.create.component';

@NgModule({
  declarations: [
    MentionItemComponent,
    MentionListComponent,
    MentionLikeComponent,
    MentionCreateComponent,
    MentionCommentComponent,
    MentionHeaderComponent
  ],
  exports:[
    MentionItemComponent,
    MentionListComponent,
    MentionLikeComponent,
    MentionCreateComponent,
    MentionCommentComponent,
    MentionHeaderComponent
  ],
  imports: [
    BrowserModule,
    SharedModule,
    RouterModule,
    FormsModule
  ]
})
export class MentionModule { }
