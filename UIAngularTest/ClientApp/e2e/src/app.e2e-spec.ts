import { AppPage } from './app.po';

describe('App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should display welcome message', () => {
    // page.navigateTo();
    expect(page.mainHedding.getText()).toEqual('Hello, world!');




    //wait for the controller to load

    //expect status results

    expect(1).toBe(0);

  });
});
