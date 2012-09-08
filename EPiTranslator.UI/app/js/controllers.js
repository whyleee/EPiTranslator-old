'use strict';

function TranslationsCtrl($scope, $cookieStore, storage, $routeParams, core) {

  $scope.onlyNotTranslated = $routeParams.category == 'not-translated';
  $scope.selectedCategory = !$scope.onlyNotTranslated ? $routeParams.category : null;

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