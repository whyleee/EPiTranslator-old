'use strict';

function TranslationsCtrl($scope, $cookieStore, storage) {
  $scope.langs = storage.allLanguages(function (result) {
    var selectedLangs = $cookieStore.get('selectedLangs');
    
    if (selectedLangs && selectedLangs.length > 0) {
      _.each(result, function (lang) { lang.selected = _.any(selectedLangs, function (langId) { return langId == lang.Id; }); });
    } else {
      _.each(result, function(lang) { lang.selected = true; });
    }
  });
  $scope.categories = storage.all(function (result) {
    return _.groupBy(result, 'Category');
  });

  $scope.$watch('langs', function () {
    if ($scope.langs.length > 0) {
      $cookieStore.put('selectedLangs', $scope.selectedLangs);
    }
  }, /* compare by equality */ true);
  
  // helpers
  Object.defineProperty($scope, 'selectedLangs', {
    get: function () { return _.pluck($scope.langs.filter(function (lang) { return lang.selected; }), 'Id'); }
  });
  
  // filters
  $scope.selected = function(obj) {
    return obj.selected;
  };
  $scope.forSelectedLangs = function(translation) {
    return _.any($scope.selectedLangs, function (lang) {return lang == translation.Language;});
  };
}