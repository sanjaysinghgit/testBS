'use strict';
mlm.filter('timeSpanFormat', ['localization', function (localization) {
    return function (text) {
        if (text == undefined) {
            return '';
        }
        try
        {
            var formatted = '';
            switch (text) {
                case '00:00:00':
                    formatted = localization.t('datetime.spans.none');
                    break;
                case '00:30:00':
                    formatted = "30 " + localization.t('datetime.spans.minutes');
                    break;
                case '01:00:00':
                    formatted = "1 " + localization.t('datetime.spans.hour');
                    break;
                default:
                    var parts = text.split(':');
                    var num = Number(parts[0]);
                    formatted = num.toString() + ' ' + localization.t('datetime.spans.hours');
                    break;            
            }
        }
        catch(error){
        }
          return formatted;
      }
  }]);