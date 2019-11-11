import { Component, Input } from '@angular/core';
import { MentionModel } from '../../../../client/apiClient';

@Component({
  selector: 'mention-comments',
  templateUrl: './mention.comment.component.html'
})
export class MentionCommentComponent {
  @Input() model: MentionModel;
}
