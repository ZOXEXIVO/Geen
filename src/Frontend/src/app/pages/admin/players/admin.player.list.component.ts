import { Component, OnInit } from '@angular/core';
import { PlayerModel, PlayerGetListQuery, AdminPlayerClient } from '../../../../client/apiClient';

@Component({
  templateUrl: './admin.player.list.component.html'
})
export class AdminPlayerListComponent implements OnInit {
  players: PlayerModel[];
  query: PlayerGetListQuery = new PlayerGetListQuery();

  constructor(private client: AdminPlayerClient) {
  }

  ngOnInit() {
    this.client.playerList(this.query).subscribe(players => {
      this.players = players;
    })
  }

  onFilterChange(){
    this.ngOnInit();
  }

  getPlayerPhotoUrl(player){
    return 'https://storage.geen.one/geen/' + player.id + '.jpg';
  }
}
