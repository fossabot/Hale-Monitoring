import { HaleFrontendPage } from './app.po';
import { browser, protractor } from 'protractor';

describe('the login page', function() {
  let page: HaleFrontendPage;

  beforeEach(() => {
    page = new HaleFrontendPage();
    page.navigateTo();

  });

  it('should contain a username control', () => {
    expect(page.getUsernameControl().isPresent()).toBeTruthy();
  });
  it('should contain a password control', () => {
    expect(page.getPasswordControl().isPresent()).toBeTruthy();
  });
  it('should contain a submit button', () => {
    expect(page.getSubmitButton().isPresent()).toBeTruthy();
  });
  it('should redirect on submit', () => {
    page.fillInForm();
    browser.sleep(500);
    expect(page.getNavbar().isPresent()).toBeTruthy();
  });

});
