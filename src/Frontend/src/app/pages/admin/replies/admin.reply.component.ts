import { Component, OnInit } from '@angular/core';
import { ReplyModel, GetReplyUnapprovedListQuery, AdminReplyClient } from '../../../../client/apiClient';

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

  constructor(private client: AdminReplyClient) {
    this.query = new GetReplyUnapprovedListQuery();
    this.query.isApproved = false;    
  }

  ngOnInit() {
    this.query.isApproved = !this.onlyApproved;
    this.client.unapproved(this.query).subscribe(replies => {
      this.replies = replies;
    })
  }

  approve(reply: ReplyModel){
    this.client.approveReply(reply.id).subscribe(_ => {
        reply.isApproved = true;
    });
  }

  disapprove(reply: ReplyModel){
    this.client.disapproveReply(reply.id).subscribe(_ => {
      reply.isApproved = false;
    });
  }
  
  remove(reply: ReplyModel){
    this.client.removeReply(reply.id).subscribe(_ => {

    });
  }
  
  onScrollDown() {
    if (this.nothingToScroll || this.isBusy)
      return;
      
    var query = new GetReplyUnapprovedListQuery();

    query.page = ++this.currentPage;

    this.client.unapproved(query).subscribe(Replys => {
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
