import { Component, OnInit, Input } from '@angular/core';
import { PlayerModel, PlayerClient, ClubModel } from '../../../client/apiClient';
import { ClubPlayerService } from '../../pages/clubs/view/players/services/club.player.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'top-club-players',
  templateUrl: './top.club.players.widget.component.html'
})
export class TopClubPlayersWidgetComponent implements OnInit {
  players: PlayerModel[];

  @Input() club: Observable<ClubModel>;

  constructor(private playerClient: PlayerClient, private playerService: ClubPlayerService) {
  }

  ngOnInit() {
    this.club.subscribe(item => {
      if(!item){
        return;
      }
      this.playerClient.topClubPlayers(item.urlName).subscribe(data => {
        this.players = data;
      });
    });
  }

  getPlayerPhotoUrl(player) {
    return this.playerService.getPlayerPhotoUrl(player);
  }

  getPlayerText(player) {
    return player.lastName + ' ' + player.firstName;
  }

  getClassName(index){
    var className = "col-xs-3 col-sm-2 col-md-2 col-lg-1 col-xl-1";
    if(index > 3)
      return className + ' hidden-xs';

    return className;
  }
}
