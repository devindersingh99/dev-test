(function () {
    var app = angular.module('music', ['ui.router']);
    app.config(function ($stateProvider, $urlRouterProvider, $locationProvider) {

        //    $locationProvider.html5Mode(true);

        $urlRouterProvider.otherwise('/');

        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '/app/templates/home.html',
                controller: 'homeController'
            })
            .state('search', {
                url: '/search',
                templateUrl: '/app/templates/search.html',
                controller: 'searchController'
            })
            .state('searchResults', {
                url: '/search/:ser',
                templateUrl: '/app/templates/searchResults.html',
                controller: 'searchResultsController'
            })
            .state('favourite', {
                url: '/fav',
                templateUrl: '/app/templates/favourite.html',
                controller: 'favouriteController'
            })
            .state('newlist', {
                url: '/newlist',
                templateUrl: '/app/templates/newFavouriteList.html',
                controller: 'newFavouriteController'
            })
            .state('release', {
                url: '/releases/:name/:id',
                templateUrl: '/app/templates/releases.html',
                controller: 'releaseController'
            });


    });

    app.service('favouriteService', function ($http) {

        this.getFavourite = function () {
            return $http.get('/favourite/get');
        }

        this.getFavouriteListName = function () {
            return $http.get('/favourite/list');
        }

        this.addToFavourite = function (name, releaseId, title, label) {

            return $http.post('favourite/addrelease/' + name + '/' + releaseId + '/' + title + '/' + label);
        }

        this.removeFromFavourite = function (name, releaseId) {
            return $http.post('favourite/remove/' + name + '/' + releaseId);
        }

        this.removeFavouriteList = function (listName) {
            return $http.delete('favourite/deletelist/' + listName);
        }

        this.createNewList = function (name) {
            return $http.post('favourite/createlist/' + name);
        }

    });

    app.service('searchService', function ($http) {
        this.search = function (name) {
            return $http.get("/artist/search/" + name + '/1/10')
        }
    });

    app.controller('newFavouriteController', function ($scope, $state, favouriteService) {


        $scope.error = false;
        $scope.errorMessage = '';
        $scope.listName = '';

        $scope.createNewList = function () {

            if (!$scope.listName) {
                $scope.error = true;
                $scope.errorMessage = 'Please enter a valid name';
                return;
            }
            $scope.error = false;
            $scope.errorMessage = '';

            favouriteService.createNewList($scope.listName)
            .then(function (res) {
                $state.go('favourite');
            }, function (err) {
                $scope.error = true;
                if (err.status == 409)
                    $scope.errorMessage = 'List with name '+ $scope.listName + ' already exists';
                    else
                $scope.errorMessage = 'There was an error getting data from server. Please try again';
            });
        }


    });

    app.controller('favouriteController', function ($scope, favouriteService, $state) {

        $scope.loading = false;

        $scope.getFavourite = function () {
            $scope.loading = true;
            favouriteService.getFavourite()
            .then(function (res) {
                $scope.favourites = res.data;
                $scope.loading = false;
            }, function (err) {
                $scope.errorMessage = 'There was an error getting data from server. Please try again'
                $scope.loading = false;
            });
        }

        $scope.removeList = function (name) {
            favouriteService.removeFavouriteList(name)
            .then(function (res) {
                $scope.getFavourite();
            }, function (err) {

            });
        }

        $scope.remove = function (id, name) {
            favouriteService.removeFromFavourite(name, id)
            .then(function (res) {
                $scope.getFavourite();
            }, function (err) {

            });
        }

        $scope.createList = function () {
            $state.go('newlist');
        }


        $scope.getFavourite();

    });

    app.controller('homeController', function ($scope, $http) {

    });

    app.controller('releaseController', function ($scope, $http, $stateParams, favouriteService, $state) {

        $scope.id = $stateParams.id;
        $scope.name = $stateParams.name;
        $scope.error = false;
        $scope.loading = false;

        $scope.search = function () {
            $scope.error = false;
            $scope.loading = true;
            $http.get("/artist/" + $scope.id + '/releases')
            .then(function (res) {
                $scope.releases = res.data;
                $scope.loading = false;
            }, function (err) {
                $scope.loading = false;
                $scope.error = true;
                $scope.errorMessage = 'There was an error getting data from server. Please try again';
            });
        }

        $scope.add = function (id, name) {
            var item = $scope.releases.find(function (ele) {
                return ele.releaseId == id;
            });
            favouriteService.addToFavourite(name, item.releaseId, item.title, item.label)
            .then(function (res) {
                var idx = item.inFavourites.indexOf(name);
                if (idx == -1)
                    item.inFavourites.push(name);
            }, function (err) {

            });
        }

        $scope.remove = function (id, name) {
            var item = $scope.releases.find(function (ele) {
                return ele.releaseId == id;
            });
            favouriteService.removeFromFavourite(name, item.releaseId)
            .then(function (res) {
                var idx = item.inFavourites.indexOf(name)
                if (idx > -1)
                    item.inFavourites.splice(idx, 1);

            }, function (err) {

            });
        }

        $scope.action = function (id, idx, name) {
            var itm = $scope.releases[idx];
            var item = itm.inFavourites.find(function (ele) {
                return ele == name;
            });
            if (item != null) {
                $scope.remove(id, name);
            }
            else {
                $scope.add(id, name);
            }
        }

        $scope.buttonText = function (idx, name) {

            var itm = $scope.releases[idx];
            var item = itm.inFavourites.find(function (ele) {
                return ele == name;
            });
            if (item != null)
                return 'Remove from ' + name;
            else
                return 'Add to ' + name;

        }



        $scope.favouriteList = [];
        $scope.getFavouriteList = function () {
            favouriteService.getFavouriteListName()
            .then(function (res) {
                $scope.favouriteList = res.data;
            }, function (err) {

            });
        }

        $scope.missing = function (arr2) {
            return $scope.favouriteList.filter(function (x) {
                return arr2.indexOf(x) < 0;
            });
        };



        if ($scope.id) {
            $scope.search();
            $scope.getFavouriteList();
        }


    });

    app.controller('searchResultsController', function ($scope, $stateParams, searchService) {

        $scope.searchText = $stateParams.ser;

        this.search = function () {
            $scope.error = false;
            $scope.loading = true;
            searchService.search($scope.searchText)
            .then(function (res) {
                $scope.loading = false;
                $scope.artists = res.data;
                $scope.hasData = $scope.artists.results.length > 0;
            }, function (err) {
                $scope.loading = false;
                $scope.error = true;
                $scope.errorMessage = 'There was an error getting data from server. Please try again';
            });
        }


        if ($scope.searchText) {
            this.search();
        }

    });

    app.controller('searchController', function ($scope, $http, $state) {

        $scope.searchText = '';

        $scope.errorMessage = '';
        $scope.error = false;
        $scope.submitText = 'Search'
        $scope.submitDisabled = false;

        $scope.search = function () {

            $scope.errorMessage = '';
            $scope.error = false;


            if (!$scope.searchText) {
                $scope.errorMessage = 'Please enter artist name to search';
                $scope.error = true;
                return;
            }

            $state.go('searchResults', { ser: $scope.searchText });

        };

        $scope.hasError = function () {
            return $scope.error;
        }


        $scope.$watch('searchText', function (newValue, oldValue) {
            if (newValue) {
                $scope.errorMessage = '';
                $scope.error = false;
            }
        });
    });
}())


