'use strict';

function TranslationsCtrl($scope, $cookieStore, storage, $routeParams, core, $location) {

  var TRANSLATIONS_PER_PAGE = 40;

  $scope.onlyNotTranslated = $routeParams.category == 'not-translated';
  $scope.selectedCategory = !$scope.onlyNotTranslated ? $routeParams.category : null;

  $scope.page = $routeParams.p ? parseInt($routeParams.p) : 1;
  $scope.pagesCount = 1;
  $scope.categoriesForPage = [];

  // set languages
  $scope.langs = storage.allLanguages(function (result) {
    var selectedLangs = $cookieStore.get('selectedLangs');
    
    if (selectedLangs && selectedLangs.length > 0) {
      _.each(result, function (lang) {
        lang.selected = _.any(selectedLangs,
          function (langId) { return langId == lang.Id; }
        );
      });
    } else {
      _.each(result, function(lang) { lang.selected = true; });
    }
  });
  
  // set translations grouped by category
  $scope.categories = storage.all(function (result) {
    var all = _.groupBy(result, 'Category');
    if ($routeParams.category && !$scope.onlyNotTranslated) {
      all[$routeParams.category].active = true;
    }

    var translations = result;
    if ($routeParams.category && !$scope.onlyNotTranslated) {
      translations = all[$routeParams.category];
    }
    
    $scope.categoriesForPage = core.toArray([], translations, function (translations) {
      return _.chain(translations)
        .tail(($scope.page - 1) * TRANSLATIONS_PER_PAGE)
        .take(TRANSLATIONS_PER_PAGE)
        .groupBy('Category')
        .value();
    });
    $scope.pagesCount = Math.ceil(translations.length / TRANSLATIONS_PER_PAGE);
    
    return all;
  });

  // set translation into the edit mode
  $scope.edit = function (translation) {
    if ($scope.editedTranslation != translation) {
      translation.prevVal = translation.Value;
      translation.wasTranslated = true;
      
      $scope.editedTranslation = translation;
    }
  };

  // update translation in storage
  $scope.doneEditing = function (translation) {
    if (!translation.Value) {
      translation.Value = translation.prevVal;
    }
    if (translation.Value != translation.prevVal) {
      translation.prevVal = translation.Value;
      $scope.editedTranslation = null;
      
      storage.save(translation, function () {
        notifyUpdated(translation);
        
        translation.updated = true;
        translation.IsFallback = false;
      });
    } else {
      translation.wasTranslated = false;
    }
  };
  
  // undo edited translation
  $scope.undoEditing = function(translation) {
    translation.Value = translation.prevVal;
    translation.canceling = false;
    translation.wasTranslated = false;
  };

  // watchers
  $scope.$watch('langs', function () {
    if ($scope.langs.length > 0) {
      $cookieStore.put('selectedLangs', $scope.selectedLangs);
    }
  }, /* compare by equality */ true);
  
  // helpers
  Object.defineProperty($scope, 'selectedLangs', {
    get: function () {
      return _.pluck($scope.langs.filter(function (lang) {return lang.selected;}), 'Id');
    }
  });

  $scope.range = function(from, to) {
    return _.range(from, to);
  };

  $scope.urlToPage = function(p) {
    return '#' + $location.path() + '?p=' + p;
  };

  $scope.computeTranslationIndex = function (scope) {
    var categoryIndex = scope.$parent.$parent.$index;
    var totalTranslationsInPrevCategories = 0;

    for (var i = 0; i < categoryIndex; i++) {
      var entries = scope.categories[i].Entries;
      
      for (var j = 0; j < entries.length; j++) {
        totalTranslationsInPrevCategories += scope.selectedLangs.length;
      }
    }

    return scope.$index + scope.$parent.$index * scope.selectedLangs.length + categoryIndex * totalTranslationsInPrevCategories;
  };
  
  // show notify message that translation was updated
  var notifyUpdated = function (translation) {
    var language = _.find($scope.langs, function (lang) {
      return lang.Id == translation.Language;
    });

    var message = _.str.sprintf("<b>%s</b>: '%s' to <b>'%s'</b>",
      language.Name, translation.Keyword, translation.Value);

    core.notify('Translation updated', message);
  };
  
  // filters
  $scope.selected = function(obj) {
    return obj.selected;
  };
  $scope.forSelectedLangs = function(translation) {
    return _.any($scope.selectedLangs, function (lang) {return lang == translation.Language;});
  };
  $scope.notTranslatedIfSelected = function (entry) {
    return !$scope.onlyNotTranslated || _.any(_.filter(entry.Translations, $scope.forSelectedLangs),
        function (translation) {return translation.wasTranslated || !translation.Value; }
      );
  };
  $scope.haveNotTranslatedIfSelected = function(category) {
    return _.any(category.Entries, $scope.notTranslatedIfSelected);
  };
}