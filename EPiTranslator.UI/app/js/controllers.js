'use strict';

/* Controllers */
function TranslationsCtrl($scope, storage) {
  $scope.langs = storage.allLanguages();
  $scope.categories = storage.all(function (result) {
    return _.groupBy(result, 'Category');
  });
}

function MyCtrl1() {}
MyCtrl1.$inject = [];


function MyCtrl2() {
}
MyCtrl2.$inject = [];
