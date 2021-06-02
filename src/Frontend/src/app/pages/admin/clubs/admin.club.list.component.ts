import { Component, OnInit } from '@angular/core';
import { ClubModel, AdminClubClient } from '../../../../client/apiClient';

@Component({
  templateUrl: './admin.club.list.component.html'
})
export class AdminClubListComponent implements OnInit {
  clubs: ClubModel[];

  constructor(private client: AdminClubClient) {
  }

  ngOnInit() {
    this.client.getAdminClubList().subscribe(clubs => {
      this.clubs = clubs;
    })
  }
}
