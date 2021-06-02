import { Component, OnInit } from '@angular/core';
import { PlayerModel, CountryModel, AdminCountryClient } from '../../../../client/apiClient';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  templateUrl: './admin.country.edit.component.html'
})
export class AdminCountryEditComponent implements OnInit {
  model: CountryModel;

  constructor(private client: AdminCountryClient, 
    private route: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params.id) {
        this.client.getAdminCountry(params.id).subscribe(data => {
          this.model = data;
        });
      }else{
        this.model = new CountryModel();       
      }
    });
  }

  save(){
    this.client.saveAdminCountry(this.model).subscribe(_ =>  {
      this.router.navigateByUrl('/admin/country/list');
    });
  }

  cancel(){
    this.router.navigateByUrl('/admin/country/list');
  }
}
