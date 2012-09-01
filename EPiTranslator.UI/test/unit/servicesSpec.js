'use strict';

/* jasmine specs for services go here */

describe('', function () {

  beforeEach(function() {
    this.addMatchers({
      toEqualData: function(expected) {
        return angular.equals(this.actual, expected);
      }
    });
  });

  beforeEach(module('translator.services'));

  describe('Storage service:', function () {
    

    describe('when asked for all languages', function () {
      var $httpBackend, result;

      beforeEach(inject(function (_$httpBackend_, storage) {
        // prepare
        $httpBackend = _$httpBackend_;

        // setups
        $httpBackend.expectGET('/api/languages').
          respond([
            { Id: 'en', Name: 'English' },
            { Id: 'da', Name: 'Danish' }
          ]);

        // act
        result = storage.allLanguages();
      }));

      it('should return languages fetched from the server', function () {
        expect(result).toEqual([]);

        $httpBackend.flush();

        expect(result).toEqualData([
          { Id: 'en', Name: 'English' },
          { Id: 'da', Name: 'Danish' }
        ]);
      });
    });
    

    describe('when asked for all translations', function () {
      var $httpBackend, result;

      beforeEach(inject(function (_$httpBackend_, storage) {
        // prepare
        $httpBackend = _$httpBackend_;

        // setups
        $httpBackend.expectGET('/api/translations').
          respond([
            {
              Language: 'en', Entries: [
                {Keyword: 'Name', Value: 'Name'},
                {Keyword: 'Email', Value: 'Email'}
              ]
            },
            {
              Language: 'da', Entries: [
                {Keyword: 'Name', Value: 'Navn'}
              ]
            }
          ]);

        // act
        result = storage.all();
      }));

      it('should return translations fetched from the server', function () {
        expect(result).toEqual([]);

        $httpBackend.flush();

        expect(result).toEqualData([
          {
            Language: 'en', Entries: [
              { Keyword: 'Name', Value: 'Name' },
              { Keyword: 'Email', Value: 'Email' }
            ]
          },
          {
            Language: 'da', Entries: [
              { Keyword: 'Name', Value: 'Navn' }
            ]
          }
        ]);
      });
    });
  });
});
