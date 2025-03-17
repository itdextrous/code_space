(function(app) {
    "use strict";

    app.requires.push("treeGrid");
    app.controller("Naviair.AimTreeViewController", ["$scope", "naviairApiResource", "$location", "$timeout",
        function($scope, naviairApiResource, $location, $timeout) {
            var fullTree = {};
            var takeValue = 50;
            function search(skip, take) {
                skip = skip || 0;
                take = take || takeValue;
                naviairApiResource.getSearch({ criterion: $scope.keywords, skip: skip, take: take })
                    .then(function(response) {
                            if ($scope.keywords !== "") {
                                if (skip === 0) {
                                    $scope.tree_data = response.data.nodes;
                                } else {
                                    $scope.tree_data = $scope.tree_data.concat(response.data.nodes);
                                }
                                if (response.data.total > $scope.tree_data.length) {
                                    $scope.showSeeMore = true;
                                }
                                $("#seemore-spiner").removeClass("naviair-fonts-spin3").removeClass("animate-spin");
                                $("#seemore").prop("disabled", false);
                            }
                            $scope.searching = false;
                        },
                        function(response) {

                        });
            }
            $scope.loading = true;
            $scope.loadMore = function() {
                $("#seemore-spiner").addClass("naviair-fonts-spin3").addClass("animate-spin");
                $("#seemore").prop("disabled", true);
                search($scope.tree_data.length, takeValue);
            }
            $scope.my_tree_handler = function (branch, row) {
              
                if (!branch.isLoaded && branch.isDir && branch.hasChildren) {
                    naviairApiResource.getNodesForParent({ parentId: branch.id })
                        .then(function(response) {
                                Array.prototype.push.apply(branch.children, response.data);
                                branch.isLoaded = true;
                                branch.expanded = !branch.expanded;
                            },
                            function(response) {

                            });
                } else {
                    branch.expanded = !branch.expanded;
                }
            }
            $scope.blobUrl = "https://naviair.blob.core.windows.net/files/";
            $scope.keywords = "";
            $scope.searching = false;
            $scope.showSeeMore = false;
            var promise = null;
            $scope.search = function() {
                $scope.showSeeMore = false;
                if (promise != null) {
                    $timeout.cancel(promise);
                }
                if ($scope.keywords === "") {
                    $scope.tree_data = fullTree;
                    $scope.searching = false;
                    return;
                }
                $scope.searching = true;
                promise = $timeout(search, 300);
            }
            $scope.colsDisplayName = {title:"",name:""};
            $scope.expanding_property = {
                field: "title",
                cellTemplate:
                    "<a ng-if=\"!row.branch.isDir\"  ng-href=\"{{ row.branch.href }}\"  target=\"_blank\" >{{row.branch[expandingProperty.field]}}</a>" +
                        "<span ng-if=\"row.branch.isDir\" >{{row.branch[expandingProperty.field]}}</span>"
            };
            $scope.col_defs = [
                {
                    field: "name",
                    cellTemplate:
                        "<a ng-if=\"!row.branch.isDir\"  ng-href=\"{{ row.branch.href }}\"  target=\"_blank\" >{{ row.branch[col.field] }}</a>" +
                            "<span ng-if=\"row.branch.isDir\" >{{ row.branch[col.field] }}</span>"
                }
            ];
            naviairApiResource.getNodesForParent({ parentId: "" })
                .then(function(response) {
                        $scope.loading = false;
                        fullTree = response.data;
                        $scope.tree_data = response.data;
                    },
                    function(response) {

                    });
            $scope.tree_data = [];
        }]);
})(angular.module("app"));