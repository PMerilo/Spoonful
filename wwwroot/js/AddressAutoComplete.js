// AddressAutoComplete.js custom js file

document.addEventListener("DOMContentLoaded", function (event) {
    google.maps.event.addDomListener(window, 'load', initialize);
});


function initialize() {
    var input = document.getElementById('address');
    var autocomplete = new google.maps.places.Autocomplete(input);
}


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