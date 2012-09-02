'use strict';

/* http://docs.angularjs.org/guide/dev_guide.e2e-testing */

describe('Translator', function() {

  beforeEach(function() {
    browser().navigateTo('/app/index.html');
  });


  it('should automatically redirect to /translations when location hash/fragment is empty', function() {
    expect(browser().location().url()).toBe("/translations");
  });


  describe('Translations table', function() {
    it('should have column for each received language', function() {
      expect(repeater('#translations .language-col').count()).toBe(2);
    });

    it('language columns should show language display names', function () {
      expect(element('#translations .language-col:eq(0)').text()).toMatch('English');
      expect(element('#translations .language-col:eq(1)').text()).toMatch('Danish');
    });

    it('should have been grouped by categories', function() {
      expect(element('#translations .category').count()).toBe(2);
      expect(element('#translations .category:first .category-header').text()).toMatch('Dictionary');
      expect(element('#translations .category:last .category-header').text()).toMatch('Header');
    });

    it('categories should list keyword and translation for each language', function () {
      expect(element('#translations .category:first .keyword:nth(0)').text()).toMatch('Name');
      expect(element('#translations .category:first .translation:nth(0)').text()).toMatch('Name');
      expect(element('#translations .category:first .translation:nth(1)').text()).toMatch('Navn');
    });
  });


  describe('Language selector', function () {

    it('should show checkboxes for all languages', function() {
      expect(repeater('.language-selector .checkbox').count()).toBe(2);
      expect(element('.language-selector .checkbox:nth(0)').text()).toMatch('English');
      expect(element('.language-selector .checkbox:nth(1)').text()).toMatch('Danish');
    });

    it('should check only checkboxes for selected languages', function () {
      expect(repeater('.language-selector .checkbox input:checked').count()).toBe(2);
    });

    it('should hide language column in translation table after unselecting a language', function() {
      input('lang.selected').check();
      
      expect(repeater('#translations .language-col').count()).toBe(0);
    });
  });
});
