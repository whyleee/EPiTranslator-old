'use strict';

/* http://docs.angularjs.org/guide/dev_guide.e2e-testing */

describe('Translator', function() {

  beforeEach(function() {
    browser().navigateTo('/app/index.html');
  });


  it('should automatically redirect to /translations when location hash/fragment is empty', function() {
    expect(browser().location().url()).toBe("/translations/");
  });


  describe('Translations table', function() {
    it('should have column for each received language', function () {
      expect(repeater('#translations .language-col').count()).toBe(2);
    });

    it('language columns should show language display names', function () {
      expect(element('#translations .language-col:eq(0)').text()).toMatch('English');
      expect(element('#translations .language-col:eq(1)').text()).toMatch('Danish');
    });

    it('should have been grouped by categories', function() {
      expect(repeater('#translations .category').count()).toBe(2);
      expect(element('#translations .category:first .category-header').text()).toMatch('Dictionary');
      expect(element('#translations .category:last .category-header').text()).toMatch('Header');
    });

    it('categories should list keyword and translation for each language', function () {
      expect(element('#translations .category:first .keyword:nth(0)').text()).toMatch('Name');
      expect(element('#translations .category:first .translation:nth(0)').text()).toMatch('Name');
      expect(element('#translations .category:first .translation:nth(1)').text()).toMatch('Navn');
    });

    it('should filter categories by selected', function() {
      browser().navigateTo('/app/index.html#/translations/Dictionary');
      
      expect(repeater('#translations .category').count()).toBe(1);
      expect(element('#translations .category:first .category-header').text()).toMatch('Dictionary');
    });

    it('should filter categories while typing part of the category name into the search box', function() {
      expect(repeater('#translations .category').count()).toBe(2);

      input('query').enter('dict');
      expect(repeater('#translations .category').count()).toBe(1);
      expect(element('#translations .category:first .category-header').text()).toMatch('Dictionary');
    });

    it('should filter entries while typing part of the keyword into the search box', function() {
      expect(repeater('#translations .keyword').count()).toBe(3);

      input('query').enter('em');
      expect(repeater('#translations .keyword').count()).toBe(1);
      expect(element('#translations .keyword:first').text()).toMatch('Email');
    });
    
    it('should filter entries while typing part of the translation into the search box', function () {
      expect(repeater('#translations .keyword').count()).toBe(3);

      input('query').enter('avn');
      expect(repeater('#translations .keyword').count()).toBe(1);
      expect(element('#translations .keyword:first').text()).toMatch('Name');
    });

    it('should filter categories while typing part of the keyword into the search box', function() {
      expect(repeater('#translations .category').count()).toBe(2);

      input('query').enter('hello');
      expect(repeater('#translations .category').count()).toBe(1);
      expect(element('#translations .category:first .category-header').text()).toMatch('Header');
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


  describe('Category selector', function() {
    it('should show all categories with additional "All" entry', function() {
      expect(repeater('#categories li').count()).toBe(3);
      expect(element('#categories li:nth(0)').text()).toMatch('All');
      expect(element('#categories li:nth(1)').text()).toMatch('Dictionary');
      expect(element('#categories li:nth(2)').text()).toMatch('Header');
    });

    it('should go to category view when clicked', function() {
      element('#categories li:nth(1) a').click();
      
      expect(browser().location().url()).toBe("/translations/Dictionary");
    });
  });
});
