import { Component, OnInit } from '@angular/core';
import { Client, MentionModel, GetMentionListQuery } from '../../../../client/apiClient';

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

  constructor(private client: Client) {
    this.query = new GetMentionListQuery();
    this.query.isApproved = false;    
  }

  ngOnInit() {
    this.query.isApproved = !this.onlyApproved;
    this.client.apiAdminMentionUnapprovedlist(JSON.stringify(this.query)).subscribe(mentions => {
      this.mentions = mentions;
    })
  }

  approve(mention: MentionModel){
    this.client.apiAdminMentionApprove(mention.id).subscribe(_ => {
        mention.isApproved = true;
    });
  }

  disapprove(mention: MentionModel){
    this.client.apiAdminMentionDisapprove(mention.id).subscribe(_ => {
      mention.isApproved = false;
    });
  }
  
  remove(mention: MentionModel){
    this.client.apiAdminMentionRemove(mention.id).subscribe(_ => {

    });
  }

  setAnonymousAvatar(mention: MentionModel){
    this.client.apiAdminMentionSetanonymousavatar(mention.id).subscribe(_ => {
        
    });
  }

  
  onScrollDown() {
    if (this.nothingToScroll || this.isBusy)
      return;
      
    var query = new GetMentionListQuery();

    query.page = ++this.currentPage;

    var queryStr = JSON.stringify(query);

    this.client.apiAdminMentionUnapprovedlist(queryStr).subscribe(mentions => {
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
