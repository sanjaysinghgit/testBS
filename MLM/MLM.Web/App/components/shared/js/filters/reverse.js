'use strict';

mlm.filter('reverse', function () {
    return function(items) {
        if(_.isArray(items))
            return items.slice().reverse();
    };
});