import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule } from '@angular/router';
import { MentionModule } from '../../components/mentions/mention.module';
import { PlayerViewComponent } from './view/player.view.component';
import { ClubPlayerService } from '../clubs/view/players/services/club.player.service';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

@NgModule({
  declarations: [
    PlayerViewComponent
  ],
  imports: [   
    RouterModule,
    BrowserModule,
    MentionModule,
    SharedModule,
    InfiniteScrollModule,
  ],
  providers: [
    ClubPlayerService
  ]
})
export class PlayerModule { }
