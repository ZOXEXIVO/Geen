import { Injectable } from "@angular/core";


@Injectable()
export class ClubPlayerService {
   
    getPositionName(position: number) {
        switch (position) {
            case 0:
                return 'Вратари';
            case 1:
                return 'Защитники';
            case 2:
                return 'Полузащитники';
            case 3:
                return 'Нападающие';
            case 4:
                return 'Главный тренер';
        }
    }

    getPosition(position: number) {
        switch (position) {
            case 0:
                return 'вратарь';
            case 1:
                return 'защитник';
            case 2:
                return 'полузащитник';
            case 3:
                return 'нападающий';
            case 4:
                return 'главный тренер';
        }
    }

    getPlayerPhotoUrl(player) {
        return 'https://storage.geen.one/geen/' + player.id + '.jpg';
    }

    getAge(player){
        try{
            var today = new Date();
            var birthDate = new Date(player.birthDate);
            var age = today.getFullYear() - birthDate.getFullYear();
            var m = today.getMonth() - birthDate.getMonth();
            if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
                age--;
            }

            return age.toString() + ' ' + this.pluralizeAge(age);   
        }
        catch(e){
            return null;
        }                                
    }

    pluralizeAge(age){
        var txt;
        var count = age % 100;
        if (count >= 5 && count <= 20) {
            txt = 'лет';
        } else {
            count = count % 10;
            if (count == 1) {
                txt = 'год';
            } else if (count >= 2 && count <= 4) {
                txt = 'года';
            } else {
                txt = 'лет';
            }
        }
        return txt;
    }
}
