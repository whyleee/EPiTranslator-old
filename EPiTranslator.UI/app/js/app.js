'use strict';


// Declare app level module which depends on filters, and services
angular.module('translator', ['translator.filters', 'translator.services', 'translator.directives']).
  config(['$routeProvider', function($routeProvider) {
    $routeProvider.when('/view1', {templateUrl: 'partials/partial1.html', controller: MyCtrl1});
    $routeProvider.when('/view2', {templateUrl: 'partials/partial2.html', controller: MyCtrl2});
    $routeProvider.otherwise({redirectTo: '/view1'});
  }]);

if (!translator) var translator = {};

// Contains styling and initialization logic
translator.Core = (function () {

  // Constructor.
  function Core() {
    this.init();
  }

  // Initializes application and behaviors before document was loaded.
  Core.prototype.init = function () {
    $.ajaxSetup({accepts: 'json'});
  };

  // Initializes application and behaviors when document was loaded.
  Core.prototype.load = function () {
  };

  return Core;
})();

// Initialization for application before document was loaded.
(function () {
  translator.core = new translator.Core();
})();

// Initialization for application when document was loaded.
$(function () {
  translator.core.load();
});