'use strict';

/* jasmine specs for controllers go here */

describe('TranslationsCtrl:', function () {
  var scope, ctrl, $httpBackend, $cookieStore, storage, $routeParams;
  
  beforeEach(function () {
    this.addMatchers({
      toEqualData: function (expected) {
        return angular.equals(this.actual, expected);
      }
    });
  });

  beforeEach(module('ngCookies', 'translator.services'));

  beforeEach(inject(function (_$httpBackend_, $rootScope, $controller, _$cookieStore_, _storage_, _$routeParams_) {
    // prepare
    $httpBackend = _$httpBackend_;
    $cookieStore = _$cookieStore_;
    $routeParams = _$routeParams_;
    scope = $rootScope.$new();
    ctrl = $controller;
    storage = _storage_;
  }));

  describe('when called', function () {
    
    beforeEach(function () {
      // setups
      $httpBackend.expectGET('/api/languages').
        respond([
          { Id: 'en', Name: 'English' },
          { Id: 'da', Name: 'Danish' }
        ]);
      $httpBackend.expectGET('/api/translations').
        respond([
          {
            Keyword: 'Name',
            Category: 'Dictionary',
            Translations: [
              {
                Key: '/Dictionary/Name',
                Keyword: 'Name',
                Value: 'Name',
                Language: 'en',
                Category: 'Dictionary'
              },
              {
                Key: '/Dictionary/Name',
                Keyword: 'Name',
                Value: 'Navn',
                Language: 'da',
                Category: 'Dictionary'
              }
            ]
          },
          {
            Keyword: 'Email',
            Category: 'Dictionary',
            Translations: [
              {
                Key: '/Dictionary/Email',
                Keyword: 'Email',
                Value: 'Email',
                Language: 'en',
                Category: 'Dictionary'
              },
              {
                Key: '/Dictionary/Email',
                Keyword: 'Email',
                Language: 'da',
                Category: 'Dictionary'
              }
            ]
          },
          {
            Keyword: 'Hello',
            Category: 'Header',
            Translations: [
              {
                Key: '/Header/Hello',
                Keyword: 'Hello',
                Value: 'Hello, world!',
                Language: 'en',
                Category: 'Header'
              },
              {
                Key: '/Header/Hello',
                Keyword: 'Hello',
                Value: 'Hej, verden!',
                Language: 'da',
                Category: 'Header'
              }
            ]
          }
        ]);

      // act
      ctrl(TranslationsCtrl, { $scope: scope, $cookieStore: $cookieStore, storage: storage, $routeParams: $routeParams });
    });


    it('should create langs model with languages got from storage', function () {
      expect(scope.langs).toEqualData([]);

      $httpBackend.flush();

      expect(scope.langs.length).toBe(2);

      expect(scope.langs[0].Id).toBe('en');
      expect(scope.langs[0].Name).toBe('English');

      expect(scope.langs[1].Id).toBe('da');
      expect(scope.langs[1].Name).toBe('Danish');
    });


    it('should select all languages', function () {
      $httpBackend.flush();

      expect(scope.langs.length).toBe(2);

      expect(scope.langs[0].selected).toBeTruthy();
      expect(scope.langs[1].selected).toBeTruthy();
    });


    it('should create categories model with translations from storage grouped by category', function () {
      expect(scope.categories).toEqualData([]);

      $httpBackend.flush();

      expect(scope.categories).toEqualData([
        {
          Name: 'Dictionary',
          Entries: [
            {
              Keyword: 'Name',
              Category: 'Dictionary',
              Translations: [
                {
                  Key: '/Dictionary/Name',
                  Keyword: 'Name',
                  Value: 'Name',
                  Language: 'en',
                  Category: 'Dictionary'
                },
                {
                  Key: '/Dictionary/Name',
                  Keyword: 'Name',
                  Value: 'Navn',
                  Language: 'da',
                  Category: 'Dictionary'
                }
              ]
            },
            {
              Keyword: 'Email',
              Category: 'Dictionary',
              Translations: [
                {
                  Key: '/Dictionary/Email',
                  Keyword: 'Email',
                  Value: 'Email',
                  Language: 'en',
                  Category: 'Dictionary'
                },
                {
                  Key: '/Dictionary/Email',
                  Keyword: 'Email',
                  Language: 'da',
                  Category: 'Dictionary'
                }
              ]
            }
          ]
        },
        {
          Name: 'Header',
          Entries: [
            {
              Keyword: 'Hello',
              Category: 'Header',
              Translations: [
                {
                  Key: '/Header/Hello',
                  Keyword: 'Hello',
                  Value: 'Hello, world!',
                  Language: 'en',
                  Category: 'Header'
                },
                {
                  Key: '/Header/Hello',
                  Keyword: 'Hello',
                  Value: 'Hej, verden!',
                  Language: 'da',
                  Category: 'Header'
                }
              ]
            }
          ]
        }
      ]);
    });
  });


  describe('when called with selected languages in cookie', function () {

    beforeEach(function() {
      // setups
      $httpBackend.expectGET('/api/languages').
        respond([
          { Id: 'en', Name: 'English' },
          { Id: 'da', Name: 'Danish' }
        ]);
      $httpBackend.expectGET('/api/translations').respond([]);

      $cookieStore.put('selectedLangs', ['en']);

      // act
      ctrl(TranslationsCtrl, { $scope: scope, $cookieStore: $cookieStore, storage: storage, $routeParams: $routeParams });
    });


    it('should select only languages defined in cookie', function () {
      $httpBackend.flush();
      
      expect(scope.langs.length).toBe(2);

      expect(scope.langs[0].selected).toBeTruthy();
      expect(scope.langs[1].selected).toBeFalsy();
    });
  });


  describe('when langs were changed', function () {

    beforeEach(function() {
      // setups
      $httpBackend.expectGET('/api/languages').
        respond([
          { Id: 'en', Name: 'English' },
          { Id: 'da', Name: 'Danish' }
        ]);
      $httpBackend.expectGET('/api/translations').respond([]);

      // act
      ctrl(TranslationsCtrl, { $scope: scope, $cookieStore: $cookieStore, storage: storage, $routeParams: $routeParams });
    });

    it('should update cookie with selected languages', function () {
      $httpBackend.flush();
      
      spyOn($cookieStore, 'put');
      
      scope.langs[1].selected = false;
      scope.$digest();

      expect($cookieStore.put).toHaveBeenCalledWith('selectedLangs', ['en']);
    });
  });


  describe('when called for category', function() {

    beforeEach(function () {
      // setups
      $httpBackend.expectGET('/api/languages').
        respond([
          { Id: 'en', Name: 'English' }
        ]);
      $httpBackend.expectGET('/api/translations').
        respond([
          {
            Keyword: 'Name',
            Category: 'Dictionary',
            Translations: [
              {
                Key: '/Dictionary/Name',
                Keyword: 'Name',
                Value: 'Name',
                Language: 'en',
                Category: 'Dictionary'
              }
            ]
          },
          {
            Keyword: 'Hello',
            Category: 'Header',
            Translations: [
              {
                Key: '/Header/Hello',
                Keyword: 'Hello',
                Value: 'Hello, world!',
                Language: 'en',
                Category: 'Header'
              }
            ]
          }
        ]);

      $routeParams.category = 'Dictionary';

      // act
      ctrl(TranslationsCtrl, {$scope: scope, $cookieStore: $cookieStore, storage: storage, $routeParams: $routeParams});
    });

    it('should set selected category model', function() {
      expect(scope.selectedCategory).toBe('Dictionary');
    });

    it('should set selected category active in translations', function () {
      $httpBackend.flush();
      
      expect(scope.categories[0].active).toBeTruthy();
    });
  });
});