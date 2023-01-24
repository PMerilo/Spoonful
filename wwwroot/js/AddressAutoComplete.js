// AddressAutoComplete.js custom js file

document.addEventListener("DOMContentLoaded", function (event) {
    google.maps.event.addDomListener(window, 'load', initialize);
});


function initialize() {
    var input = document.getElementById('address');
    var autocomplete = new google.maps.places.Autocomplete(input);
}