mlm.factory("paginator", 
    ['$location', 'localization', 
    function($location, localization) {

    return function (
        label, 
        service, 
        query, 
        queryMap, 
        contextId, 
        sortOptions,
        onListUpdate
    ) {
        
        // defaults
        var p = {
            // arguments
            label:              label || "items",
            service:            service,
            query:              query || {},
            queryMap:           queryMap || {},
            contextId:          contextId,
            sortOptions:        sortOptions,
            onListUpdate:       onListUpdate || angular.noop,
            // contents
            rawRecords:         {},
            items:              [],
            // search state 
            searchType:         '',
            searchMsg:          '',
            // meta data
            currentPage:        0, 
            totalPages:         0,
            firstRecord:        0,
            lastRecord:         0,
            totalRecords:       0,
            // state flags
            searching:          false,
            atStart:            false,
            atEnd:              false,
            empty:              false,
            fresh:              true,
            // methods 
            refreshList:        refreshList,
            nextPage:           nextPage,
            prevPage:           prevPage,
            goToPage:           goToPage,
            filterByText:       filterByText,
            sort:               sort,
            _updatePagingData:  _updatePagingData,
            _composeSearchMsg:  _composeSearchMsg,
            _updatePushState:   _updatePushState
        }

        /////////////////////////////////////////////////
        //// "Public" Methods
        /////////////////////////////////////////////////

        /**
         * Call this any time you want to update the list of items in
         * the paginator. Also calls necessary functions to ensure that 
         * state variables are maintained once the items are returned.
         */
        function refreshList() {

            // update the searching state
            p.searching = true;
            p._composeSearchMsg();
            p._updatePushState();

            p.service
                .getList(p.contextId, p.query, p.queryMap)
                .then(function (records) {
                    // assign the new records
                    p.rawRecords = records;
                    p.items      = records.items;
                    // the list is no longer fresh
                    p.fresh      = false;
                    // set the empty flag
                    p.empty      = p.items.length < 1;
                    // update the list data
                    p._updatePagingData();
                    // execute the callback
                    p.onListUpdate();
                    // reset the searching flag
                    p.searching  = false;
                });
        }

        /**
         * Gets the next page of items, and enforces bounds
         */
        function nextPage() {
            if (!p.atEnd) {
                p.query.skip+=p.query.top;
                p.searchType = 'nextPage';
            }
            
        }

        /**
         * Gets the previous page of items, and enforces bounds
         */
        function prevPage() {
            if (!p.atStart) {
                p.query.skip-=p.query.top;
                p.searchType = 'prevPage';
            }
        }

        /**
         * Gets a specific page of items, and enforces bounds
         */
        function goToPage(page) {
            if (page > 0 && page <= p.totalPages) {
                p.query.skip = (page-1)*p.query.top;
                p.searchType = 'updatePage';
            }
        }

        /**
         * Gets a specific page of items, and enforces bounds
         */
        function filterByText() { 
            p.queryText = p.query.queryTextTokensStartsWith || p.query.queryText;

            if (p.queryText) {
                p.searchType = 'text';
            } else {
                p.searchType = null;
            }

            p.query.skip = 0;
            p.refreshList();
        }

        function sort() {
            p.activeSort = _.find(p.sortOptions, { 
                field: p.query.sort 
            });
            if (p.activeSort) {
                p.query.skip = 0;
                p.searchType = 'sort';
                p.refreshList();
            }
        }

        /////////////////////////////////////////////////
        //// "Private" Methods
        /////////////////////////////////////////////////

        function _updatePagingData() {

            p.currentPage   = Math.ceil((p.query.skip+p.query.top)/ p.query.top);
            p.perPage       = p.query.top;

            p.totalRecords  = p.rawRecords.totalItems;
            p.totalPages    = Math.ceil(p.rawRecords.totalItems / p.query.top);

            p.firstRecord   = p.query.skip+1;
            var calcLast    = p.currentPage * p.perPage;
            p.lastRecord    = (calcLast > p.totalRecords) ? p.totalRecords : calcLast;

            // catch lower limit
            if (p.currentPage <= 1) {
                p.currentPage = 1;
                p.atStart = true;
            } else {
                p.atStart = false;
            }

            // catch upper limit
            if (p.currentPage >= p.totalPages) {
                p.currentPage = p.totalPages;
                p.atEnd = true;
            } else {
                p.atEnd = false;
            }

            if (p.totalRecords == 0) {
                p.firstRecord = 0;
            }
        }

        function _composeSearchMsg() {

            switch(p.searchType) {
                case 'sort':
                    
                    // assign its name as the sort label
                    p.searchMsg = 
                        localization.t('pagination.refreshing.by_sorting')
                            .format(p.label, p.activeSort.name)
                    break;
                case 'text':
                    p.searchMsg = 
                        localization.t('pagination.refreshing.by_text')
                            .format(p.label, p.queryText)
                    break;
                case 'prevPage':
                    p.searchMsg = 
                        localization.t('pagination.refreshing.prev_page')
                    break;
                case 'nextPage':
                    p.searchMsg = 
                        localization.t('pagination.refreshing.next_page')
                    break;
                default:
                    p.searchMsg =
                        localization.t('pagination.refreshing.default')
                            .format(p.label)
                    break;
            }

        }

        function _updatePushState() {

            _.each(_.pairs(p.query), function (param) {
                if (param[1] != null) {
                    $location.search(param[0], param[1].toString()).replace();
                }
            });

            p.encodedUrlState = encodeURIComponent($location.url());

        }
        
        return p;
    };

    return paginator; 
}]);