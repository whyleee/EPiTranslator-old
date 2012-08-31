'use strict';

/* Controllers */
function TranslationsCtrl($scope, $http) {
  $http.get('/api/languages').success(function(data) {
    $scope.langs = data;
  });
  $http.get('/api/translations').success(function(data) {
    $scope.entries = data;
  });
}

function MyCtrl1() {}
MyCtrl1.$inject = [];


function MyCtrl2() {
}
MyCtrl2.$inject = [];
