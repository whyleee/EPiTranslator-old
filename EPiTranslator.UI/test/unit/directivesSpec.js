'use strict';

/* jasmine specs for directives go here */

describe('directives', function() {
  beforeEach(module('translator.directives'));

  // Test directive is removed. Left here for example purposes.
  /*describe('app-version', function() {
    it('should print current version', function() {
      module(function($provide) {
        $provide.value('version', 'TEST_VER');
      });
      inject(function($compile, $rootScope) {
        var element = $compile('<span app-version></span>')($rootScope);
        expect(element.text()).toEqual('TEST_VER');
      });
    });
  });*/
});
