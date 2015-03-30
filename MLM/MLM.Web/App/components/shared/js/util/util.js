var util = {
	getLocalDate: function (date, offset) {

		//get around I.E formatting issue
		if (date) {
			var segments = date.split('.');
			if (segments[segments.length - 1].length == 3) {
				date = date.replace(segments[segments.length - 1], "333Z");
			}
		}
		var d = new Date(date);
		//Deal with dates in milliseconds for most accuracy
		var utc = d.getTime() + (d.getTimezoneOffset() * 60000);
		var newDateWithOffset = new Date(utc + (60000 * offset));
		return newDateWithOffset;
	},
	getFormattedTime: function (hour, minutes) {
		var hours = hour == 0 ? "12" : hour > 12 ? hour - 12 : hour;
		var ampm = hour < 12 ? "am" : "pm";

		var formattedTime = "";
		formattedTime = minutes > 0 ? (hours + ":" + minutes + ampm) : (hours + ampm);
		return formattedTime;
	},
	convertTimeToDate: function (startTime, timeString, offset) {

		var startDate = new Date(startTime.getTime());
		var localOffset = startDate.getTimezoneOffset();
		var startTimeMilliSeconds = 60000 * (startDate.getHours() * 60 + startDate.getMinutes());
		var timeMilliSeconds = (60 * moment(timeString, 'HH:m A').hours() + moment(timeString, 'HH:m A').minutes()) * 60000;
		var timeToDate;
		if (offset) {
			timeToDate = new Date(startDate.setTime(timeMilliSeconds - startTimeMilliSeconds + startDate.getTime() - (localOffset + offset) * 60000));
		} else {
			timeToDate = new Date(startDate.setTime(timeMilliSeconds - startTimeMilliSeconds + startDate.getTime()));
		}

		return timeToDate;
	},

	/**
	 * Determine if two time objects are on the same calendar day
	 * @param  {Date} date1 [description]
	 * @param  {Date} date2 [description]
	 * @return {boolean} true if dates are identical
	 */
	compareDate: function (date1, date2) {
		return date1.getFullYear() === date2.getFullYear() 
			&& date1.getMonth() === date2.getMonth() 
			&& date1.getDate() === date2.getDate() 
	},

	/*
     * Test existence of keypath on an object
     */
	isSet: function (obj, propStr) {
		var parts = propStr.split(".");
		var cur = obj;
		for (var i = 0; i < parts.length; i++) {
			if (!cur[parts[i]])
				return false;
			cur = cur[parts[i]];
		}
		return true;
	},

	/*
     * Get a value at a given keypath if it exists. Returns undefined if keypath
     * doesn't exist. 
     */
	valueAtKeyPath: function (obj, propStr) {
		var parts = propStr.split(".");
		var cur = obj;
		for (var i = 0; i < parts.length; i++) {
			if (!cur[parts[i]])
				return undefined;
			cur = cur[parts[i]];
		}
		return cur;
	},


	// parseUri 1.2.2
	// (c) Steven Levithan <stevenlevithan.com>
	// MIT License
	parseUri: function (str) {
		if (!util.parseUri.hasOwnProperty("regexPattern")) {
			util.parseUri.regexPattern = /^(?:(?![^:@]+:[^:@\/]*@)([^:\/?#.]+):)?(?:\/\/)?((?:(([^:@]*)(?::([^:@]*))?)?@)?([^:\/?#]*)(?::(\d*))?)(((\/(?:[^?#](?![^?#\/]*\.[^?#\/.]+(?:[?#]|$)))*\/?)?([^?#\/]*))(?:\?([^#]*))?(?:#(.*))?)/;
			util.parseUri.uriFields = ["source", "protocol", "authority", "userInfo", "user", "password", "host", "port", "relative", "path", "directory", "file", "query", "anchor"];
		}
		var m = util.parseUri.regexPattern.exec(str);
		var uri = {};
		for (var i = 14; i > 0; i--)
			uri[util.parseUri.uriFields[i]] = m[i] || "";

		//uri[o.q.name] = {};
		//uri[o.key[12]].replace(o.q.parser, function ($0, $1, $2) {
		//	if ($1) uri[o.q.name][$1] = $2;
		//});

		return uri;
	}
};

util.getFormattedTimeFromDate = function(date){
        var localDate = util.getLocalDate(date.utc,date.offset);
        return util.getFormattedTime(localDate.getHours(),localDate.getMinutes());
}

util.getUtcMin = function(date){
    var localDate = util.getLocalDate(date.utc,date.offset);
    var min = localDate.getHours()*60+localDate.getMinutes();
    return min;
}

if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}



