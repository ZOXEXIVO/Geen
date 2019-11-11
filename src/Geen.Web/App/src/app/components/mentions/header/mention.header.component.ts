import { Component, Input } from '@angular/core';
import { MentionModel } from '../../../../client/apiClient';

@Component({
  selector: 'mention-header',
  templateUrl: './mention.header.component.html',
  styleUrls: ['./mention.header.component.css']
})
export class MentionHeaderComponent {
  @Input() model: MentionModel;
  @Input() isSubjectVisible: boolean;

  getUserImageUrl() {
    if (!this.model.user.profileImage) {
      return '/assets/images/users/anonymous.png';
    }

    return this.model.user.profileImage;
  }

  getUserName() {
    if (!this.model.user.name) {
      return 'Анонимно';
    }

    return this.model.user.name;
  }

  getClubName() {
    if (!this.model.club)
      return '';

    if (this.model.club.isNational) {
      return 'по футболу';
    }

    return 'Футбольный клуб';
  }

  getPlayerPhotoUrl() {
    return "https://storage.geen.one/geen/" + this.model.player.id + ".jpg";
  }
}
