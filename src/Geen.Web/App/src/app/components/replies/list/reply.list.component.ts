import { Component, Input } from '@angular/core';
import { ReplyModel } from '../../../../client/apiClient';

@Component({
  selector: 'reply-list',
  templateUrl: './reply.list.component.html',
  styleUrls: ['./reply.list.component.css']
})
export class ReplyListComponent {
  @Input() model: ReplyModel[];  
}
