'use strict';
/**
 * Uses the Sonic library to draw a bazinga spinner 
 * using canvas
 *
 * TODO: fallback to gif for older browsers
 * 
 */
mlm.directive('bzSpinner', function () {
    return {
        link: function (scope, element, attrs) {

        	var size = 100,
        		thickness = 8;

        	element.addClass('bz-spinner');

        	if (!_.isUndefined(attrs.small)) {
        		size = 48;
        		thickness = 4;
        		element.addClass('bz-spinner--small');
        	} 
        	else if (!_.isUndefined(attrs.tiny)) {
        		size = 36;
        		thickness = 3;
        		element.addClass('bz-spinner--tiny');
        	} 

        	var spinner = new Sonic({
			
				width: size,
				height: size,

				stepsPerFrame: 5,    // best between 1 and 5
				trailLength: 1,    // between 0 and 1
				pointDistance: 0.01, // best between 0.01 and 0.05
				fps: 4,

				backgroundColor: 'transparent',
				fillColor: '#26b1d3',

				path: [
					['arc',size/2, size/2, (size)/2-thickness, 0, 360]
				],

				step: function(point, index, frame) {

					// Here you can do custom stuff.
					// `this._` is a HTML 2d Canvas Context
					
					this._.beginPath();
					this._.moveTo(point.x, point.y);
					this._.arc(
						point.x, point.y,
						thickness, 0,
						Math.PI*2, false
					);
					this._.closePath();
					this._.fill();

				}

			});

			spinner.play();
			element.empty().append(spinner.canvas);

        }
    };
});