import { Component, OnInit } from '@angular/core';
import { ClubModel, AdminPlayerClient, PlayerModel, AdminClubClient } from '../../../../client/apiClient';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  templateUrl: './admin.player.edit.component.html'
})
export class AdminPlayerEditComponent implements OnInit {
  model: PlayerModel;
  birthDate: string;

  clubs: ClubModel[];

  constructor(private client: AdminPlayerClient,
    private clubClient: AdminClubClient,
    private route: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params.id) {
        this.client.getPlayer(params.id).subscribe(player => {
          this.model = player;

          if (player.birthDate) {
            var day = player.birthDate.getDate();
            var month = (player.birthDate.getMonth() + 1);
            var year =  player.birthDate.getFullYear();

            var playerBirthday = (day < 10 ? '0' : '') + day + '.' +  (month < 10 ? '0' : '') + month + '.' +year;
            this.birthDate = playerBirthday;
          }
        });
      } else {
        this.model = new PlayerModel();

        this.client.nextId().subscribe(nextId => {
          this.model.id = nextId * 1;
        });
      }
    });

    this.clubClient.getAdminClubList().subscribe(clubs => {      
      this.clubs = clubs;
      this.clubs.unshift(null);
    });
  }

  clubsCompareFunc(clubA, clubB) {
    if (!clubA || !clubB)
      return false;

    return clubA.id == clubB.id;
  }

  save() {
    var parts = this.birthDate.split('.');
    this.model.birthDate = new Date(parseInt(parts[2], 10), parseInt(parts[1], 10) - 1, parseInt(parts[0], 10));
    this.model.position = this.model.position * 1;
    
    this.client.savePlayer(this.model).subscribe(x => {
      this.router.navigateByUrl('/admin/player/list');
    });
  }

  cancel() {
    this.router.navigateByUrl('/admin/player/list');
  }
}
