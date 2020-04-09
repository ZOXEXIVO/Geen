import { NgModule } from '@angular/core';
import { GlobalSpinnerComponent } from './spinners/spinner.global.component';
import { SpinnerComponent } from './spinners/spinner.component';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    SpinnerComponent,
    GlobalSpinnerComponent
  ],
  imports: [
    RouterModule
  ],
  exports:[
    SpinnerComponent,
    GlobalSpinnerComponent
  ]
})
export class SharedModule { }
