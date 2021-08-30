import { Component, OnInit } from '@angular/core';
import { AppConfigService } from './app-config.service';
import { AppConfigResult } from './models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  isLoading: boolean;
  appConfigStatus: 'Not Loaded'|'Loading...'|'Loaded'; //string;
  appConfig: AppConfigResult;

  constructor(private appConfigService: AppConfigService) { }  

  ngOnInit(): void {
    this.appConfig = null;
    this.appConfigStatus = "Not Loaded";
  }

  onRefreshClick(): void {
    this.getAppStatus();
  }

  private getAppStatus(): void {
    this.isLoading = true;
    this.appConfigStatus = "Loading..."
    this.appConfigService.getAppConfig().subscribe(result => {
      this.appConfig = result;
      this.appConfigStatus = "Loaded"
      this.isLoading = false;
    });
  }

}
