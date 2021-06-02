import { Component, OnInit } from '@angular/core';
import { MentionModel, GetMentionListQuery, AdminMentionClient } from '../../../../client/apiClient';

@Component({
  templateUrl: './admin.mention.component.html',
  styleUrls: ['./admin.mention.component.css']
})
export class AdminMentionsComponent implements OnInit {
  mentions: MentionModel[];
  query: GetMentionListQuery;

  onlyApproved: boolean = true;

  nothingToScroll: boolean = false;
  currentPage: number = 1;

  isBusy: boolean = false;

  constructor(private client: AdminMentionClient) {
    this.query = new GetMentionListQuery();
    this.query.isApproved = false;    
  }

  ngOnInit() {
    this.query.isApproved = !this.onlyApproved;
    this.client.unapprovedAll(this.query).subscribe(mentions => {
      this.mentions = mentions;
    })
  }

  approve(mention: MentionModel){
    this.client.approve(mention.id).subscribe(_ => {
        mention.isApproved = true;
    });
  }

  disapprove(mention: MentionModel){
    this.client.disapprove(mention.id).subscribe(_ => {
      mention.isApproved = false;
    });
  }
  
  remove(mention: MentionModel){
    this.client.mentionDelete(mention.id).subscribe(_ => {

    });
  }

  setAnonymousAvatar(mention: MentionModel){
    this.client.anonymousAvatar(mention.id).subscribe(_ => {
        
    });
  }

  
  onScrollDown() {
    if (this.nothingToScroll || this.isBusy)
      return;
      
    var query = new GetMentionListQuery();

    query.page = ++this.currentPage;

    this.client.unapprovedAll(query).subscribe(mentions => {
      if(mentions.length < 30)
        this.nothingToScroll = true;

      this.mentions.push(...mentions);
    });
  }

  onFilterChange(){
    this.currentPage = 1;
    this.ngOnInit();
  }
}
