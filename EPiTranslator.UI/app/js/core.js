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

  // Converts specified object to array, filtering the result.
  Core.prototype.toArray = function (store, result, filter) {
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

    return store;
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