'use strict';

/* Controllers */

wwwApp.controller('PlayersCtrl', function($scope, $rootScope) {


});


wwwApp.controller('MainCtrl', function($scope, $http, $templateCache, $location, $timeout) {
	
	// global config
	$scope.server_host = 'http://10.4.35.1:8080/demo';
	
	$scope.login_url = $scope.server_host + '/oauth/token';
	$scope.register_url = $scope.server_host + '/api/players/add';
	
	$scope.client_id = 'web_www';
	$scope.client_secret = 'secret';
	$scope.grant_type = 'password';
	
	$scope.SignIn = function() {
		$.ajax({
			type : "POST",
			beforeSend : function(request) {
				request.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
			},
			url : $scope.login_url,
			data : {
				client_id : $scope.client_id,
				client_secret : $scope.client_secret,
				grant_type : $scope.grant_type,
				username : $scope.username,
				password : $scope.password
			},
			success : function(authData) {
				var data = authData;
				//console.log(authData)
				//$scope.access_token = data.access_token;
				//console.log($scope.access_token)
				$scope.setUserStatus( data.access_token, true, $scope.username.replace(/['"]/g,'') );	
		    	$timeout(function(e){$location.path('/')}, 100)
				
			},
			error : function(errorData) {
				var response = errorData.responseText;
				var error = $.parseJSON(response);
				var reason = error.error_description;
				alert("Error: " + reason);
			}
		});
	}
	
	$scope.RegisterUser = function() {
		$.ajax({
			type : "POST",
			beforeSend : function(request) {
				request.setRequestHeader("Content-type", "application/json");
				request.setRequestHeader("Accept", "application/json");
			},
			url : $scope.register_url,
			data : JSON.stringify({
				username:$scope.r_username,
				password:$scope.r_password
			}),
			success : function(authData) {
				$scope.username = $scope.r_username;
				$scope.password = $scope.r_password;
				$scope.SignIn();
			},
			error : function(errorData) {
				alert(errorData)
			}
		});
		
	}

});

wwwApp.controller('GamesCtrl', function($scope, $http, $templateCache, $location, $timeout, $rootScope) {
	
	$scope.allGames = '';
	$scope.myGames = '';
	$scope.newGames = '';
	
	$scope.games_url = $scope.server_host + '/api/secured/games';
	
	$scope.init = function() {
		
		if($rootScope.token != '') {
			// GET all games
			$.ajax({
				type : "GET",
				beforeSend : function(request) {
					request.setRequestHeader("Accept", "application/json");
		            request.setRequestHeader("Content-type", "application/json");
		            request.setRequestHeader("Authorization", "Bearer " + $rootScope.token.replace(/['"]/g,''));
				},
				url : $scope.games_url,
				success : function(data) {
					//console.log(data.games);
					$scope.allGames = data.games;
					if (!$scope.$$phase) {
	                    $scope.$apply();
	                } 
				},
				error : function(errorData) {
					var response = errorData.responseText;
					var error = $.parseJSON(response);
					var reason = error.error_description;
					alert("Error: " + reason);
				}
			});
			
			// GET my games only
			$.ajax({
				type : "GET",
				beforeSend : function(request) {
					request.setRequestHeader("Accept", "application/json");
		            request.setRequestHeader("Content-type", "application/json");
		            request.setRequestHeader("Authorization", "Bearer " + $rootScope.token.replace(/['"]/g,''));
				},
				url : $scope.games_url+'?myGamesOnly=true',
				success : function(data) {
					//console.log(data.games);
					$scope.myGames = data.games;
					if (!$scope.$$phase) {
	                    $scope.$apply();
	                } 
				},
				error : function(errorData) {
					var response = errorData.responseText;
					var error = $.parseJSON(response);
					var reason = error.error_description;
					alert("Error: " + reason);
				}
			});
			
			// GET new games
			$.ajax({
				type : "GET",
				beforeSend : function(request) {
					request.setRequestHeader("Accept", "application/json");
		            request.setRequestHeader("Content-type", "application/json");
		            request.setRequestHeader("Authorization", "Bearer " + $rootScope.token.replace(/['"]/g,''));
				},
				url : $scope.games_url+'?status=NEW',
				success : function(data) {
					//console.log(data.games);
					$scope.newGames = data.games;
					if (!$scope.$$phase) {
	                    $scope.$apply();
	                } 
				},
				error : function(errorData) {
					var response = errorData.responseText;
					var error = $.parseJSON(response);
					var reason = error.error_description;
					alert("Error: " + reason);
				}
			});
		}
		
	}
	$scope.AddGame = function() {
			
		$.ajax({
			type : "POST",
			beforeSend : function(request) {
				request.setRequestHeader("Accept", "application/json");
	            request.setRequestHeader("Content-type", "application/json");
	            request.setRequestHeader("Authorization", "Bearer " + $rootScope.token.replace(/['"]/g,''));
			},
			url : $scope.games_url,
			data : JSON.stringify({
				name: $scope.game_name,
				description: $scope.game_description,
				time_start: $('#game_time_start').val(),
				duration: parseInt($('#game_duration').val()),
				points_max: $scope.game_points_max,
				players_max: $scope.game_players_max,
				localization: {
					name: $('#game_localization_name').val(),
					latLng: $('#game_localization_latLng').val().replace('(','').replace(')','').split(","),
					radius: $scope.game_localization_radius
				},
				red_team_base: {
					name: $scope.game_red_team_base_name,
					latLng: $('#game_red_team_base_latLng').val().replace('(','').replace(')','').split(",")
				},
					blue_team_base: {
					name: $scope.game_blue_team_base_name,
					latLng: $('#game_blue_team_base_latLng').val().replace('(','').replace(')','').split(",")
				}
			}),
			success : function(data) {
				//console.log(data)
				$timeout(function(e){$location.path('/games')}, 100)
			},
			error : function(errorData) {
				var response = errorData.responseText;
				var error = $.parseJSON(response);
				var reason = error.error_description;
				alert("Error: " + reason);
			}
		});
	}
	

	$scope.initMap = function() {

		$scope.isset = false;
		$scope.markers = [];
		$scope.redMarker = [];
        $scope.blueMarker = [];
		$scope.circle  = null;
		$scope.map = null;
		$scope.dst = document.getElementById('map-canvas');
        
		if (typeof $scope.dst !== 'object') {
			throw "ERROR:\tincorrect destination <div>";
		}
		 
		$scope.config = {
			start: new google.maps.LatLng(53.43, 14.56),
	        zoom: 13,
	        maxMarkers : $scope.game_points_max,
	        circleRadius : $scope.game_localization_radius,
	        me : this
        };
		
		$scope.rand = function() {
	        if ($scope.isset) {
	            var k = 111196.672; // 1 stopień miary kątowej to 111.196,672m [równik]
	            var alfa = Math.random() * 360;
	            var r = Math.random() *  $scope.game_localization_radius
	     
	            var a = r * Math.cos(alfa) / k;
	            var b = r * Math.sin(alfa) / k;
	     
	            var x0 = $scope.circle.getCenter().lng();
	            var y0 = $scope.circle.getCenter().lat();
	     
	            return (new google.maps.LatLng(y0 + b, x0 + a));
	        }
	    }
		
		$scope.placeMarker = function(location, type) {
			if($scope.isset && type != null) {

				if(type == 'blue') {
					var marker = new google.maps.Marker({
	                    position: location,
	                    draggable : true,
	                    animation : google.maps.Animation.DROP,
	                    map: $scope.map,
	                    icon: 'images/blueIcon.png'
					});
					
					marker.previousPosition = marker.getPosition();
	                marker.positionListener = google.maps.event.addListener(
	                        marker, 
	                        'dragend', 
	                        function (event) {
	                                var t = google.maps.geometry.spherical.computeDistanceBetween(event.latLng, $scope.circle.getCenter());
	                                if ($scope.game_localization_radius >= t) {
	                                        marker.previousPosition = event.latLng;
	                                        $('#game_blue_team_base_latLng').val(event.latLng);
	                                } else {
	                                        marker.setPosition(marker.previousPosition);
	                                }
	                        }
	                );
	                $('#game_blue_team_base_latLng').val(location);
					$scope.blueMarker.push(marker);
				} else {
					var marker = new google.maps.Marker({
	                    position: location,
	                    draggable : true,
	                    animation : google.maps.Animation.DROP,
	                    map: $scope.map,
	                    icon: 'images/redIcon.png'
					});
					
					marker.previousPosition = marker.getPosition();
	                marker.positionListener = google.maps.event.addListener(
	                        marker, 
	                        'dragend', 
	                        function (event) {
	                                var t = google.maps.geometry.spherical.computeDistanceBetween(event.latLng, $scope.circle.getCenter());
	                                if ($scope.game_localization_radius >= t) {
	                                	marker.previousPosition = event.latLng;
	                                	$('#game_red_team_base_latLng').val(event.latLng);
	                                } else {
	                                    marker.setPosition(marker.previousPosition);
	                                }
	                        }
	                );
	                $('#game_red_team_base_latLng').val(location);
					$scope.redMarker.push(marker);
				}
				
			} else if ($scope.isset && $scope.markers.length < $scope.game_points_max && type == null) {
                    var marker = new google.maps.Marker({
                            position: location,
                            draggable : true,
                            animation : google.maps.Animation.DROP,
                            map: $scope.map,
                            icon: 'images/flagIcon.png'
                    });
                    marker.previousPosition = marker.getPosition();
                    marker.positionListener = google.maps.event.addListener(
                            marker, 
                            'dragend', 
                            function (event) {
                                    var t = google.maps.geometry.spherical.computeDistanceBetween(event.latLng, $scope.circle.getCenter());
                                    if ($scope.game_localization_radius >= t) {
                                            marker.previousPosition = event.latLng;
                                    } else {
                                            marker.setPosition(marker.previousPosition);
                                    }
                            }
                    );
                    $scope.markers.push(marker);
            }
		}
		
		$scope.placeMarkers = function () {
	        if ($scope.isset) {
	        	$scope.config.placeListener = google.maps.event.addListener(
	        		$scope.circle, 'click', function (event) {
	        			$scope.placeMarker(event.latLng, null);
	                    if ($scope.markers.length === $scope.game_points_max) {
	                        google.maps.event.removeListener($scope.config.placeListener);
	                        $scope.config.placeListener = null;
	                    }
	                }
	            );
	        }
	    };
	    
	    $scope.placeMarkersRandom = function () {
	    	//console.log($scope.game_points_max)
	    	if(typeof $scope.game_points_max == 'undefined' || $scope.game_points_max == '' || $scope.game_points_max == '0') {
	    		alert('Points max empty');
	    		return false;
	    	}
	        if ($scope.isset && $scope.markers.length < $scope.game_points_max) {
	            var i;
	            for (i = $scope.markers.length; i < $scope.game_points_max; i++) {
	            	$scope.placeMarker($scope.rand(), null);
	            }
	            google.maps.event.removeListener($scope.config.placeListener);
	            $scope.config.placeListener = null;
	        }
	    };
	        
	    $scope.clearMarkers = function () {
	            if ($scope.markers.length > 0) {
	                if (confirm("Remove markers??")) {
	                    var i;
	                    for (i in $scope.markers) {
	                    	$scope.markers[i].setMap(null);
	                        google.maps.event.removeListener($scope.markers[i].positionListener);
	                        $scope.markers[i] = null;
	                    }
	                    $scope.markers = [];
	                    $scope.redMarker = [];
	                    $scope.blueMarker = [];
	                }
	            }
	    };
	    
	    $scope.geocode = function(latLng) {
	    	$scope.geocoder = new google.maps.Geocoder();
	    	$scope.geocoder.geocode({'latLng' : latLng}, function(results, status) {
				if (status == google.maps.GeocoderStatus.OK) {
					if (results[1]) {
						$('#game_localization_name').val(results[1].formatted_address);
					} else {
						$('#game_localization_name').val('No results found');
					}
				} else {
					$('#game_localization_name').val('Geocoder failed due to: ' + status);
				}
			});
	    }
		
		$scope.drawCircle = function() {
			if(typeof $scope.game_localization_radius == 'undefined' || $scope.game_localization_radius == '' || $scope.game_localization_radius == '0') {
	    		alert('Circle radius empty');
	    		return false;
	    	}
			
			if (!$scope.isset) {
				$scope.config.drawListener = google.maps.event.addListener(
						$scope.map, 'click', function (event) {
							$scope.circle = new google.maps.Circle(
	                                {
										strokeColor: "#FF0000",
										strokeOpacity: 0.7,
										strokeWeight: 2,
										fillColor: "#FF0000",
										fillOpacity: 0.1,
										map: $scope.map,
										center: event.latLng,
										radius: parseInt($scope.game_localization_radius)
	                                }
                                );
							    $('#game_localization_latLng').val(event.latLng);
							    $scope.geocode(event.latLng);
                                $scope.isset = true;
                                $scope.map.fitBounds($scope.circle.getBounds());
                                google.maps.event.removeListener($scope.config.drawListener);
                                $scope.config.drawListener = null;
                });
			}
		}
		
		$scope.setBlueTeamBase = function() {
			if ($scope.isset) {
				$scope.config.placeListener = google.maps.event.addListener(
	        		$scope.circle, 'click', function (event) {
	        			$scope.placeMarker(event.latLng, 'blue');
	        			//console.log('blue : '+$scope.blueMarker.length)
	                    if ($scope.blueMarker.length === 1) {
	                        google.maps.event.removeListener($scope.config.placeListener);
	                        $scope.config.placeListener = null;
	                    }
	                }
	            );
			}
		}
		
		$scope.setRedTeamBase = function() {
			if ($scope.isset) {
				$scope.config.placeListener = google.maps.event.addListener(
	        		$scope.circle, 'click', function (event) {
	        			$scope.placeMarker(event.latLng, 'red');
	        			//console.log('red : '+$scope.redMarker.length)
	                    if ($scope.redMarker.length === 1) {
	                        google.maps.event.removeListener($scope.config.placeListener);
	                        $scope.config.placeListener = null;
	                    }
	                }
	            );
			}
		}
		
		$scope.reset = function () {
			if ($scope.isset) {
		        if (confirm("Remove circle?")) {
			        if ($scope.markers.length > 0) {
			            var i;
			            for (i in $scope.markers) {
			            	$scope.markers[i].setMap(null);
			                google.maps.event.removeListener($scope.markers[i].positionListener);
			                $scope.markers[i] = null;
			            }
			            $scope.markers = [];
			        }
			        $scope.circle.setMap(null);
			        $scope.circle = null;
			        $scope.isset = false;
			    }
			}
		};
		
		/*$scope.arenaPlaceMarkers = function() {
            var option = document.getElementsByName("placeMarkers");
            // if true = auto
            if (option[0].checked) {
            	$scope.placeMarkersRandom();
            } else {
                // else manual
            	$scope.placeMarkers();
            }
       }*/
		
		$scope.initialize = function() {
			
			$scope.map = new google.maps.Map($scope.dst, {
				//center: $scope.config.start,
	            zoom: $scope.config.zoom,        
	            mapTypeId: google.maps.MapTypeId.HYBRID
			});

			// Try HTML5 geolocation
			if (navigator.geolocation) {
				navigator.geolocation
						.getCurrentPosition(
								function(position) {
									var pos = new google.maps.LatLng(
											position.coords.latitude,
											position.coords.longitude);

									var infowindow = new google.maps.InfoWindow(
											{
												map : $scope.map,
												position : pos
											});

									$scope.map.setCenter(pos);
								}, function() {
									handleNoGeolocation(true);
								});
			} else {
				// Browser doesn't support Geolocation
				$scope.handleNoGeolocation(false);
			}
		}

		$scope.handleNoGeolocation = function(errorFlag) {
			if (errorFlag) {
				var content = 'Error: The Geolocation service failed.';
			} else {
				var content = 'Error: Your browser doesn\'t support geolocation.';
			}

			var options = {
				map : $scope.map,
				position : $scope.config.start,
				content : content
			};

			var infowindow = new google.maps.InfoWindow(options);
			$scope.map.setCenter(options.position);
		}

		// init map
		$scope.initialize();

	}

});
