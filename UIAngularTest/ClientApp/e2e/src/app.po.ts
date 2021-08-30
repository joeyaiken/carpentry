import { browser, by, element, ElementFinder, $ } from 'protractor';

export class AppPage {

  public mainHedding: ElementFinder;

  public configStatus: ElementFinder;
  public configValue: ElementFinder;
  public configLastUpdated: ElementFinder;
  public refreshButton: ElementFinder;

  public constructor() {
    this.mainHedding = element(by.css('app-root h1'));
    this.configStatus = $('#app-config-status')

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


}
