// AddressAutoComplete.js custom js file

document.addEventListener("DOMContentLoaded", function (event) {
    google.maps.event.addDomListener(window, 'load', initialize);
});


//function initialize() {
//    var input = document.getElementById('address');
//    var autocomplete = new google.maps.places.Autocomplete(input);
//}


function ValidateAddress(elem) {
    var address = elem.value
    console.log(address)
    let button = document.querySelector("#submitBtn")
    console.log(button)
    button.disabled = true;
    if (address != null) {

        $.ajax({
            type: "POST",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val())
            },
            dataType: "json",
            data: { address: address },
            url: "/user/MealKitSubscription/Order?handler=ValidateAddress",
            success: function (res) {
                console.log(res)
                if (res.status == "no code") {
                    button.disabled = true;
                    $("#addressval").text(res.msg)
                    $("#addressval").css("display", "")
                } else if (res.status == "failed") {
                    button.disabled = true;
                    $("#addressval").text(res.msg)
                    $("#addressval").css("display", "")
                } else if (res.status == "valid") {
                    console.log(address)
                    var bool = /[0-9]{6,6}$/.test(address)
                    console.log(bool)
                    if (bool == false) {
                        var newAddress = address + ", " + res.code
                        $("#address").val(newAddress)
                    }
                    button.disabled = false;
                    $("#addressval").text(res.msg)
                    $("#addressval").css("display", "none")
                }
            }
        })
    }
}

function initMap() {
    var map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: -33.8688, lng: 151.2195 },
        zoom: 13
    });
    var input = document.getElementById('address');


    var autocomplete = new google.maps.places.Autocomplete(input);
    autocomplete.bindTo('bounds', map);

    var infowindow = new google.maps.InfoWindow();
    var marker = new google.maps.Marker({
        map: map,
        anchorPoint: new google.maps.Point(0, -29)
    });

    autocomplete.addListener('place_changed', function () {
        infowindow.close();
        marker.setVisible(false);
        var place = autocomplete.getPlace();
        if (!place.geometry) {
            window.alert("Autocomplete's returned place contains no geometry");
            return;
        }

        // If the place has a geometry, then present it on a map.
        if (place.geometry.viewport) {
            map.fitBounds(place.geometry.viewport);
        } else {
            map.setCenter(place.geometry.location);
            map.setZoom(17);
        }
        marker.setIcon(({
            url: place.icon,
            size: new google.maps.Size(71, 71),
            origin: new google.maps.Point(0, 0),
            anchor: new google.maps.Point(17, 34),
            scaledSize: new google.maps.Size(35, 35)
        }));
        marker.setPosition(place.geometry.location);
        marker.setVisible(true);

        var address = '';
        if (place.address_components) {
            address = [
                (place.address_components[0] && place.address_components[0].short_name || ''),
                (place.address_components[1] && place.address_components[1].short_name || ''),
                (place.address_components[2] && place.address_components[2].short_name || '')
            ].join(' ');
        }

        infowindow.setContent('<div><strong>' + place.name + '</strong><br>' + address);
        infowindow.open(map, marker);

        // Location details
        //for (var i = 0; i < place.address_components.length; i++) {
        //    if (place.address_components[i].types[0] == 'postal_code') {
        //        document.getElementById('postal_code').innerHTML = place.address_components[i].long_name;
        //    }
        //    if (place.address_components[i].types[0] == 'country') {
        //        document.getElementById('country').innerHTML = place.address_components[i].long_name;
        //    }
        //}
        document.getElementById('location').innerHTML = place.formatted_address;
        document.getElementById('lat').innerHTML = place.geometry.location.lat();
        document.getElementById('lon').innerHTML = place.geometry.location.lng();
    });
}