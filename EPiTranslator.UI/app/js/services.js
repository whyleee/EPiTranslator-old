'use strict';

angular.module('translator.services', ['ngResource']).
  value('core', translator.core).

  factory('storage', function ($resource, $http) {
    var translations;
    var languages;

    // Filters result using provided function.
    var filterResult = function(store, result, filter) {
      if (!filter) {
        return;
      }

      var filtered = filter(result);

      if (!filtered) {
        return;
      }

      store.length = 0;

      if (filtered instanceof Array) {
        store.push.apply(store, filtered);
      } else {
        for (var prop in filtered) {
          var item = { Name: prop, Entries: filtered[prop] };
          for (var entry in item.Entries) {
            if (isNaN(parseInt(entry))) {
              item[entry] = item.Entries[entry];
            }
          }
          store.push(item);
        }
      }
    };

    return {
      all: function(filter) {
        var res = $resource('/api/translations').query(function(result) {
          filterResult(translations, result, filter);
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
          filterResult(languages, result, filter);
        });
        if (!languages) {
          languages = res;
        }
        return languages;
      }
    };
  });
