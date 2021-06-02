import { Component, OnInit } from '@angular/core';
import { combineLatest } from 'rxjs';
import * as _ from 'underscore'
import { LeagueModel, ClubModel, ClubClient, LeagueClient } from '../../../../../client/apiClient';

@Component({
  selector: 'clubs',
  templateUrl: './club.list.component.html',
  styleUrls: ['./club.list.component.css']
})
export class ClubListComponent implements OnInit {
  isBusy: Boolean = true;

  clubs: ClubModel[];
  leagues: LeagueModel[];
  clubGroups: GroupedClubs[] = [];

  constructor(private client: ClubClient, private leagueClient: LeagueClient) {
  }

  ngOnInit() {

    combineLatest(
      this.client.listAll(),
      this.leagueClient.list()
    ).subscribe(([clubs, leagues]) => {
      this.clubs = clubs;
      this.leagues = leagues;

      const groups = _.groupBy(this.clubs, 'leagueId');
      
      for(var leagueId in groups) {
        var group = new GroupedClubs();

        group.LeagueId = parseInt(leagueId, 10);
        group.Clubs = groups[leagueId];

        this.clubGroups.push(group);
     }

     this.isBusy = false;
    });
  }

  getLeagueName(leagueId) {
    if (!this.leagues) {
      return '-';
    }

    var league = _.find(this.leagues, {
      id: (leagueId * 1)
    });
    if (league) {
      return league.name;
    }
    return '-';
  }
}


export class GroupedClubs{
   public LeagueId : number;
   public Clubs: ClubModel[];
}