(function() {
	'use strict';
	var directiveId = 'ngMatch';
	wwwApp.directive(directiveId, [ '$parse', function($parse) {

		var directive = {
			link : link,
			restrict : 'A',
			require : '?ngModel'
		};
		return directive;

		function link(scope, elem, attrs, ctrl) {
			// if ngModel is not defined, we don't need to do anything
			if (!ctrl)
				return;
			if (!attrs[directiveId])
				return;

			var firstPassword = $parse(attrs[directiveId]);

			var validator = function(value) {
				var temp = firstPassword(scope), v = value === temp;
				ctrl.$setValidity('match', v);
				return value;
			}

			ctrl.$parsers.unshift(validator);
			ctrl.$formatters.push(validator);
			attrs.$observe(directiveId, function() {
				validator(ctrl.$viewValue);
			});

		}
	} ]);
})();




/*'use strict';
angular.module('wwwApp', ['wwwApp.directives']);
 Controllers 
function stageController($scope) {
    $scope.r_password = 'password';
}
 Directives 
angular.module('myApp.directives', [])
    .directive('ngMatch', [function () {
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            var firstPassword = '#' + attrs.ngMatch;
            elem.add(firstPassword).on('keyup', function () {
                scope.$apply(function () {
                    // console.info(elem.val() === $(firstPassword).val());
                    ctrl.$setValidity('match', elem.val() === $(firstPassword).val());
                });
            });
        }
    }
}]);*/

