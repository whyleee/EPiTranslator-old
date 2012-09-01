'use strict';

/* jasmine specs for controllers go here */

  
describe('TranslationsCtrl', function () {
  var scope, ctrl, $httpBackend;

  beforeEach(inject(function (_$httpBackend_, $rootScope, $controller) {
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
            Language: 'en', Entries: [
              {
                Keyword: 'Name',
                Value: 'Name',
                Language: 'en',
                Category: 'Dictionary'
              },
              {
                Keyword: 'Email',
                Value: 'Email',
                Language: 'en',
                Category: 'Dictionary'
              },
              {
                Keyword: 'Hello',
                Value: 'Hello, world!',
                Language: 'en',
                Category: 'Header'
              }
            ]
          },
          {
            Language: 'da', Entries: [
              {
                Keyword: 'Name',
                Value: 'Navn',
                Language: 'da',
                Category: 'Dictionary'
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
    ctrl = $controller(TranslationsCtrl, { $scope: scope });
  }));

  it('should create langs model with 2 languages fetched from xhr', function () {
    expect(scope.langs).toBeUndefined();

    $httpBackend.flush();

    expect(scope.langs).toEqual([
      { Id: 'en', Name: 'English' },
      { Id: 'da', Name: 'Danish' }
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
