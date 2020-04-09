import { Component, Input, OnInit } from '@angular/core';
import { MentionModel, Client, MentionCreateDto, PlayerModel, ClubModel } from '../../../../client/apiClient';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/shared/auth/auth.service';

@Component({
  selector: 'mention-create',
  templateUrl: './mention.create.component.html',
  styleUrls: ['./mention.create.component.css']
})
export class MentionCreateComponent implements OnInit {
  model: MentionModel = new MentionModel();

  @Input() container: MentionModel[];

  @Input() player$: Observable<PlayerModel>;
  player: PlayerModel;

  @Input() club$: Observable<ClubModel>;
  club: ClubModel;

  isBusy: boolean = true;
  isUnapproved: boolean = false;
  userName: string = null;

  constructor(private client: Client, public authClient: AuthService) {
  }

  ngOnInit() {
    if (this.club$) {
      this.club$.subscribe(club => {
        this.club = club;
        this.isBusy = false;
      });
    }

    if (this.player$) {
      this.player$.subscribe(player => {
        this.player = player;
        this.isBusy = false;
      });
    }

    this.authClient.getUser().subscribe(user => {
      this.userName = user;
    });
  }

  userChanged(){
    this.authClient.setUser(this.userName);
  }

  save() {
    if(this.isBusy)
    return;

    this.isBusy = true;
    this.isUnapproved = false;

    var mentionModel = new MentionCreateDto();

    mentionModel.text = this.model.text;

    if (!this.player && !this.club)
      return;

    if (this.player) {
      mentionModel.playerId = this.player.id;
    }
    if (this.club) {
      mentionModel.clubId = this.club.id;
    }

    this.model.text = '';

    this.client.apiMentionCreate(mentionModel).subscribe(item => {
      this.isBusy = false;

      var mentionResult = item;

      this.isUnapproved = !mentionResult.isApproved;

      if (mentionResult.isApproved) {
        this.container.unshift(mentionResult);
      }
    }, error => {
      this.isBusy = false;
    });
  }
}
