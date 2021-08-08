import { Component, OnInit } from '@angular/core';
import { PlayerModel, GetMentionListQuery, MentionModel, PlayerClient, MentionClient } from '../../../../client/apiClient';
import * as _ from 'underscore'
import { ActivatedRoute } from '@angular/router';
import { ClubPlayerService } from '../../clubs/view/players/services/club.player.service';
import { Title } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'player-view',
  templateUrl: './player.view.component.html',
  styleUrls: ['./player.view.component.css']
})
export class PlayerViewComponent implements OnInit {
  player: PlayerModel;
  player$: BehaviorSubject<PlayerModel> = new BehaviorSubject<PlayerModel>(null);

  relatedPlayers: PlayerModel[];
  mentions: MentionModel[];

  playerId: number;

  nothingToScroll: boolean = false;
  currentPage: number = 1;

  isBusy: boolean = false;
  isScrollBusy: boolean = false;

  constructor(private client: PlayerClient,
    private mentionClient: MentionClient,
    private route: ActivatedRoute,
    private playerService: ClubPlayerService,
    private titleService: Title) {
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.isBusy = true;

      this.client.playerGet(params.urlName).subscribe((player: PlayerModel) => {
        this.player = player;
        this.player$.next(player);

        var query = new GetMentionListQuery();

        this.playerId = player.id;

        query.playerId = this.playerId;

        if (params.page) {
          this.currentPage = parseInt(params.page);
        }
        
        query.page = this.currentPage;

        this.setTitle(player, this.currentPage);

        this.mentionClient.getMentionList(query).subscribe(mentions => {
          if (mentions.length < 30)
            this.nothingToScroll = true;

          this.mentions = mentions;
          this.isBusy = false;
        });

        this.client.related(params.urlName).subscribe(related => {
          this.relatedPlayers = related;
        });
      });
    });
  }

  private setTitle(player: PlayerModel, page: number) {
    var title = player.lastName + " " + player.firstName;

    title += ", " + this.playerService.getPosition(player.position);

    if (player.club && player.club.name) {
      title += ' ФК ' + player.club.name;
    }

    if (page > 1) {
      title += ' - страница ' + page;
    }

    this.titleService.setTitle(title + ' | Футбольные комментарии');
  }

  onScrollDown() {
    if (this.nothingToScroll || this.isScrollBusy ||  this.isBusy)
      return;

    this.isScrollBusy = true;

    var query = new GetMentionListQuery();

    query.playerId = this.playerId;
    query.page = ++this.currentPage;

    this.mentionClient.getMentionList(query).subscribe(mentions => {
      if (mentions.length < 30)
        this.nothingToScroll = true;

      this.mentions.push(...mentions);
      this.isScrollBusy = false;
    });
  }

  getPositionName(position) {
    return this.playerService.getPosition(position);
  }

  getPlayerPhotoUrl(player) {
    return this.playerService.getPlayerPhotoUrl(player);
  }

  isCoach(player) {
    return player.position == 4;
  }

  getAge(player) {
    return this.playerService.getAge(player);
  }
}