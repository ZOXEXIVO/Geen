import { Component, Input } from '@angular/core';
import { MentionModel, MentionClient, LikeModel } from '../../../../client/apiClient';

@Component({
  selector: 'mention-like-item',
  templateUrl: './mention.like.component.html'
})
export class MentionLikeComponent {
  @Input() model: MentionModel;

  constructor(private client: MentionClient) {
  }

  like(){
    this.client.like(this.model.id).subscribe((data: LikeModel) => {
        this.updateModel(data);
    });
  }

  dislike(){
    this.client.dislike(this.model.id).subscribe((data: LikeModel) => {
        this.updateModel(data);
    });
  }

  updateModel(likeModel: LikeModel){
    this.model.likes = likeModel.likes;
    this.model.dislikes = likeModel.dislikes;
  }
}
