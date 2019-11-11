import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClubModel, MentionModel, Client, GetReplyListQuery, ReplyModel, PlayerModel } from '../../../../../client/apiClient';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'mention-club-view',
  templateUrl: './mention.club.view.html',
  styleUrls: ['./mention.club.view.css']
})
export class MentionClubViewComponent implements OnInit {
  model: ClubModel;
  coach: PlayerModel;
  
  mention: MentionModel;
  replies: ReplyModel[];

  mentionId: number;

  nothingToScroll: boolean = false;
  currentPage: number = 1;

  isBusy: boolean = false;
  isScrollBusy: boolean = false;

  constructor(private client: Client,
    private route: ActivatedRoute,
    private titleService: Title) {
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.isBusy = true;

      this.client.apiMention(params.urlName, params.mentionId).subscribe(mention => {
        this.mention = mention;

        if (params.page) {
          this.currentPage = parseInt(params.page);
        }

        this.setTitle(mention.title, this.currentPage);
      });

      this.client.apiClubCoach(params.urlName).subscribe(data =>{
        if(data.id){
          this.coach = data;
        }        
      });

      this.client.apiClub(params.urlName).subscribe((data: ClubModel) => {
        this.model = data;
      });

      var query = new GetReplyListQuery();

      this.mentionId = parseInt(params.mentionId, 10)

      query.mentionId = this.mentionId;
      query.page = this.currentPage;

      this.client.apiReply(JSON.stringify(query)).subscribe(replies => {
        if (replies.length < 30)
          this.nothingToScroll = true;

        this.replies = replies;

        this.isBusy = false;
      });
    })
  }

  private setTitle(title: string, page: number) {
    if (page > 1) {
      this.titleService.setTitle(title + " - страница " + page + " | GEEN");
    }
    else {
      this.titleService.setTitle(title + " | GEEN");
    }
  }

  onScrollDown() {
    if (this.nothingToScroll || this.isScrollBusy || this.isBusy)
      return;

    this.isScrollBusy = true;

    var query = new GetReplyListQuery();

    query.mentionId = this.mentionId;
    query.page = ++this.currentPage;

    this.client.apiReply(JSON.stringify(query)).subscribe(replies => {
      if (replies.length < 30)
        this.nothingToScroll = true;

      this.replies.push(...replies);

      this.isScrollBusy = false;

    });
  }
}