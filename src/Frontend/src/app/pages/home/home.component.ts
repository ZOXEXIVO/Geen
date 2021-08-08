import { Component, OnInit } from '@angular/core';
import { MentionModel, GetMentionListQuery, MentionClient } from '../../../client/apiClient';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'home-component',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
  mentions: MentionModel[];
  currentPage: number = 1;

  isBusy: boolean = false;
  isScrollBusy: boolean = false;

  constructor(private client: MentionClient,
    private route: ActivatedRoute,
    private titleService: Title) {
  }

  ngOnInit() {
    this.route.params.subscribe((params: Params) => {
      this.isBusy = true;

      if (params.page) {
        this.currentPage = parseInt(params.page, 10);
      }

      this.setTitle(this.currentPage);

      const query = new GetMentionListQuery();

      query.page = this.currentPage;

      this.client.getMentionList(query).subscribe((mentions: MentionModel[]) => {
        this.mentions = mentions;
        this.isBusy = false;
      });
    });
  }

  onScrollDown() {
    this.isScrollBusy = true;

    const query = new GetMentionListQuery();

    this.currentPage += 1;

    query.page = this.currentPage;

    this.client.getMentionList(query).subscribe((mentions: MentionModel[]) => {
      this.mentions.push(...mentions);
      this.isScrollBusy = false;
    });
  }

  onScrollUp(){
    console.log('up');
  }

  private setTitle(page: number) {
    var title = 'Анонимная Футбольная гостевая книга';

    if (page > 1) {
      title += ' - страница ' + page;
    }

    this.titleService.setTitle(title + ' | GEEN');
  }
}
