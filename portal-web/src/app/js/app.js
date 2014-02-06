'use strict';

/* wwwApp Module */

var wwwApp = angular.module('wwwApp', ['wwwApp.services'])
	.config(['$routeProvider', function($routeProvider) {

    // default route
    $routeProvider.when('/', {templateUrl: 'views/main.html', controller: 'MainCtrl'}).otherwise({redirectTo: '/'});

    // register route
    $routeProvider.when('/register', {templateUrl: 'views/register.html', controller: 'MainCtrl'})
	
	// register route
    $routeProvider.when('/games', {templateUrl: 'views/games.html', controller: 'GamesCtrl'})
    $routeProvider.when('/add_game', {templateUrl: 'views/game_add.html', controller: 'MainCtrl'})
    
}]);

wwwApp.factory('localStorage', function(){
	var APP_ID =  'ctf-local-storage';

	// api exposure
	return {
		// return item value
		getB: function(item){
			return localStorage.getItem(item)
		},
		// return item value
		getN: function(item){
			return localStorage.getItem(item)
		},
		// return item value
		get: function(item){
			return localStorage.getItem(item)
		},
		set: function(item, value){
			// set item value
			localStorage.setItem(item, JSON.stringify(value));
		}

	};

});

/*
 *   User Management for main
*/
wwwApp.run(function($rootScope, localStorage, $location){

	$rootScope.location = $location;
	$rootScope.$watch('location.path()', function( path ) {
	    if (path == '/logout') {  
	    	$rootScope.setUserStatus('', false, '');
	    	$location.path('/');
	    }
	});

	$rootScope.token = localStorage.get('token');
	$rootScope.Signed =  localStorage.getB('is-user-signed');
	$rootScope.userName = localStorage.get('userName');

	$rootScope.setUserStatus = function(token, state, userName){
		
		console.log('is-user-signed: ' + state);
		console.log('token: ' + token);
		console.log('userName: ' + userName);
		
		localStorage.set('is-user-signed', state);
		localStorage.set('token', token);
		localStorage.set('userName', userName);
		$rootScope.token = token;
		$rootScope.Signed = state;
		$rootScope.userName = userName;
		
		console.log('$rootScope.token: ' + $rootScope.token);
		console.log('$rootScope.Signed: ' + $rootScope.Signed);
		console.log('$rootScope.userName: ' + $rootScope.userName);
	}
});
