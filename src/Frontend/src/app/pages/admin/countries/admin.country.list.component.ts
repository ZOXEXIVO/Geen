import { Component, OnInit } from '@angular/core';
import { Client, CountryModel } from '../../../../client/apiClient';

@Component({
  templateUrl: './admin.country.list.component.html'
})
export class AdminCountryListComponent implements OnInit {
  countries: CountryModel[];

  constructor(private client: Client) {
  }

  ngOnInit() {
    this.client.apiAdminCountryList().subscribe(countries => {
      this.countries = countries;
    })
  }
}
