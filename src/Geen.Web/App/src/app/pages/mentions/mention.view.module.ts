import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule } from '@angular/router';
import { MentionModule } from '../../components/mentions/mention.module';
import { ClubPlayerService } from '../clubs/view/players/services/club.player.service';
import { MentionClubViewComponent } from './view/club/mention.club.view';
import { MentionPlayerViewComponent } from './view/player/mention.player.view';
import { ReplyModule } from '../../components/replies/reply.module';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

@NgModule({
  declarations: [
    MentionClubViewComponent,
    MentionPlayerViewComponent
  ],
  imports: [
    BrowserModule,
    RouterModule,
    MentionModule,
    ReplyModule,
    SharedModule,
    InfiniteScrollModule,
  ],
  providers: [
    ClubPlayerService
  ]
})
export class MentionViewModule { }
