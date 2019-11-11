import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { TopPlayersWidgetComponent } from './top/top.players.widget.component';
import { ClubPlayerService } from '../pages/clubs/view/players/services/club.player.service';
import { TopClubPlayersWidgetComponent } from './topClub/top.club.players.widget.component';

@NgModule({
  declarations: [
    TopPlayersWidgetComponent,
    TopClubPlayersWidgetComponent
  ],
  exports:[
    TopPlayersWidgetComponent,
    TopClubPlayersWidgetComponent
  ],
  imports: [
    BrowserModule,
    RouterModule,
    SharedModule
  ],
  providers: [ClubPlayerService]
})
export class WidgetModule { }
