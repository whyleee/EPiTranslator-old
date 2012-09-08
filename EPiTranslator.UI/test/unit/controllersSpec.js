'use strict';

/* jasmine specs for controllers go here */

describe('TranslationsCtrl:', function () {
  var scope, ctrl, $httpBackend, $cookieStore, storage, $routeParams, core;
  
  beforeEach(function () {
    this.addMatchers({
      toEqualData: function (expected) {
        return angular.equals(this.actual, expected);
      }
    });
  });

  beforeEach(module('ngCookies', 'translator.services'));

  beforeEach(inject(function (_$httpBackend_, $rootScope, $controller, _$cookieStore_, _storage_, _$routeParams_, _core_) {
    // prepare
    $httpBackend = _$httpBackend_;
    $cookieStore = _$cookieStore_;
    $routeParams = _$routeParams_;
    scope = $rootScope.$new();
    ctrl = $controller;
    storage = _storage_;
    core = _core_;
  }));

  var call = function () {
    ctrl(TranslationsCtrl, { $scope: scope, $cookieStore: $cookieStore, storage: storage, $routeParams: $routeParams, core: core});
  };

  var mockHttp = function() {
    $httpBackend.expectGET('/api/languages').respond([]);
    $httpBackend.expectGET('/api/translations').respond([]);
  };

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
      call();
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
      call();
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
      call();
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
      call();
    });

    it('should set selected category model', function() {
      expect(scope.selectedCategory).toBe('Dictionary');
    });

    it('should set selected category active in translations', function () {
      $httpBackend.flush();
      
      expect(scope.categories[0].active).toBeTruthy();
    });
  });

  describe('when started translation editing', function () {

    var editedTranslation;

    beforeEach(function () {
      // setups
      mockHttp();
      
      editedTranslation = { Keyword: 'Hello', Value: 'Hello, world!' };

      // act
      call();
      scope.edit(editedTranslation);
    });

    it('should save current translation value in a separate property', function() {
      expect(editedTranslation.prevVal).toBe('Hello, world!');
    });

    it('should set translation status to was translated', function() {
      expect(editedTranslation.wasTranslated).toBeTruthy();
    });

    it('should set editedTranslation in scope', function() {
      expect(scope.editedTranslation).toBe(editedTranslation);
    });
  });

  describe('when translation editing finished but value was not changed', function() {

    var editedTranslation;

    beforeEach(function() {
      // setups
      mockHttp();
      
      editedTranslation = { Keyword: 'Hello', Value: 'Hello, world!' };
      
      spyOn(storage, 'save');

      // act
      call();
      scope.edit(editedTranslation);
      scope.doneEditing(editedTranslation);
    });

    it('should not call server for updation', function() {
      expect(storage.save).not.toHaveBeenCalled();
    });

    it('should set translation status to wasnt translated', function() {
      expect(editedTranslation.wasTranslated).toBeFalsy();
    });
  });

  describe('when translation editing finished and value is empty', function() {

    var editedTranslation;

    beforeEach(function () {
      // setups
      mockHttp();
      
      editedTranslation = { Keyword: 'Hello', Value: 'Hello, world!' };

      spyOn(storage, 'save');

      // act
      call();
      scope.edit(editedTranslation);

      editedTranslation.Value = '';
      scope.doneEditing(editedTranslation);
    });
    
    it('should restore translation value to previous', function () {
      expect(editedTranslation.Value).toBe('Hello, world!');
    });

    it('should not call server for updation', function () {
      expect(storage.save).not.toHaveBeenCalled();
    });
  });

  describe('when translation editing finished and value was changed', function () {
    
    var editedTranslation;

    beforeEach(function () {
      // setups
      $httpBackend.expectGET('/api/languages').
        respond([
          { Id: 'en', Name: 'English' },
          { Id: 'da', Name: 'Danish' }
        ]);
      $httpBackend.expectGET('/api/translations').respond([]);
      $httpBackend.whenPUT('/api/translations').respond(200);

      editedTranslation = { Keyword: 'Hello', Value: 'Hello, world!', Language: 'en', IsFallback: true };
      scope.langs = [{Id: 'en', Name: 'English'}, {Id: 'da', Name: 'Danish'}];

      spyOn(storage, 'save').andCallThrough();
      spyOn(core, 'notify');

      // act
      call();
      scope.edit(editedTranslation);

      editedTranslation.Value = 'Hi guys!';
      scope.doneEditing(editedTranslation);
    });

    it('should update previous translation value to new one', function () {
      expect(editedTranslation.prevVal).toBe('Hi guys!');
    });

    it('should clear edited translation in scope', function() {
      expect(scope.editedTranslation).toBeNull();
    });

    it('should call server to update translation', function () {
      expect(storage.save).toHaveBeenCalledWith(editedTranslation, jasmine.any(Function));
    });

    it('should set translated updated status to true', function () {
      $httpBackend.flush();
      
      expect(editedTranslation.updated).toBeTruthy();
    });

    it('should notify user that translation was updated', function() {
      $httpBackend.flush();

      expect(core.notify).toHaveBeenCalledWith('Translation updated', "<b>English</b>: 'Hello' to <b>'Hi guys!'</b>");
    });

    it('should unset fallback status from translation', function() {
      $httpBackend.flush();

      expect(editedTranslation.IsFallback).toBeFalsy();
    });
  });
});