import { Component, OnInit } from '@angular/core';
import { VoteFullModel } from 'src/client/apiClient';
import { ClubPlayerService } from 'src/app/pages/clubs/view/players/services/club.player.service';

@Component({
  selector: 'vote',
  templateUrl: './vote.component.html',
  styleUrls: ['./vote.component.css']
})
export class VoteComponent implements OnInit {
  model: VoteFullModel;

  constructor(private playerService: ClubPlayerService) {
  }

  ngOnInit() {
    
  }

  getPlayerPhotoUrl(player) {
    
  }

  save() {
    
  }
}
