'use strict';

mlm.filter('capitalize', function () {
    return function (input, scope) {
        return input.charAt(0).toUpperCase() + input.slice(1);
    }
});