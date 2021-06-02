import { Component, OnInit } from '@angular/core';
import { ClubModel, LeagueModel, AdminClubClient, AdminLeagueClient } from '../../../../client/apiClient';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  templateUrl: './admin.club.edit.component.html'
})
export class AdminClubEditComponent implements OnInit {
  model: ClubModel;
  leagues: LeagueModel[];

  constructor(private clubClient: AdminClubClient, 
    private leagueClient: AdminLeagueClient, 
    private route: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params.id) {
        this.clubClient.getAdminClub(params.id).subscribe(data => {
          this.model = data;
        });
      }else{
        this.model = new ClubModel();

        this.clubClient.getAdminClubNextId().subscribe(nextId => {
          this.model.id = nextId;
        });
      }
    });

    this.leagueClient.getAdminLeagueList().subscribe(leagues => {
      this.leagues = leagues;
    });
  }

  save(){
    this.clubClient.saveAdminClub(this.model).subscribe(_ =>  {
      this.router.navigateByUrl('/admin/club/list');
    });
  }

  cancel(){
    this.router.navigateByUrl('/admin/club/list');
  }
}
