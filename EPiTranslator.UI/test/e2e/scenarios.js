'use strict';

/* http://docs.angularjs.org/guide/dev_guide.e2e-testing */

describe('Translator', function() {

  beforeEach(function() {
    browser().navigateTo('/app/index.html');
  });


  it('should automatically redirect to /view1 when location hash/fragment is empty', function() {
    expect(browser().location().url()).toBe("/view1");
  });


  describe('Translations table', function() {
    it('should have column for each received language', function() {
      expect(repeater('.translations .language-col').count()).toBe(2);
    });

    it('language columns should show language display names', function () {
      expect(element('.translations .language-col:eq(0)').text()).toBe('English');
      expect(element('.translations .language-col:eq(1)').text()).toBe('Danish');
    });

    it('should have been grouped by categories', function() {
      expect(element('.translations .category').count()).toBe(2);
      expect(element('.translations .category:first .category-header').text()).toBe('Dictionary');
      expect(element('.translations .category:last .category-header').text()).toBe('Header');
    });

    it('categories should list keyword and translation for each language', function () {
      expect(element('.translations .category:first .keyword:nth(0)').text()).toBe('Name');
      expect(element('.translations .category:first .translation:nth(0)').text()).toBe('Name');
      expect(element('.translations .category:first .translation:nth(1)').text()).toBe('Navn');
    });
  });


  describe('view1', function() {

    beforeEach(function() {
      browser().navigateTo('#/view1');
    });


    it('should render view1 when user navigates to /view1', function() {
      expect(element('[ng-view] p:first').text()).
        toMatch(/partial for view 1/);
    });

  });


  describe('view2', function() {

    beforeEach(function() {
      browser().navigateTo('#/view2');
    });


    it('should render view2 when user navigates to /view2', function() {
      expect(element('[ng-view] p:first').text()).
        toMatch(/partial for view 2/);
    });

  });
});
