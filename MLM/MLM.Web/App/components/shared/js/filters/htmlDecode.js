'use strict';

mlm.filter('htmlDecode', function () {
    return function (input, scope) {
        var txt = document.createElement("textarea");
        txt.innerHTML = input;
        return txt.value;
        }
});