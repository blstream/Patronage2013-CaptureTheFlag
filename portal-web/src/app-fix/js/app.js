'use strict';

/* wwwApp Module */

var wwwApp = angular.module('wwwApp', ['wwwApp.services'])
	.config(['$routeProvider', function($routeProvider) {

    // default route
    $routeProvider.when('/', {templateUrl: 'views/main.html', controller: 'MainCtrl'}).otherwise({redirectTo: '/'});
	
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
	    $rootScope.initHeatMap();
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
	
	$rootScope.initHeatMap = function() {
		
		$('body > canvas').remove();
		
		setTimeout(function() {
			!function($) {
				$(function() {

					var heatmap2 = h337.create({
								"element" : document.getElementById("page"),
								"radius" : 40,
								"visible" : false,
								"opacity" : 40
							});
					window.heatmap = heatmap2;

					// realtime heatmap mousemovement
					// handling
					(function() {
						var active = false, 
							lastCoords = [], 
							mouseMove = false, 
							mouseOver = false, 
							activate = function() {
								active = true;
							}, 
							timer = null, simulateEv = function() {
								heatmap2.store.addDataPoint(
											lastCoords[0],
											lastCoords[1]
								);
							}, 
							antiIdle = function() {
								if (mouseOver && !mouseMove && lastCoords && !timer) {
								    timer = setInterval(
										simulateEv,
										1000);
								}
							}, 
							letsgo = function() {
								$("#closer").show();
								heatmap2.toggleDisplay();
							};

							$("#act-heatmap").click(function() {
								letsgo();
								return false;
							});

							if (window.location.hash == "#heat-it") {
								letsgo();
							}

							$("#closer").click(function() {
								$("#closer").hide();
								heatmap2.toggleDisplay();
							})
							
							$("#closer").mousemove(function() {
								mouseOver = false;
								if (timer) {
									clearInterval(timer)
									timer = null;
								}
							});

							(function(fn) {
								setInterval(fn, 1000);
							}(antiIdle));
							var tmp = $("#page");

							tmp.mouseout(function() {
								mouseOver = false;
								if (timer) {
									clearInterval(timer)
									timer = null;
								}
							});

							var collect = function(ev) {
								mouseMove = true;
								mouseOver = true;
								if (active) {
									if (timer) {
										clearInterval(timer);
										timer = null;
									}
									var x = ev.pageX
											- this.offsetLeft;
									var y = ev.pageY
											- this.offsetTop;
									// var pos =
									// h337.util.mousePosition(ev);
	
									heatmap2.store
											.addDataPoint(
													x, y);
									lastCoords = [ x, y ];
	
									active = false;
								}
								mouseMove = false;
							};

							tmp.mousemove(collect).click(collect);

							tmp[0]["ontouchmove"] = function(
									ev) {
								var touch = ev.touches[0],
								// simulating a mousemove
								// event
								simulatedEvent = document
										.createEvent("MouseEvent");
								simulatedEvent
										.initMouseEvent(
												"mousemove",
												true,
												true,
												window,
												1,
												touch.screenX,
												touch.screenY,
												touch.clientX,
												touch.clientY,
												false,
												false,
												false,
												false, 0,
												null);
								// dispatching the simulated event              
								touch.target
										.dispatchEvent(simulatedEvent);
								// we don't want to have the default iphone scrolling behaviour ontouchmove  
								ev.preventDefault();
							};

							(function(fn) {
								setInterval(fn, 50);
							}(activate));
						})();

					});
			}(window.jQuery);

		}, 2000);

	}
});
