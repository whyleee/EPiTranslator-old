'use strict';

/* jasmine specs for controllers go here */


describe('TranslationsCtrl: when called', function () {
  var scope, ctrl, $httpBackend;
  
  beforeEach(function () {
    this.addMatchers({
      toEqualData: function (expected) {
        return angular.equals(this.actual, expected);
      }
    });
  });

  beforeEach(module('translator.services'));

  beforeEach(inject(function (_$httpBackend_, $rootScope, $controller, storage)  {
    // prepare
    $httpBackend = _$httpBackend_;
    scope = $rootScope.$new();

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
              Keyword: 'Name',
              Value: 'Name',
              Language: 'en',
              Category: 'Dictionary'
            },
            {
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
              Keyword: 'Email',
              Value: 'Email',
              Language: 'en',
              Category: 'Dictionary'
            },
            {
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
              Keyword: 'Hello',
              Value: 'Hello, world!',
              Language: 'en',
              Category: 'Header'
            },
            {
              Keyword: 'Hello',
              Value: 'Hej, verden!',
              Language: 'da',
              Category: 'Header'
            }
          ]
        }
      ]);

    // act
    ctrl = $controller(TranslationsCtrl, {$scope: scope, storage: storage});
  }));

  it('should create langs model with languages got from storage', function () {
    expect(scope.langs).toEqualData([]);
    
    $httpBackend.flush();
    
    expect(scope.langs).toEqualData([
      { Id: 'en', Name: 'English' },
      { Id: 'da', Name: 'Danish' }
    ]);
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
                Keyword: 'Name',
                Value: 'Name',
                Language: 'en',
                Category: 'Dictionary'
              },
              {
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
                Keyword: 'Email',
                Value: 'Email',
                Language: 'en',
                Category: 'Dictionary'
              },
              {
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
                Keyword: 'Hello',
                Value: 'Hello, world!',
                Language: 'en',
                Category: 'Header'
              },
              {
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

describe('MyCtrl1', function(){
  var myCtrl1;

  beforeEach(function(){
    myCtrl1 = new MyCtrl1();
  });


  it('should ....', function() {
    //spec body
  });
});


describe('MyCtrl2', function(){
  var myCtrl2;


  beforeEach(function(){
    myCtrl2 = new MyCtrl2();
  });


  it('should ....', function() {
    //spec body
  });
});
