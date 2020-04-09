import { Component, OnInit } from '@angular/core';
import * as _ from 'underscore'
import { ActivatedRoute } from '@angular/router';
import { Client, PlayerModel } from '../../../../../client/apiClient';
import { ClubPlayerService } from './services/club.player.service';
import { Title } from '@angular/platform-browser';

@Component({
  templateUrl: './club.players.component.html',
  styleUrls: ['./club.players.component.css']
})
export class ClubPlayerComponent implements OnInit {
  isBusy: boolean = true;
  players: PlayerModel[];
  playerGroups: GroupedPlayers[] = [];
  averageAge: string;

  constructor(private client: Client,
    private route: ActivatedRoute,
    private service: ClubPlayerService,
    private titleService: Title) {
  }

  ngOnInit() {
    this.route.parent.params.subscribe(params => {
      this.client.apiPlayersClub(params.urlName).subscribe((players: PlayerModel[]) => {
        const playerGroups = _.groupBy(players, 'position');

        for (var positionId in playerGroups) {
          var group = new GroupedPlayers();

          group.PositionId = parseInt(positionId, 10);
          group.Players = playerGroups[positionId];

          this.playerGroups.push(group);
        }

        this.isBusy = false;
      });


      this.client.apiClub(params.urlName).subscribe(club => {
        this.titleService.setTitle('Состав команды ФК ' + club.name + ' | GEEN');
      });

      this.client.apiClubAgeAverage(params.urlName).subscribe((data: number) => {
        if (data) {
          this.averageAge = data + ' ' + this.service.pluralizeAge(data);
        }
      });
    });
  }

  getPositionName(position) {
    return this.service.getPositionName(position);
  }

  getPlayerPhotoUrl(player) {
    return this.service.getPlayerPhotoUrl(player);
  }

  getAge(player) {
    return this.service.getAge(player);
  }
}

export class GroupedPlayers {
  public PositionId: number;
  public Players: PlayerModel[];
}