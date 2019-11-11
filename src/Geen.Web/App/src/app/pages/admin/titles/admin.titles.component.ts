import { Component, OnInit } from '@angular/core';
import { Client, MentionModel, GetMentionTitleListQuery, BodyText } from '../../../../client/apiClient';

@Component({
  templateUrl: './admin.titles.component.html'
})
export class AdminTitlesComponent implements OnInit {
  mentions: MentionModel[];
  query: GetMentionTitleListQuery = new GetMentionTitleListQuery();

  constructor(private client: Client) {
  }

  ngOnInit() {
    this.client.apiAdminMentionTitleslist(JSON.stringify(this.query)).subscribe(mentions => {
      this.mentions = mentions;
    })
  }

  changeText(mention: MentionModel){
    var text = new BodyText();

    text.text = mention.text;

    this.client.apiAdminMentionChangetext(mention.id, text).subscribe(_ => {

    });
  }

  changeTitle(mention: MentionModel){
    this.client.apiAdminMentionChangetitle(mention.id, mention.title).subscribe(_ => {
      
    });;
  }

  onFilterChange(){
    this.ngOnInit();
  }
}
