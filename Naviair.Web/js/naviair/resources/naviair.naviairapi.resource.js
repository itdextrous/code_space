(function(app) {
    "use strict";

    app.factory("naviairApiResource",["$http", function($http) {
            return {
                getSearch: function(data) {
                    return $http({
                        method: "GET",
                        url: "/umbraco/api/naviairapi/getsearch",
                        params: data
                    });
                },
                getNodesForParent: function(data) {
                    return $http({
                        method: "GET",
                        url: "/umbraco/api/naviairapi/getnodesforparent",
                        params: data
                    });
                }
            }
        }]);

})(angular.module("app"));