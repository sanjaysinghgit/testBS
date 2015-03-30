'use strict';

/**
 * This filter accepts a start time, an end time, and a time increment and then
 * returns an array of Date objects representing all of the steps in between. 
 *
 * ie. If I provide [12pm, 2pm, 30 minutes] it would return 
 * [   12pm, 
 *     12:30pm,
 *     1pm,
 *     1:30pm,
 *     2pm
 * ]
 *
 * * the above are presented in human readable time just to simplify things
 */

mlm.filter('makeRange', function() {

    return function(input) {

        var lowBound, highBound, step;

        // if there are 3 things
        if (input.length == 3) {

            // and the first one is a Date object
            if(input[0] instanceof Date) {

                // prepare the array to return
                var result = [];

                // get the hours of the first object
                lowBound = input[0].getHours();

                // get the hours of the last object
                highBound = input[1].getHours();

                // figure out what the steps are supposed to be in between
                step = parseInt(input[2]);
               
                // if the objects are the same
                if(     lowBound === highBound 
                    &&  input[0].getMinutes() === input[1].getMinutes() )
                {
                    // push the formatted time into the results
                    result.push(
                        util.getFormattedTime(
                            input[1].getHours(),
                            input[1].getMinutes()
                        )
                    );

                    // and exit
                    return result;
                }
                
                // otherwise we need to iterate through each hour provided in the 
                // date range
                for ( var i = lowBound; i <= highBound; i++) 
                {

                    // if we are at the low bound, but the minutes are past the hour
                    // we round up to the next closest time increment
                    // ... basically using Price Is Right rules: closest step without 
                    // going over
                    if (    i === lowBound 
                        &&  input[0].getMinutes() > 0)
                    {
                        result.push(
                            util.getFormattedTime(
                                input[0].getHours(),
                                step
                            )
                        );
                    } 

                    // if we are at the *exact* highbound for the range, add that 
                    // to the time range
                    else if (   i === highBound 
                            &&  input[1].getMinutes() === 0)
                    {
                        result.push(
                            util.getFormattedTime(
                                i,
                                0
                            )
                        );
                    }

                    // by default, we add two entries to the range: one for this 
                    // hour, and then one for the half hour increment
                    else {
                        result.push(
                            util.getFormattedTime(
                                i,
                                0
                            )
                        );
                        result.push(
                            util.getFormattedTime(
                                i,
                                step
                            )
                        );
                    }
                    
                }

                // if we are looking at the end of the day, do not do any 
                // rounding because this is a hack for us to book until the 
                // second before midnight (12am) of tomorrow
                if(     input[1].getHours() == '23' 
                    &&  input[1].getMinutes() == '59')
                {
                    result.push(
                        util.getFormattedTime(
                            input[1].getHours(),
                            input[1].getMinutes()
                        )
                    );
                }
                
                return result;
            }

        } else {

            switch (input.length) {
                // for a single input, assume that the start time was 12am
                case 1:                                 
                    lowBound    = 0;
                    highBound   = parseInt(input[0]) - 1;
                    break;
                // for two inputs, assume that the step is 1 full hour
                case 2:
                    lowBound    = parseInt(input[0]);
                    highBound   = parseInt(input[1]);
                    break;
                default:
                    return input;
            }

            var result = [];
            for (var i = lowBound; i <= highBound; i++)
                result.push(i);
            return result;
        }
    };
});