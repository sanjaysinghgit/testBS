// AppMonitor class collects browser memory data in response to user behavior over time.
// It may be accessed outside of Angular scopes, therefore it does not follow the standard
// Angular design patterns.

AppMonitor = {
    getDomStats: function (obj, level) {
        if (obj == undefined) {
            AppMonitor.getDomStats.startTime = new Date();
            AppMonitor.getDomStats.stats = { count: 0, levels: 0, nodes: {}, time: 0 };
            AppMonitor.getDomStats(document, 0);
            AppMonitor.getDomStats.stats.time = (new Date() - AppMonitor.getDomStats.startTime) / 1000;
            return AppMonitor.getDomStats.stats;
        }
        var name = obj.tagName ? obj.tagName : obj.nodeName;
        if (!name) return;

        if (AppMonitor.getDomStats.stats.nodes[name])
            AppMonitor.getDomStats.stats.nodes[name]++;
        else
            AppMonitor.getDomStats.stats.nodes[name] = 1;
        AppMonitor.getDomStats.stats.count++;
        AppMonitor.getDomStats.stats.levels = Math.max(AppMonitor.getDomStats.stats.levels, level);

        for (var i in obj.childNodes)
            AppMonitor.getDomStats(obj.childNodes[i], level + 1);
    }
};