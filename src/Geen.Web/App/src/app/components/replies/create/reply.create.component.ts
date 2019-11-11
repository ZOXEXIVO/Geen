import { Component, Input, OnInit } from '@angular/core';
import { MentionModel, Client, ReplyModel, ReplyCreateDto } from '../../../../client/apiClient';
import { AuthService } from 'src/app/shared/auth/auth.service';

@Component({
  selector: 'reply-create',
  templateUrl: './reply.create.component.html',
  styleUrls: ['./reply.create.component.css']
})
export class ReplyCreateComponent  implements OnInit {
  model: ReplyModel = new ReplyModel();

  @Input() container: ReplyModel[];
  @Input() mention: MentionModel;

  isBusy: boolean = false;
  isUnapproved: boolean = false;
  userName: string = null;
  
  constructor(private client: Client, public authClient: AuthService) {
  }

  ngOnInit() {    
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

    var replyModel = new ReplyCreateDto();

    replyModel.text = this.model.text;
    replyModel.mentionId = this.mention.id;

    this.model.text = '';

    this.client.apiReplyCreate(replyModel).subscribe(item => {
      this.isBusy = false;
      
      this.isUnapproved = !item.isApproved;

      if(item.isApproved){
        this.container.push(item);
      }      
    }, error => {
      this.isBusy = false;
    });
  }
}
