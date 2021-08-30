import { ElementFinder, ExpectedConditions } from 'protractor';
import { AppPage } from './app.po';

describe('App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should display welcome message', async () => {
    page.navigateTo();
    //expect(await page.mainHedding.getText()).toEqual('Hello, world!');
    await expectText(page.mainHedding, 'Hello, world!')
  });

  it('should load config info', async () => {
    //expect initial config
    //expect(await page.configStatus.getText()).toEqual('Not Loaded');
    //expect config val === 'Not Loaded'
    await expectText(page.configStatus, 'Not Loaded');
    
    //expect no ConfigVal
    //expect no LastUpdated
    //Expect refresh button
    //expect(page.refreshButton); //.isPresent()
    
    //click refresh
    await page.refreshButton.click();

    
    //expect config val === 'Loading...'
    await expectText(page.configStatus, 'Loading...');

    //wait for busy indicator to disapear
    await page.waitForBusy();

    //expect new config val
    await expectText(page.configStatus, 'Loaded');
    //expect val
    await expectText(page.configValue, 'Example Value');

    //expect datetime
    //expect(await page.progressBar.getText()).tobenear
    // ExpectedConditions.
    //(optional) do again

    //
  });

  async function expectText(element: ElementFinder, expectedValue: string) {
    expect(await element.getText()).toEqual(expectedValue);
  }

  // function expectFieldPresent(element: ElementFinder) {
  //   //expect(page.configStatus).
  // }
});
