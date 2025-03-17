(function () {
    "use strict";
    
    angular.module("angulargrid", []).directive("grid", function () {
        var directive = {
            restrict: "E",
            scope: {
                content: "=name"
            },
            templateUrl: "/templates/ng-grid/angular-grid.template.html"
        };
        return directive;
    });
})();