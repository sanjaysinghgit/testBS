mlm.controller('agentGTreeCtrl',
    [
    '$scope',
    '$route',
    '$location',
    'cacheManager',
    'agentRepository',
    'agentTreeList',
    function (
        $scope,
        $route,
        $location,
        cacheManager,
        agentRepository,
        agentTreeList
    ) {

        //$scope.context = $route.current.params.typeMode;
        //var ele;
        //if ($scope.context == 'Binary') {
        //    $('#chartBinary').show();
        //    $('#chart').hide();
        //    ele = '#chartBinary';
        //}
        //else {
        //    $('#chartBinary').hide();
        //    $('#chart').show();
        //    ele = '#chart';
        //}
        
        

        var data = [];
        console.log("");
        var agentData = $route.current.locals.agentTree;
        _.each(agentData, function (item) {
            data.push(function () {
                return {
                    name: item.AgentCode,
                    parent: item.IntroducerCode,
                    position: item.Position,
                    status: item.Status,
                };
            }());
        });

        //parent: function () {
        //    if ($scope.context == 'Binary')
        //    { item.SponsorCode }
        //    else {
        //        item.IntroducerCode
        //    }
        //},

        // create a name: node map
        var dataMap = data.reduce(function (map, node) {
            map[node.name] = node;
            return map;
        }, {});

        // create the tree array
        var treeData = [];
        data.forEach(function (node) {
            // add to parent
            var parent = dataMap[node.parent];
            if (parent) {
                // create child array if it doesn't exist
                (parent.children || (parent.children = []))
                    // add node to child array
                    .push(node);
            } else {
                // parent is null or missing
                treeData.push(node);
            }
        });


        



        var margin = { top: 120, right: 120, bottom: 20, left: 120 },
             width = 960 - margin.right - margin.left,
             height = 800 - margin.top - margin.bottom;

        var i = 0,
            duration = 750,
            root;

        root = treeData[0];
        root.x0 = height / 2;
        root.y0 = 0;

        //if (d3.select("#chart").select("svg")) {
        //    update(root);
        //    return;
        //}

        //d3.select("#chart").select("svg").remove()
        
        var tree = d3.layout.tree()
            .size([height, width]);

        var diagonal = d3.svg.diagonal()
            .projection(function (d) { return [d.x, d.y]; });

        var svg = d3.select("#Gchart").append("svg")
            .attr("width", width + margin.right + margin.left)
            .attr("height", height + margin.top + margin.bottom)
          .append("g")
            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

        

        function collapse(d) {
            if (d.children) {
                d._children = d.children;
                d._children.forEach(collapse);
                d.children = null;
            }
        }

        //root.children.forEach(collapse);
        console.log(root);
        //root.children[0].children[0].forEach(collapse);
        //root.children[0].children[1].forEach(collapse);
        //root.children[1].children[0].forEach(collapse);
        //root.children[1].children[1].forEach(collapse);


        update(root);


        d3.select(self.frameElement).style("height", "800px");


        function update(source) {

            // Compute the new tree layout.
            var nodes = tree.nodes(root).reverse(),
                links = tree.links(nodes);

            // Normalize for fixed-depth.
            nodes.forEach(function (d) { d.y = d.depth * 100; });

            // Update the nodes…
            var node = svg.selectAll("g.node")
                .data(nodes, function (d) { return d.id || (d.id = ++i); });

            // Enter any new nodes at the parent's previous position.
            var nodeEnter = node.enter().append("g")
                .attr("class", "node")
                .attr("transform", function (d) { return "translate(" + source.x0 + "," + source.y0 + ")"; })
                .on("click", click);

            nodeEnter.append("circle")
                .attr("r", 1e-6)
                .style("fill", function (d) { return d._children ? "lightsteelblue" : "#fff"; });

            nodeEnter.append("text")
                .attr("y", function (d) { return d.children || d._children ? -15 : 15; })
                .attr("dy", ".35em")
                .attr("text-anchor", "middle")
                .text(function (d) { return d.name; })
                .style("fill-opacity", 1e-6);

            // Transition nodes to their new position.
            var nodeUpdate = node.transition()
                .duration(duration)
                .attr("transform", function (d) { return "translate(" + d.x + "," + d.y + ")"; });

            nodeUpdate.select("circle")
                .attr("r", 4.5)
                .style("fill", function (d) { return d._children ? "lightsteelblue" : "#fff"; });

            nodeUpdate.select("text")
                .style("fill-opacity", 1);

            // Transition exiting nodes to the parent's new position.
            var nodeExit = node.exit().transition()
                .duration(duration)
                .attr("transform", function (d) { return "translate(" + source.x + "," + source.y + ")"; })
                .remove();

            nodeExit.select("circle")
                .attr("r", 1e-6);

            nodeExit.select("text")
                .style("fill-opacity", 1e-6);

            // Update the links…
            var link = svg.selectAll("path.nodelink")
                .data(links, function (d) { return d.target.id; });

            // Enter any new links at the parent's previous position.
            link.enter().insert("path", "g")
                .attr("class", "nodelink")
                .attr("d", function (d) {
                    var o = { x: source.x0, y: source.y0 };
                    return diagonal({ source: o, target: o });
                });

            // Transition links to their new position.
            link.transition()
                .duration(duration)
                .attr("d", diagonal);

            // Transition exiting nodes to the parent's new position.
            link.exit().transition()
                .duration(duration)
                .attr("d", function (d) {
                    var o = { x: source.x, y: source.y };
                    return diagonal({ source: o, target: o });
                })
                .remove();

            // Stash the old positions for transition.
            nodes.forEach(function (d) {
                d.x0 = d.x;
                d.y0 = d.y;
            });
        }

        // Toggle children on click.
        function click(d) {
            if (d.children) {
                d._children = d.children;
                d.children = null;
            } else {
                d.children = d._children;
                d._children = null;
            }
            update(d);
        }


        // var adata = [
        //{ "name": "Level 2: A", "parent": "Top Level" },
        //{ "name": "Top Level", "parent": "null" },
        //{ "name": "Son of A", "parent": "Level 2: A" },
        //{ "name": "Daughter of A", "parent": "Level 2: A" },
        //{ "name": "Level 2: B", "parent": "Top Level" }
        // ];



    }]);
