'use strict';

angular.module('translator.services', ['ngResource']).
  value('core', translator.core).

  factory('storage', function ($resource, $http, core) {
    var translations;
    var languages;

    return {
      all: function(filter) {
        var res = $resource('/api/translations').query(function(result) {
          core.toArray(translations, result, filter);
        });
        if (!translations) {
          translations = res;
        }
        return translations;
      },
      save: function(translation, success) {
        $http.put('/api/translations', translation).success(function() {
          success(translation);
        });
      },
      allLanguages: function(filter) {
        var res = $resource('/api/languages').query(function(result) {
          core.toArray(languages, result, filter);
        });
        if (!languages) {
          languages = res;
        }
        return languages;
      }
    };
  });
