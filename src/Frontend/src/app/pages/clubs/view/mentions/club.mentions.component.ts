import { Component, OnInit } from '@angular/core';
import * as _ from 'underscore'
import { ActivatedRoute } from '@angular/router';
import { GetMentionListQuery, MentionModel, Client, ClubModel } from '../../../../../client/apiClient';
import { BehaviorSubject } from 'rxjs';
import { Title } from '@angular/platform-browser';
import { combineLatest } from 'rxjs';

@Component({
  selector: 'clubs',
  templateUrl: './club.mentions.component.html',
  styleUrls: ['./club.mentions.component.css']
})
export class ClubMentionComponent implements OnInit {
  mentions: MentionModel[];
  club: ClubModel;
  club$: BehaviorSubject<ClubModel> = new BehaviorSubject<ClubModel>(null);
  
  clubUrlName: string;

  nothingToScroll: boolean = false;
  currentPage: number = 1;

  isBusy: boolean = false;
  isScrollBusy: boolean = false;

  constructor(private client: Client,
    private route: ActivatedRoute, 
    private titleService: Title) {
  }

  ngOnInit() {
    combineLatest(
      this.route.parent.params,
      this.route.params
    ).subscribe(([parentParams, params]) => {
      var query = new GetMentionListQuery();

      if(params.page){
         this.currentPage = parseInt(params.page);
      }
      
      this.clubUrlName = parentParams.urlName;

      query.page = this.currentPage;
      query.clubUrlName = this.clubUrlName

      var queryStr = JSON.stringify(query);

      this.isBusy = true;

      this.client.apiMentionList(queryStr).subscribe(mentions => {
        if(mentions.length < 30)
          this.nothingToScroll = true;

        this.mentions = mentions;
        this.isBusy = false;
      });

      this.client.apiClub(parentParams.urlName).subscribe(club =>{
        this.setTitle(club, this.currentPage);
        this.club$.next(club);
     });
    });
  }

  private setTitle(club: ClubModel, page: number){   
    var title = club.isNational ? '' : 'ФК ';

    title += club.name + ', Гостевая книга болельщиков';

    if(page > 1){
      title += ' - страница ' + page;
    }

    this.titleService.setTitle(title + ' | GEEN');
  }

  onScrollDown() {
    if (this.nothingToScroll || this.isScrollBusy || this.isBusy)
      return;
      
    var query = new GetMentionListQuery();

    query.page = ++this.currentPage;
    query.clubUrlName = this.clubUrlName;

    var queryStr = JSON.stringify(query);

    this.isScrollBusy = true;

    this.client.apiMentionList(queryStr).subscribe(mentions => {
      if(mentions.length < 30)
        this.nothingToScroll = true;

      this.mentions.push(...mentions);
      this.isScrollBusy = false;
    });
  }
}
