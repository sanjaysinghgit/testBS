var FromEmailCtrl = [ '$route', '$location', '$window',
    function ($route, $location, $window)
    {
        //var params          = $location.search(),
        //    developmentId   = params.developmentId,
        //    resourceType    = params.resourceType,
        //    resourceId      = params.resourceId,
        //    // we don't do anything with these yet,
        //    // but we will soon
        //    subResourceType = params.subResourceType,
        //    subResourceId   = params.subResourceID,
        //    // setup base paths
        //    path            = '',
        //    developmentPath = '/developments/'+developmentId,
        //    defaultPath     = '/' //default to the root

        //// if we don't have a development ID, redirect to the 
        //// default path and quit
        //if (!developmentId) {
        //    $location.path(defaultPath);
        //    return;
        //}
        
        //// further refine based on resourceType
        //if (resourceType == 'privateMessage') {
        //    path = developmentPath + '/privateMessages/message/'+resourceId;
        //}
        //else if (resourceType == 'loopMessage') {
        //    path = developmentPath + '/loop/message/'+resourceId;
        //}
        //else if (resourceType == 'invitation') {
        //    path = '/invitation/' + resourceId + '/' + subResourceId + '/';
        //    //We have to use $window
        //    $window.location.href = path;
        //    return;
        //} else {
        //    path = defaultPath;
        //}

        //// execute the redirect
        //$location.path(path);
    }];