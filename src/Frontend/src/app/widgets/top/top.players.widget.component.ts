import { Component, OnInit } from '@angular/core';
import { PlayerModel, PlayerClient } from '../../../client/apiClient';
import { ClubPlayerService } from '../../pages/clubs/view/players/services/club.player.service';

@Component({
  selector: 'top-players',
  templateUrl: './top.players.widget.component.html'
})
export class TopPlayersWidgetComponent implements OnInit {
  players: PlayerModel[];

  constructor(private client: PlayerClient, private playerService: ClubPlayerService) {
  }

  ngOnInit() {
    this.client.topPlayers().subscribe(data => {
      this.players = data;
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
