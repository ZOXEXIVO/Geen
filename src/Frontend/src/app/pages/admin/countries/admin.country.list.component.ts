import { Component, OnInit } from '@angular/core';
import { AdminCountryClient, CountryModel } from '../../../../client/apiClient';

@Component({
  templateUrl: './admin.country.list.component.html'
})
export class AdminCountryListComponent implements OnInit {
  countries: CountryModel[];

  constructor(private client: AdminCountryClient) {
  }

  ngOnInit() {
    this.client.getAdminCountryList().subscribe(countries => {
      this.countries = countries;
    })
  }
}
