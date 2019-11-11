import { Component, Input } from '@angular/core';
import { MentionModel, Client, LikeModel } from '../../../../client/apiClient';

@Component({
  selector: 'mention-like-item',
  templateUrl: './mention.like.component.html'
})
export class MentionLikeComponent {
  @Input() model: MentionModel;

  constructor(private client: Client) {
  }

  like(){
    this.client.apiMentionLike(this.model.id).subscribe((data: LikeModel) => {
        this.updateModel(data);
    });
  }

  dislike(){
    this.client.apiMentionDislike(this.model.id).subscribe((data: LikeModel) => {
        this.updateModel(data);
    });
  }

  updateModel(likeModel: LikeModel){
    this.model.likes = likeModel.likes;
    this.model.dislikes = likeModel.dislikes;
  }
}
