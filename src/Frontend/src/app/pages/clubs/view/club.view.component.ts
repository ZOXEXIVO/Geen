import { Component, OnInit } from '@angular/core';
import { ClubModel, PlayerModel, ClubClient } from '../../../../client/apiClient';
import * as _ from 'underscore'
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'club-view',
  templateUrl: './club.view.component.html',
  styleUrls: ['./club.view.component.css']
})
export class ClubViewComponent implements OnInit {
  model: ClubModel;
  coach: PlayerModel;

  constructor(private client: ClubClient, 
    private route: ActivatedRoute) {
  }

  ngOnInit() {    
    this.route.params.subscribe(params => {
      this.client.club(params.urlName).subscribe(data =>{
         this.model = data;        
      });

      this.client.coach(params.urlName).subscribe(data =>{
        if(data.id){
          this.coach = data;
        }        
      });
    }); 
  }
}