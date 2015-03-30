'use strict'

mlm.directive('bzFileInput', function(){
    return {
        scope: {
            file: '=',  
            onFileChange: '='
        },
        link: function(scope, el, attrs){
            el.bind('change', function(event){
                var files = event.target.files;
                var file = files[0];
                scope.file = file ? file.name : undefined;
                scope.$apply();
                if (_.isFunction(scope.onFileChange)) {
                	scope.onFileChange(files);
                }
            });
        }
    };
});