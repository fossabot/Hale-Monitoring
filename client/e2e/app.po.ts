import { browser, element, by } from 'protractor';

export class HaleFrontendPage {
  navigateTo() {
    return browser.get('/');
  }

  getUsernameControl() {
    return element(by.name('username'));
  }

  getPasswordControl() {
    return element(by.name('password'));
  }

  getSubmitButton() {
    return element(by.css('button'));
  }

  fillInForm() {
    const eUsername = this.getUsernameControl();
    const ePassword = this.getPasswordControl();
    const eBtn = this.getSubmitButton();
    eUsername.clear().then(() => eUsername.sendKeys('test01'));
    ePassword.clear().then(() => ePassword.sendKeys('test01'));
    eBtn.click();
  }

  getNavbar() {
    return element(by.css('.nav'));
  }

}
