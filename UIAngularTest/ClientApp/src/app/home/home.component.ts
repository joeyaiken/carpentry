import { Component, OnInit } from '@angular/core';
import { convertToObject } from 'typescript';
import { AppConfigService } from '../app-config.service';
import { AppConfigResult } from '../models';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less'],
})
export class HomeComponent implements OnInit {
  isLoading: boolean;
  appConfigStatus: 'Not Loaded'|'Loading...'|'Loaded'; //string;

  appConfig: AppConfigResult;

  constructor(
    private appStatusService: AppConfigService,
  ) { }

  ngOnInit(): void {
    this.appConfig = null;
    this.appConfigStatus = "Not Loaded"
  }

  onRefreshClick(): void {
    this.getAppStatus();
  }

  private getAppStatus(): void {
    this.isLoading = true;
    this.appConfigStatus = "Loading..."
    this.appStatusService.getAppConfig().subscribe(result => {
      this.appConfig = result;
      this.appConfigStatus = "Loaded"
      this.isLoading = false;
    });
  }
}
