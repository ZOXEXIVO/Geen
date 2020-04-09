import { Component, Input } from '@angular/core';
import { ReplyModel } from '../../../../client/apiClient';

@Component({
  selector: 'reply-item',
  templateUrl: './reply.item.component.html'
})
export class ReplyItemComponent {
  @Input() model: ReplyModel;

  constructor() {

  }
}
