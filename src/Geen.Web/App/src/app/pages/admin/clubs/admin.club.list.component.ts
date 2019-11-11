import { Component, OnInit } from '@angular/core';
import { ClubModel, Client } from '../../../../client/apiClient';

@Component({
  templateUrl: './admin.club.list.component.html'
})
export class AdminClubListComponent implements OnInit {
  clubs: ClubModel[];

  constructor(private client: Client) {
  }

  ngOnInit() {
    this.client.apiClubList().subscribe(clubs => {
      this.clubs = clubs;
    })
  }
}
