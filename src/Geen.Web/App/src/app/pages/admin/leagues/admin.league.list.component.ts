import { Component, OnInit } from '@angular/core';
import { Client, LeagueModel } from '../../../../client/apiClient';

@Component({
  templateUrl: './admin.league.list.component.html'
})
export class AdminLeagueListComponent implements OnInit {
  leagues: LeagueModel[];

  constructor(private client: Client) {
  }

  ngOnInit() {
    this.client.apiLeagueList().subscribe(leagues => {
      this.leagues = leagues;
    })
  }
}
