import { Component, Input } from '@angular/core';
import { MentionModel } from '../../../../client/apiClient';

@Component({
  selector: 'mention-list',
  templateUrl: './mention.list.component.html',
  styleUrls: ['./mention.list.component.css']
})
export class MentionListComponent {
  @Input() model: MentionModel[];  
  @Input() isSubjectVisible: boolean = true;
}
