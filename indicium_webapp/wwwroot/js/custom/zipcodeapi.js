// zip code api implementation
var amountused = 0;

$('#zip_code').focusout(function () { 
    var location = $('#zip_code').val().replace(" ", "");
    var patt = new RegExp("^[0-9]{4}\s?[a-zA-Z]{2}$");
    if (patt.test(location) && amountused < 3)
    {
        getAddress(location);
        amountused++;
    } 
});

function getAddress(location) {
    geocoder = new google.maps.Geocoder();
    var city;
    var streetname;
    var country = 'Nederland';
    geocoder.geocode({ 'address': location }, function (results, status) {
        if (status == 'OK') {
            var result = results[0].geometry.location;
            geocoder.geocode({ latLng: results[0].geometry.location }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    // get the streetname:
                    if (results[0].types[0] == 'street_address' && results[0].address_components[1]) {
                        streetname = results[0].address_components[1].long_name;
                    }
                    else if (results[0].types[0] == 'route') {
                        streetname = results[0].address_components[0].long_name;
                    }
                    $('#street').val(streetname);
                }
            });
            results[0].address_components.forEach(function (element2) {
                element2.types.forEach(function (element3) {
                    switch (element3) {
                        case 'locality':
                            city = element2.long_name;
                            break;
                    };
                });
            });
            $('#city').val(city);
            $('#country').val(country);
        }
        else {
            console.log("Geocode was not successful for the following reason: " + status);
        };
    });
};