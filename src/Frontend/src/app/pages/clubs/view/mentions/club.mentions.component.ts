import { Component, OnInit } from '@angular/core';
import * as _ from 'underscore'
import { ActivatedRoute } from '@angular/router';
import { GetMentionListQuery, MentionModel, ClubModel, MentionClient, ClubClient } from '../../../../../client/apiClient';
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

  constructor(private client: MentionClient,
    private clubClient: ClubClient,
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

      this.isBusy = true;

      this.client.getMentionList(query).subscribe(mentions => {
        if(mentions.length < 30)
          this.nothingToScroll = true;

        this.mentions = mentions;
        this.isBusy = false;
      });

      this.clubClient.club(parentParams.urlName).subscribe(club =>{
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

    this.titleService.setTitle(title + ' | Футбольные комментарии');
  }

  onScrollDown() {
    if (this.nothingToScroll || this.isScrollBusy || this.isBusy)
      return;
      
    var query = new GetMentionListQuery();

    query.page = ++this.currentPage;
    query.clubUrlName = this.clubUrlName;

    this.isScrollBusy = true;

    this.client.getMentionList(query).subscribe(mentions => {
      if(mentions.length < 30)
        this.nothingToScroll = true;

      this.mentions.push(...mentions);
      this.isScrollBusy = false;
    });
  }
}
