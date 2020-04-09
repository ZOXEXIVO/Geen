import { Component, Input } from '@angular/core';
import { MentionModel } from '../../../../client/apiClient';

@Component({
  selector: 'mention-item',
  templateUrl: './mention.item.component.html',
  styleUrls: ['./mention.item.component.css']
})
export class MentionItemComponent {
  @Input() model: MentionModel;
  @Input() showTitle: true;
}
