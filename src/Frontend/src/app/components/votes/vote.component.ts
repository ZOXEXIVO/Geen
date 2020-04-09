import { Component, OnInit } from '@angular/core';
import { Client, VoteFullModel } from 'src/client/apiClient';
import { ClubPlayerService } from 'src/app/pages/clubs/view/players/services/club.player.service';

@Component({
  selector: 'vote',
  templateUrl: './vote.component.html',
  styleUrls: ['./vote.component.css']
})
export class VoteComponent implements OnInit {
  model: VoteFullModel;

  constructor(private client: Client, private playerService: ClubPlayerService) {
  }

  ngOnInit() {
     this.client.apiVotes().subscribe(model => {
        this.model = model;
     });
  }

  getPlayerPhotoUrl(player) {
    return this.playerService.getPlayerPhotoUrl(player);
  }

  save() {
    
  }
}
