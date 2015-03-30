
mlm.directive('ngFormflasher', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            var form = $(element);
            
            //on blur check if its invalid and show message

            form.find("[type='submit']").on('click', function () {

                var invalids = form.find("[class*=ng-invalid]");

                invalids.addClass("no-input");

                setTimeout(function () {
                    invalids.removeClass("no-input");
                }, 500);
              
                invalids.eq(0).focus();

            });
        }
    }
});