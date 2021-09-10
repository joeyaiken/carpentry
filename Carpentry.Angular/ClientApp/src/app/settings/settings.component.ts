import { Component, OnInit } from "@angular/core";

//TODO - Should this be loading collection totals / tracked sets?

@Component({
  selector: 'app-settings',
  templateUrl: 'settings.component.html',
  styleUrls: ['settings.component.less'],
})
export class SettingsComponent implements OnInit {
  isBusy: boolean;
  constructor() {
    this.isBusy = false;
  }
  ngOnInit(): void { }

  //TODO - This component should be handling the actual data calls
  //   The Collection Totals and trackedSets UI components should be purly functional
}