import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlayerModel, MentionModel, ReplyModel, GetReplyListQuery, MentionClient, PlayerClient, ReplyClient } from '../../../../../client/apiClient';
import { ClubPlayerService } from '../../../clubs/view/players/services/club.player.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'mention-player-view',
  templateUrl: './mention.player.view.html',
  styleUrls: ['./mention.player.view.css']
})
export class MentionPlayerViewComponent implements OnInit {
  model: PlayerModel;
  mention: MentionModel;
  relatedPlayers: PlayerModel[];
  replies: ReplyModel[];

  mentionId: number;

  nothingToScroll: boolean = false;
  currentPage: number = 1;

  isBusy: boolean = false;
  isScrollBusy: boolean = false;

  constructor(private client: MentionClient,
    private replyClient: ReplyClient,
    private playerClient: PlayerClient,
    private route: ActivatedRoute,
    private playerService: ClubPlayerService,
    private titleService: Title) {
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.isBusy = true;

      this.client.getMention(params.urlName, params.mentionId).subscribe(mention => {
        this.mention = mention;

        if (params.page) {
          this.currentPage = parseInt(params.page);
        }

        this.setTitle(mention.title, this.currentPage);
      });

      this.playerClient.playerGet(params.urlName).subscribe(player => {
        this.model = player;
      });

      this.playerClient.related(params.urlName).subscribe((data: PlayerModel[]) => {
        this.relatedPlayers = data;
      });

      var query = new GetReplyListQuery();

      this.mentionId = parseInt(params.mentionId, 10)

      query.mentionId = this.mentionId;
      query.page = this.currentPage;

      this.replyClient.reply(query).subscribe(replies => {
        if (replies.length < 30)
          this.nothingToScroll = true;

        this.replies = replies;

        this.isBusy = false;
      });
    })
  }

  private setTitle(title: string, page: number) {
    if (page > 1) {
      this.titleService.setTitle(title + " - страница " + page + " | Футбольные комментарии");
    }
    else {
      this.titleService.setTitle(title + " | Футбольные комментарии");
    }
  }

  onScrollDown() {
    if (this.nothingToScroll || this.isScrollBusy || this.isBusy)
      return;

    this.isScrollBusy = true;

    var query = new GetReplyListQuery();

    query.mentionId = this.mentionId;
    query.page = ++this.currentPage;

    this.replyClient.reply(query).subscribe(replies => {
      if (replies.length < 30)
        this.nothingToScroll = true;

      this.replies.push(...replies);
      this.isScrollBusy = false;
    });
  }

  getPlayerPhotoUrl(player) {
    return this.playerService.getPlayerPhotoUrl(player);
  }
}