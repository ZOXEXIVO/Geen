import { Component, OnInit } from '@angular/core';
import { PlayerModel, LeagueModel, CountryModel, Client } from '../../../../client/apiClient';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  templateUrl: './admin.league.edit.component.html'
})
export class AdminLeagueEditComponent implements OnInit {
  model: LeagueModel;
  countries: CountryModel[];

  constructor(private client: Client, 
    private route: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params.id) {
        this.client.apiAdminLeagueGet(params.id).subscribe(data => {
          this.model = data;
        });
      }else{
        this.model = new LeagueModel();
      }
    });

    this.client.apiAdminCountryList().subscribe(countries => {
      this.countries = countries;
    });
  }

  save(){
    this.client.apiAdminLeagueSave(this.model).subscribe(_ =>  {
      this.router.navigateByUrl('/admin/league/list');
    });
  }

  cancel(){
    this.router.navigateByUrl('/admin/league/list');
  }
}
