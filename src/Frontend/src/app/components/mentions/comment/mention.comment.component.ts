import { Component, Input } from '@angular/core';
import { MentionModel } from '../../../../client/apiClient';

@Component({
  selector: 'mention-comments',
  templateUrl: './mention.comment.component.html',
  styleUrls: ['./mention.comment.component.css']
})
export class MentionCommentComponent {
  @Input() model: MentionModel;
}
