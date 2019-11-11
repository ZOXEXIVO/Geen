import { Component, OnInit } from '@angular/core';
import { ClubModel, Client, LeagueModel } from '../../../../client/apiClient';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  templateUrl: './admin.club.edit.component.html'
})
export class AdminClubEditComponent implements OnInit {
  model: ClubModel;
  leagues: LeagueModel[];

  constructor(private client: Client, 
    private route: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params.id) {
        this.client.apiAdminClubGet(params.id).subscribe(data => {
          this.model = data;
        });
      }else{
        this.model = new ClubModel();

        this.client.apiAdminClubNextid().subscribe(nextId => {
          this.model.id = nextId;
        });
      }
    });

    this.client.apiAdminLeagueList().subscribe(leagues => {
      this.leagues = leagues;
    });
  }

  save(){
    this.client.apiAdminClubSave(this.model).subscribe(_ =>  {
      this.router.navigateByUrl('/admin/club/list');
    });
  }

  cancel(){
    this.router.navigateByUrl('/admin/club/list');
  }
}
