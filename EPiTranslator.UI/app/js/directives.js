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

  // TODO: can this be changed to just adding selectedLangs.length to current item index?
  directive('ttNextprevrow', function () {
    return function (scope, elem, attrs) {
      elem.keyup(function (e) {
        if (e.keyCode == 38 || e.keyCode == 40) { // up or down arrow
          var down = e.keyCode == 40;
          var translationsSection = elem.parentsUntil('#translations').parent().eq(0);
          var row = elem.parentsUntil('tr').parent().eq(0);
          var colIndex = elem.parentsUntil('td').parent().index();
          var foundCurrent = false;
          var nextRow, prevRow;

          translationsSection.find('tr:not(.category-header)').each(function () {
            if (foundCurrent) {
              nextRow = $(this);
              return false;
            }
            if ($(this).is(row)) {
              foundCurrent = true;
            } else {
              prevRow = $(this);
            }
          });
          
          var nextPrevElem = down ? nextRow : prevRow;
          
          if (nextPrevElem) {
            var nextEntry = nextPrevElem.find('.translation:nth(' + (colIndex - 1) + ') .edit input');
            nextEntry.click();
            nextEntry.focus();
          }
        }
      });
    };
  }).
  
  directive('ttNextprevcol', function () {
    return function (scope, elem, attrs) {
      elem.keyup(function (e) {
        if ((e.keyCode == 37 || e.keyCode == 39) && e.ctrlKey) { // left or right arrow
          var next = e.keyCode == 39;
          var curIndex = elem.data('index');
          var prevNextIndex = next ? curIndex + 1 : curIndex - 1;
          
          if (prevNextIndex >= 0) {
            var prevNextElem = $('#translations .translation .edit input:nth(' + prevNextIndex + ')');
            prevNextElem.click();
            prevNextElem.focus();
          }
        }
      });
    };
  }).
  
  directive('ttTabbable', function () {
    return function (scope, elem, attrs) {
      elem.keydown(function (e) {
        if (e.keyCode == 9) { // TAB
          var next = !e.shiftKey;
          var curIndex = elem.data('index');
          var prevNextIndex = next ? curIndex + 1 : curIndex - 1;
          var translationsLength = $('#translations .translation .edit input').length;

          if (prevNextIndex >= 0 && prevNextIndex < translationsLength) {
            var prevNextElem = $('#translations .translation .edit input:nth(' + prevNextIndex + ')');
            prevNextElem.click();
            prevNextElem.focus();
            
            e.preventDefault();
          }
          else if (prevNextIndex == translationsLength) {
            elem.blur();
            $('.language-selector input:first').focus();
            
            e.preventDefault();
          }
        }
      });
    };
  }).
  
  directive('ttTablasttotable', function () {
    return function (scope, elem, attrs) {
      elem.keydown(function (e) {
        if (e.keyCode == 9 && elem.is(':last-child')) { // TAB
          var firstTranslation = $('#translations .translation .edit input:first');
          firstTranslation.click();
          firstTranslation.focus();

          e.preventDefault();
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