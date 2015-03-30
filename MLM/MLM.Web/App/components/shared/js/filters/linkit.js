'use strict';

mlm.filter('parseForLinks', ['linkyFilter', function (linkyFilter) {
    return function (input, scope) {
        var safe = $.trim(input).match(/^(ftp|ftps):\/\/(.*)(\.(exe|dll|bat|scr|pif))$/);
        if (!safe) {
            return linkyFilter(input, '_blank');
        } else {
            return input;
        }
    }
}]);