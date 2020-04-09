import { Component, OnInit } from '@angular/core';
import { Client, ReplyModel, GetReplyUnapprovedListQuery } from '../../../../client/apiClient';

@Component({
  templateUrl: './admin.reply.component.html',
  styleUrls: ['./admin.reply.component.css']
})
export class AdminRepliesComponent implements OnInit {
  replies: ReplyModel[];
  query: GetReplyUnapprovedListQuery;

  onlyApproved: boolean = true;

  nothingToScroll: boolean = false;
  currentPage: number = 1;

  isBusy: boolean = false;

  constructor(private client: Client) {
    this.query = new GetReplyUnapprovedListQuery();
    this.query.isApproved = false;    
  }

  ngOnInit() {
    this.query.isApproved = !this.onlyApproved;
    this.client.apiAdminReplyUnapprovedlist(JSON.stringify(this.query)).subscribe(replies => {
      this.replies = replies;
    })
  }

  approve(reply: ReplyModel){
    this.client.apiAdminReplyApprove(reply.id).subscribe(_ => {
        reply.isApproved = true;
    });
  }

  disapprove(reply: ReplyModel){
    this.client.apiAdminReplyDisapprove(reply.id).subscribe(_ => {
      reply.isApproved = false;
    });
  }
  
  remove(reply: ReplyModel){
    this.client.apiAdminReplyRemove(reply.id).subscribe(_ => {

    });
  }
  
  onScrollDown() {
    if (this.nothingToScroll || this.isBusy)
      return;
      
    var query = new GetReplyUnapprovedListQuery();

    query.page = ++this.currentPage;

    var queryStr = JSON.stringify(query);

    this.client.apiAdminReplyUnapprovedlist(queryStr).subscribe(Replys => {
      if(Replys.length < 30)
        this.nothingToScroll = true;

      this.replies.push(...Replys);
    });
  }

  onFilterChange(){
    this.currentPage = 1;
    this.ngOnInit();
  }
}
