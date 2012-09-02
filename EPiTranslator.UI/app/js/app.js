'use strict';


// Declare app level module which depends on filters, and services
angular.module('translator', ['ngCookies', 'translator.filters', 'translator.services', 'translator.directives']).
  config(['$routeProvider', function($routeProvider) {
    $routeProvider.when('/translations', {templateUrl: 'partials/translations.html', controller: TranslationsCtrl});
    $routeProvider.otherwise({redirectTo: '/translations'});
  }]);