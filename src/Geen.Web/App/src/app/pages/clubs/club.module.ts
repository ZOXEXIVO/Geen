import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { ClubViewComponent } from './view/club.view.component';
import { ClubListComponent } from './view/list/club.list.component';
import { ClubMentionComponent } from './view/mentions/club.mentions.component';
import { RouterModule } from '@angular/router';
import { MentionModule } from '../../components/mentions/mention.module';
import { ClubPlayerService } from './view/players/services/club.player.service';
import { ClubPlayerComponent } from './view/players/club.players.component';
import { WidgetModule } from '../../widgets/widget.module';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

@NgModule({
  declarations: [
    ClubListComponent,
    ClubViewComponent,
    ClubPlayerComponent,
    ClubMentionComponent,
  ],
  imports: [
    BrowserModule,
    RouterModule,
    MentionModule,
    SharedModule,
    WidgetModule,
    InfiniteScrollModule,
  ],
  providers: [
    ClubPlayerService,
  ]
})
export class ClubModule { }
