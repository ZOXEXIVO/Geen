import { Component, OnInit } from '@angular/core';
import { MentionModel, GetMentionTitleListQuery, BodyText, AdminMentionClient } from '../../../../client/apiClient';

@Component({
  templateUrl: './admin.titles.component.html'
})
export class AdminTitlesComponent implements OnInit {
  mentions: MentionModel[];
  query: GetMentionTitleListQuery = new GetMentionTitleListQuery();

  constructor(private client: AdminMentionClient) {
  }

  ngOnInit() {
    this.client.titles(this.query).subscribe(mentions => {
      this.mentions = mentions;
    })
  }

  changeText(mention: MentionModel){
    var text = new BodyText();

    text.text = mention.text;

    this.client.text(mention.id, text).subscribe(_ => {

    });
  }

  changeTitle(mention: MentionModel){
    this.client.title(mention.id, mention.title).subscribe(_ => {
      
    });;
  }

  onFilterChange(){
    this.ngOnInit();
  }
}
