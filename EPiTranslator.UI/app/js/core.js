if (!translator) var translator = {};

// Contains styling and initialization logic
translator.Core = (function () {

  // Constructor.
  function Core() {
    this.init();
  }

  // Initializes application and behaviors before document was loaded.
  Core.prototype.init = function () {
    $.ajaxSetup({ accepts: 'json' });
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