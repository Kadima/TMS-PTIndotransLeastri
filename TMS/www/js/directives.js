var appDirectives = angular.module('EventMob.directives', []);
appDirectives.directive('dateFormat', ['$filter', function ($filter) {
    var dateFilter = $filter('date');
    return {
        require: 'ngModel',
        link: function (scope, element, attrs,ctrl) {
            function formatter(value) {
                return dateFilter(value, "yyyy-MM-dd HH:mm");
            }
            function parser() {
                return ctrl.$modelValue;
            }
            ctrl.$formatters.push(formatter);
            ctrl.$parsers.unshift(parser);
        }
    };
}]);