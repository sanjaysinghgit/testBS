'use strict';

/**
 * Formats the time in accordance with the Timestamp
 * standard pattern. 
 *
 * TODO: handle internationalization
 */
mlm.filter('relativeTime', function () {
    return function (target) {
        // the current time in UTC
        var now = new Date();
        // the time we are formatting
        var then = new Date(target);
        // the difference between now and then
        // in milliseconds
        var duration            = now - then;
        var durationInMinutes   = Math.floor(duration / (1000*60));
        var durationInHours     = Math.floor(duration / (1000*60*60));
        var durationInDays      = Math.floor(duration / (1000*60*60*24));
        var durationInWeeks     = Math.floor(duration / (1000*60*60*24*7));
        var durationInMonths    = Math.floor(duration / (1000*60*60*24*7*30));
        var durationInYears     = Math.floor(duration / (1000*60*60*24*7*365));

        // the formatted time
        var formattedTime;

        // display year when older than a year
        if (durationInDays > 365) {
            formattedTime = moment(then).format("D MMM, YYYY");
        }
        // if older than a month, display month
        else if (durationInDays > 30) {
            formattedTime = moment(then).format("D MMM");
        } 
        // if older than 6 days, display in weeks
        else if (durationInDays > 6) {
            formattedTime = durationInWeeks + "w";
        } 
        else if (durationInDays > 0) {
            formattedTime = durationInDays + "d";  
        }
        else if (durationInHours > 0) {
            formattedTime = durationInHours + "h";  
        }
        else if (durationInMinutes > 1) {
            formattedTime = durationInMinutes + "m";
        } else {
            formattedTime = "just now";
        }

        return formattedTime;
    };
});