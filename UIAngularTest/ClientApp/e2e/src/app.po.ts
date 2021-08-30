import { browser, by, element, ElementFinder, $, ExpectedConditions } from 'protractor';

export class AppPage {
  public progressBar: ElementFinder;
  public mainHedding: ElementFinder;
  public configStatus: ElementFinder;
  public configValue: ElementFinder;
  public configLastUpdated: ElementFinder;
  public refreshButton: ElementFinder;

  public constructor() {
    this.progressBar = $('mat-progress-bar')
    this.mainHedding = element(by.css('app-root h1'));
    this.configStatus = $('#app-config-status');
    this.configValue = $('#config-string');
    this.configLastUpdated = $("#config-last-updated");
    this.refreshButton = $("#refresh-button");
  }

  navigateTo() {
    return browser.get('/');
  }

  // getMainHeadingText() {
  //   return this.mainHedding.getText();
  // }
  
  getMainHeading() {
    return element(by.css('app-root h1')).getText();
  }

  async waitForBusy(timeout = 5000) {
    //await browser.wait(ExpectedConditions.not(ExpectedConditions.presenceOf(this.progressBar)), timeout);
    await browser.wait(ExpectedConditions.not(ExpectedConditions.presenceOf(this.progressBar)));
  }

}
