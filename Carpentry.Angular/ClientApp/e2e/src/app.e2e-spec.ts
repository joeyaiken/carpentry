import { browser } from 'protractor';
import { AppPage } from './app.po';

describe('App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();

    browser.sleep(5000);

    expect(page.getMainHeading()).toEqual('Hello, world!');
  });
});
