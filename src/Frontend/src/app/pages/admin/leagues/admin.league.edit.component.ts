import { Component, OnInit } from '@angular/core';
import { PlayerModel, LeagueModel, CountryModel, AdminLeagueClient } from '../../../../client/apiClient';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  templateUrl: './admin.league.edit.component.html'
})
export class AdminLeagueEditComponent implements OnInit {
  model: LeagueModel;
  countries: CountryModel[];

  constructor(private client: AdminLeagueClient, 
    private route: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params.id) {
        this.client.getAdminLeague(params.id).subscribe(data => {
          this.model = data;
        });
      }else{
        this.model = new LeagueModel();
      }
    });

    this.client.getAdminLeagueList().subscribe(countries => {
      this.countries = countries;
    });
  }

  save(){
    this.client.saveAdminLeague(this.model).subscribe(_ =>  {
      this.router.navigateByUrl('/admin/league/list');
    });
  }

  cancel(){
    this.router.navigateByUrl('/admin/league/list');
  }
}
