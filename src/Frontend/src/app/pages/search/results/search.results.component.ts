import { Component, OnInit } from '@angular/core';
import * as _ from 'underscore'
import { ActivatedRoute } from '@angular/router';
import { ClubPlayerService } from '../../clubs/view/players/services/club.player.service';
import { Title } from '@angular/platform-browser';
import { PlayerModel, PlayerClient } from 'src/client/apiClient';
import { GroupedPlayers } from '../../clubs/view/players/club.players.component';

@Component({
  selector: 'search-results',
  templateUrl: './search.results.component.html',
  styleUrls: ['./search.results.component.css']
})
export class SearchResultComponent implements OnInit {
  searchText: string;

  players: PlayerModel[];
  playerGroups: GroupedPlayers[] = [];

  isBusy: boolean = false;
  isScrollBusy: boolean = false;

  constructor(private playerClient: PlayerClient,
    private route: ActivatedRoute,
    private playerService: ClubPlayerService,
    private titleService: Title) {
  }

  ngOnInit() { 
    this.route.params.subscribe(params => {
      this.isBusy = true;
    
      this.searchText = params.searchText;

      this.titleService.setTitle('Результаты поиска по "' + params.searchText + '"');
    
      this.playerClient.searchAll(params.searchText).subscribe((players: PlayerModel[]) => {
        this.playerGroups = [];
        this.players = [];

        const playerGroups = _.groupBy(players, 'position');

        for (var positionId in playerGroups) {
          var group = new GroupedPlayers();

          group.PositionId = parseInt(positionId, 10);
          group.Players = playerGroups[positionId];

          this.playerGroups.push(group);
        }

        this.isBusy = false;
      });
    });
  }
 
  getPositionName(position) {
    return this.playerService.getPosition(position);
  }

  getPlayerPhotoUrl(player) {
    return this.playerService.getPlayerPhotoUrl(player);
  }

  isCoach(player) {
    return player.position == 4;
  }

  getAge(player) {
    return this.playerService.getAge(player);
  }
}