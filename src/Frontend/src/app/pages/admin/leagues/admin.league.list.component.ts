import { Component, OnInit } from '@angular/core';
import { AdminLeagueClient, LeagueModel } from '../../../../client/apiClient';

@Component({
  templateUrl: './admin.league.list.component.html'
})
export class AdminLeagueListComponent implements OnInit {
  leagues: LeagueModel[];

  constructor(private client: AdminLeagueClient) {
  }

  ngOnInit() {
    this.client.getAdminLeagueList().subscribe(leagues => {
      this.leagues = leagues;
    })
  }
}
