import { Component, OnInit } from '@angular/core';
import { Client, PlayerModel, PlayerGetListQuery } from '../../../../client/apiClient';

@Component({
  templateUrl: './admin.player.list.component.html'
})
export class AdminPlayerListComponent implements OnInit {
  players: PlayerModel[];
  query: PlayerGetListQuery = new PlayerGetListQuery();

  constructor(private client: Client) {
  }

  ngOnInit() {
    this.client.apiAdminPlayerList(JSON.stringify(this.query)).subscribe(players => {
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
