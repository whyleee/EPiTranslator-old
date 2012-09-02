'use strict';

/* Services */


// Demonstrate how to register services
// In this case it is a simple value service.
angular.module('translator.services', ['ngResource']).
  factory('storage', function ($resource) {
    var translations;
    var languages;

    // Filters result using provided function.
    var filterResult = function(result, filter) {
      if (!filter) {
        return;
      }

      var filtered = filter(result);
      
      if (!filtered) {
        return;
      }

      result.length = 0;

      if (filtered instanceof Array) {
        result.push.apply(result, filtered);
      } else {
        for (var prop in filtered) {
          result.push({Name: prop, Entries: filtered[prop]});
        }
      }
    };

    return {
      all: function(filter) {
        if (!translations) {
          translations = $resource('/api/translations').query(function () {
            filterResult(translations, filter);
          });
        }
        return translations;
      },
      allLanguages: function (filter) {
        if (!languages) {
          languages = $resource('/api/languages').query(function () {
            filterResult(languages, filter);
          });
        }
        return languages;
      }
    };
  });
