'use strict';

/* Directives */
angular.module('translator.directives', []).

  /* Executes an expression when the element it is applied loses focus */
  directive('jqBlur', function () {
    return function (scope, elem, attrs) {
      elem.blur(function () {
        if (!scope.translation.updated && !scope.translation.canceling) {
          scope.$apply(attrs.jqBlur);
        }
      });
    };
  }).

  /* Makes an element to loose a focus when given expression is true */
  directive('jqDoblur', function () {
    return function (scope, elem, attrs) {
      var _scope = scope;
      var _elem = elem;
      var _attrs = attrs;

      _scope.$watch(_attrs.jqDoblur, function (truthy) {
        if (truthy) {
          _elem.blur();
          _scope.translation.updated = false;
        }
      });
    };
  }).
  
  /* Executes an expression when mouse is enters or leaves the element (enter_expr|leave_expr) */
  directive('jqEnterleave', function () {
    return function (scope, elem, attrs) {
      elem.hover(function () {
        scope.$apply(_.str.words(attrs.jqEnterleave, '|')[0]);
      }, function () {
        scope.$apply(_.str.words(attrs.jqEnterleave, '|')[1]);
      });
    };
  }).
  
  /* Executes an expression when ESC key was pressed */
  directive('jqEsc', function () {
    return function (scope, elem, attrs) {
      elem.keyup(function (e) {
        if (e.keyCode == 27) { // ESC
          scope.$apply(attrs.jqEsc);
        }
      });
    };
  }).

  /* Shows tooltip when specified expression is true */
  directive('tbTooltip', function () {
    return function (scope, elem, attrs) {
      var _scope = scope;
      var _elem = elem;
      var _attrs = attrs;

      _scope.$watch(_attrs.tbTooltip, function (value) {
        if (value) {
          _elem.tooltip('show');
        } else {
          _elem.tooltip('hide');
        }
      });

      _elem.focus(function () {
        if (_scope.$apply(_attrs.tbTooltip)) {
          _elem.tooltip('show');
        } else {
          _elem.tooltip('hide');
        }
      });

      _elem.blur(function () {
        _elem.tooltip('hide');
      });
    };
  });