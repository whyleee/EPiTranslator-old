'use strict';

/* http://docs.angularjs.org/guide/dev_guide.e2e-testing */

describe('Translator', function () {

  var updateTranslation = function(inputIndex, value, skipSave) {
    element('#translations .category:first .translation:nth(' + inputIndex + ') .edit input').click();
    input('translation.Value', inputIndex).enter(value);
    
    if (!skipSave) {
      element('#translations .category:first .translation:nth(' + inputIndex + ') .save').click();
    }
  };

  beforeEach(function () {
    $.cookie('e2e', 'true', { expires: 7, path: '/' });
    browser().navigateTo('/app/index.html');
  });


  it('should automatically redirect to /translations when location hash/fragment is empty', function() {
    expect(browser().location().url()).toBe("/translations/");
  });


  describe('Translations table', function () {
    
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

    it('should show only not translated entries when in "Not translated" view', function() {
      browser().navigateTo('/app/index.html#/translations/not-translated');

      expect(repeater('#translations .keyword').count()).toBe(1);
      expect(element('#translations .category:first .translation:nth(1) .edit input').val()).toBe('');
    });

    it('should show [Missing] text for not translated words/phrases', function() {
      expect(element('#translations .category:first .translation:nth(3)').text()).toMatch('[Missing]');
    });

    it('should colorize fallback translations', function() {
      expect(element('#translations .category:first .translation:nth(2) .view span').css('color')).not().toBe('rgb(51, 51, 51)');
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


  describe('Category selector', function () {
    
    it('should show all categories with additional "All" and "Not translated" entries', function() {
      expect(repeater('#categories li').count()).toBe(4);
      expect(element('#categories li:nth(0)').text()).toMatch('All');
      expect(element('#categories li:nth(1)').text()).toMatch('Not translated');
      expect(element('#categories li:nth(2)').text()).toMatch('Dictionary');
      expect(element('#categories li:nth(3)').text()).toMatch('Header');
    });

    it('should go to default view when "All" item was clicked', function() {
      element('#categories li:nth(0) a').click();

      expect(browser().location().url()).toBe("/translations/");
    });
    
    it('should go to "not-translated" view when "Not translated" item was clicked', function () {
      element('#categories li:nth(1) a').click();

      expect(browser().location().url()).toBe("/translations/not-translated");
    });

    it('should go to category view when clicked', function() {
      element('#categories li:nth(2) a').click();
      
      expect(browser().location().url()).toBe("/translations/Dictionary");
    });
  });
  

  describe('Search', function () {

    it('should filter categories while typing part of the category name into the search box', function () {
      expect(repeater('#translations .category').count()).toBe(2);

      input('query').enter('dict');
      expect(repeater('#translations .category').count()).toBe(1);
      expect(element('#translations .category:first .category-header').text()).toMatch('Dictionary');
    });

    it('should filter entries while typing part of the keyword into the search box', function () {
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

    it('should filter categories while typing part of the keyword into the search box', function () {
      expect(repeater('#translations .category').count()).toBe(2);

      input('query').enter('hello');
      expect(repeater('#translations .category').count()).toBe(1);
      expect(element('#translations .category:first .category-header').text()).toMatch('Header');
    });
  });


  describe('Translation update', function() {

    describe('When translation was updated with another value', function () {

      beforeEach(function () {
        updateTranslation(1, 'new value');
      });

      it('should update translation view with new value', function () {
        expect(element('#translations .category:first .translation:nth(1) .view').text()).toMatch('new value');
      });

      it('should show notification that translation was updated', function () {
        sleep(0.3);
        expect(element('#jGrowl .jGrowl-notification .jGrowl-message').text()).toMatch("Danish: 'Name' to 'new value'");
      });
    });

    describe('When translation was updated with the same value', function () {

      beforeEach(function () {
        updateTranslation(1, 'Navn');
      });

      it('should be the same value in view', function () {
        expect(element('#translations .category:first .translation:nth(1) .view').text()).toMatch('Navn');
      });

      it('should not show any notifications', function () {
        sleep(0.3);
        expect(element('#jGrowl .jGrowl-notification').count()).toBe(0);
      });
    });

    describe('When translation was updated with empty value', function () {

      beforeEach(function () {
        updateTranslation(1, '');
      });

      it('should show validation tooltip', function () {
        expect(element('.tooltip:visible').text()).toMatch('Translation value is required');
      });
    });

    describe('When translation was updated with another value but cancelled', function () {

      beforeEach(function () {
        updateTranslation(1, 'new value', /* skip save */ true);
        element('#translations .category:first .translation:nth(1) .undo').click();
      });

      it('should return to previous value in translation view', function () {
        expect(element('#translations .category:first .translation:nth(1) .view').text()).toMatch('Navn');
      });

      it('should not show any notifications', function () {
        sleep(0.3);
        expect(element('#jGrowl .jGrowl-notification').count()).toBe(0);
      });
    });

    describe('When new translation was set to not translated entry in "Not translated" view', function () {

      beforeEach(function () {
        browser().navigateTo('/app/index.html#/translations/not-translated');
        updateTranslation(1, 'new value');
      });

      it('should remain visible with new value', function () {
        expect(element('#translations .category:first .translation:nth(1) .view:visible').text()).toMatch('new value');
      });
    });
  });
});
