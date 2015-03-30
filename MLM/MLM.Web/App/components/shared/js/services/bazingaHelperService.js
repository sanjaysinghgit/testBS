mlm.service('bazingaHelper', ['$resource', '$q', 'bLog',
    function ($resource, $q, bLog) {
        var countryResource = $resource(Url.resolve('countries'), {
            get: { method: 'GET', isArray: false }
        });
        var stateResource = $resource(Url.resolve('provinceorstate/:countryId'), {countryId: '@Id' }, {
            get: { method: 'GET', isArray: false }
           
        });

        return {
            getStateByCountry: function (countryId) {
                var deferred = $q.defer();
                stateResource.get({ countryId:countryId }, function (Data) {
                    deferred.resolve(Data);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
                return deferred.promise;
            },
            getCountries: function () {
                var deferred = $q.defer();
                countryResource.get(function (Data) {
                    deferred.resolve(Data);
                }, function (error) {
                    deferred.reject(error);
                    bLog.serverError(error, error);
                });
                return deferred.promise;


                //var countries = [{ "countryId": 1, "CountryName": "Canada" }];
                //return countries;
            }

        };
    }]);