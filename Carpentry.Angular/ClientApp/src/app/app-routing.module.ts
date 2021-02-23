import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { HomeComponent } from './home/home.component';
import { LandingComponent } from './landing/landing.component';
//imports


const routes: Routes = [
  { 
    path: '', 
    component: HomeComponent, 
    children: [
      { path: '', component: LandingComponent },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
    ]
  },
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
