import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HomeComponent } from './home.component';
import { MentionModule } from '../../components/mentions/mention.module';
import { SharedModule } from '../../shared/shared.module';
import { WidgetModule } from '../../widgets/widget.module';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { VoteModule } from 'src/app/components/votes/vote.module';

@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    BrowserModule,
    MentionModule,
    SharedModule,
    VoteModule,
    WidgetModule,
    InfiniteScrollModule,
  ]
})
export class HomeModule { }
