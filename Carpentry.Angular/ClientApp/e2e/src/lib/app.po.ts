// import { browser, by, element } from 'protractor';

import { browser, WebElement, $, $$, by } from "protractor";

export class AppPage {

    toolbar = $('mat-toolbar');

    async get(): Promise<any> {
        return await browser.get(browser.baseUrl);
    }


    async clickNavLink(navLinkText: string) {
        //const link = this.
        const link = this.toolbar.element(by.cssContainingText('a', navLinkText));
        await link.click();
    }

    async clickHomeButton() {
        
    }

    // async getNavLinks(): Promise<WebElement[]> {
    //     const locator = $$('mat-toolbar > a.navlink');
    //     //const elements = await browser.findElements(locator);
    // }
}

// export class AppPage {
//   navigateTo() {
//     return browser.get('/');
//   }

//   getMainHeading() {
//     return element(by.css('app-root h1')).getText();
//   }
// }
