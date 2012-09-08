if (!translator) var translator = {};
if (!$) var $ = function (onload) { onload(); };

// Contains styling and initialization logic
translator.Core = (function () {

  // Constructor.
  function Core() {
    this.init();
  }

  Core.prototype.init = function () {
  };

  Core.prototype.load = function () {
  };

  Core.prototype.notify = function(header, message) {
    $.jGrowl(message, {header: header, life: 2000});
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