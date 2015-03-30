mlm.factory("oDataQueryProvider", [function () {
    return {

        generateQueryString: function (query, queryMap, inclSkipOrderBy) {
            var components = inclSkipOrderBy ? [filter, top, skip, orderBy] : [filter, orderBy];
            var parts = [];
            for (var i = 0; i < components.length; i++) {
                push(parts, components[i], query, queryMap, '','');
            }
            return parts.join("&");
        }

    };

    function filter(query, queryMap) {
        var components = [
            queryText, 
            queryTextTokensStartsWith, 
            tags, 
            filterByField, 
            exclude
        ];
        var parts = [];
        var filter = "";
        for (var i = 0; i < components.length; i++) {
            push(parts, components[i], query, queryMap, '(', ')');
        }
        filter = parts.join(' and ');
        filter.replace(/&/g,'&amp;'); 
        if (filter != "")
            filter = "$filter=(" + filter + ")";
        return filter;
    }

    function exclude(query, queryMap) {
        if (query.exclude === undefined
            || _.isNull(query.exclude)
            || query.exclude.length === 0
            || queryMap == undefined
            || queryMap == null)
            return [];
        
        var queryArr = [];
        if (_.isArray(query.exclude)) {
            _.each(queryMap.exclude, function(e, i) {
                if (query.exclude[i] != null) {
                    _.each(query.exclude[i], function(ex, index) {
                        queryArr.push((e + " ne '{0}'").format(ex));
                    });
                }
            });
        } else {
            queryArr.push((queryMap.exclude + " ne '{0}'").format(query.exclude));
        }
        return queryArr.length > 1 ? queryArr.join(' and ') : queryArr.toString();
    }

    function orderBy(query) {
        return query.sort == null ? 0 : "$orderby=" + query.sort;
    }

    function top(query) {
        return query.top == null ? 0 : "$top=" + query.top;
    }

    function skip(query) {
        if (query.skip) {
            return query.skip == null ? 0 : "$skip=" + query.skip;
        } else{
            return query.afterId == null ? 0 : "afterId=" + query.afterId;
        }
    }

    function filterByField(query, queryMap) {
    	if (_.isArray(query.filter)) {
    		var queryArr = [];
    		if (queryMap != undefined && queryMap != null)
    			_.each(queryMap.filter, function (filter, index) {
    				if (    query.filter[index] != null 
                        &&  query.filter[index] != ''
                        &&  query.filter[index].toLowerCase() != 'all') {
    				    queryArr
                            .push((filter + " eq '{0}'")
                            .format(query.filter[index]));
    				}
    			});
    		var queryStr = queryArr.join(' and ');
    		return queryStr;
    	} else {
    		return query.filter == []
                || query.filter == undefined 
                || query.filter == null 
                || queryMap == undefined 
                || queryMap == null 
                || query.filter.toLowerCase() === 'all' ? 
                        0 : 
                        queryMap.filter + " eq '{0}'".format(query.filter);
    	}
    }

    function queryText(query, queryMap) {
        if (_.isNull(query.queryText)
            || _.isEmpty(query.queryText)
            || queryMap == undefined
            || queryMap == null)
            return 0;

        var queryStr = "";

        _.each(queryMap.textFields, function (q, index) {
            if (index !== queryMap.textFields.length -1) {
                queryStr = queryStr + "substringof('{0}'," + q + " ) or ";
            } else {
                queryStr = queryStr + "substringof('{0}'," + q + " )";
            }
        });
        return queryStr.format(query.queryText);
    }

    function queryTextContains(query, queryMap) {
        if (_.isNull(query)
            || _.isEmpty(query)
            || queryMap == undefined
            || queryMap == null)
            return 0;

        var queryStr = "";

        _.each(queryMap.textFields, function (q, index) {
            if (index !== queryMap.textFields.length -1) {
                queryStr = queryStr + "substringof('{0}'," + q + " ) or ";
            } else {
                queryStr = queryStr + "substringof('{0}'," + q + " )";
            }
        });

        return queryStr.format(query);
    }

    function queryTextStartsWith(query, queryMap) {
        if (_.isNull(query)
            || _.isEmpty(query)
            || queryMap == undefined
            || queryMap == null)
            return 0;

        var queryStr = "";
        _.each(queryMap.textFields, function (q, index) {
            if (index !== queryMap.textFields.length - 1) {
                queryStr = queryStr + "startswith(" + q + ",'{0}') or ";
            } else {
                queryStr = queryStr + "startswith(" + q + ",'{0}')";
            }
        });

        return queryStr.format(query);
    }

    function queryTextEndsWith(query, queryMap) {
        if (_.isNull(query)
            || _.isEmpty(query)
            || queryMap == undefined
            || queryMap == null)
            return 0;

        var queryStr = "";

        _.each(queryMap.textFields, function (q, index) {
            if (index !== queryMap.textFields.length - 1) {
                queryStr = queryStr + "endswith(" + q + ",'{0}') or ";
            } else {
                queryStr = queryStr + "endswith(" + q + ",'{0}')";
            }
        });

        return queryStr.format(query);
    }

    /**
     * Splits the query string into tokens and then 
     * checks to see if each field in the query map either:
     * a) startsWith:   on the first token
     * b) contains:     on additional tokens
     *     
     * FIXME: rename this function so that it more accurately 
     * reflects its behavior
     * 
     * @param  {[type]} query    [description]
     * @param  {[type]} queryMap [description]
     * @return {[type]}          [description]
     */
    function queryTextTokensStartsWith(query, queryMap){

        if (_.isNull(query.queryTextTokensStartsWith)
            || _.isEmpty(query.queryTextTokensStartsWith) 
            || queryMap == undefined 
            || queryMap == null)
            return 0;

        // split the query on certain special characters: 
        // spaces, commas, periods and dashes
        var re = /[\s*]/i;
        // Split only by space.
        var queryTokens = query.queryTextTokensStartsWith.split(re); 
        var queryStr = "";

        _.each(queryTokens, function(token, index) {

            token = token.replace(/'/g, "''").replace(new RegExp("[\*]", "g"), "");

            var tmpStr = index > 0 ? queryTextContains(token, queryMap) : queryTextStartsWith(token, queryMap);

            // if tmpStr is 0 then exit!
            if (!tmpStr) return;

            // if there were previous tokens, we need to and them together
            if (queryStr.length > 0)
                queryStr += ' and ';

            //if it is not the first token in the query
            //lets perform a contains search on it
            //so that we can find results where a single 
            //field contains multiple tokens ie. { firstName: "James Earl" }
            queryStr += '(' + tmpStr + ')';
        });
        return queryStr;
    }

    function tags(query) {
        if (query.tags === undefined || _.isNull(query.tags) || query.tags.length === 0)
            return 0;

        var tagsQuery = 'tags/id eq ';
        var tagIds = [];

        for (i = 0; i < query.tags.length; i++) {
            tagIds.push("'{0}'".format(query.tags[i])); 
        }
        var result = tagIds.length > 1 ? 
                        tagsQuery +=tagIds.join(' or tags/id eq ') : 
                                    tagsQuery += tagIds.toString();
        return result;
    }

    function push(parts, delegate, query, queryMap, prefix, suffix) {
        var result = delegate(query, queryMap);
        if (result != 0)
            parts.push(prefix + result + suffix);
    }
}]);

mlm.factory('oDataQueryMaid',
[ '$q',
function($q) {
  return {
    request: function(config){
      // this undoes the damage caused by Angular's encodeURISegment
      // which replaces all instances of &,= and + in our user entered
      // queries. The one we want to allow is the ampersand.
      config.url = config.url.replace(/('[^']*)&([^']*')/g,'$1%26$2');
      return config || $q.when(config);
    }
  }
}]);

//Http Intercpetor to check auth failures for xhr requests
mlm.config(['$httpProvider',function($httpProvider) {
  $httpProvider.interceptors.push('oDataQueryMaid');
}]);


