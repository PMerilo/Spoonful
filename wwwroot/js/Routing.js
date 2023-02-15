$('#createRoutes').click(function () {
    var route = "Central"
    console.log(route)
    $.ajax({
        type: "POST",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val())
        },
        dataType: "json",
        data: { route: route },
        url: "/Admin/DeliveryManagement/?handler=CreateDeliveryRoutes",
        success: function (data) {
            console.log(data)
            console.log("Routes created")
        }
    })
    location.reload();
})

function calcRoute(elem) {
    var route = elem.value
    var mappy = document.getElementById("map");
    console.log(route)
    $.ajax({
        type: "POST",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val())
        },
        dataType: "json",
        data: { route: route },
        url: "/Admin/DeliveryManagement/DeliveryRouting?handler=RetrieveRoutes",
        success: function (data) {
            console.log(data.list)
            var list = data.list
            var last = list[list.length - 1] + ", singapore"
            list.pop();
            console.log(last)
            console.log(list)
            const directionsService = new google.maps.DirectionsService();
            const directionsRenderer = new google.maps.DirectionsRenderer();
            const map = new google.maps.Map(document.getElementById("map"), {
                zoom: 11,
                center: { lat: 1.3742002339365351, lng: 103.81540409330088 },
            });
            directionsRenderer.setMap(map);

            const waypts = [];

            for (let i = 0; i < list.length; i++) {
                waypts.push({
                    location: list[i] + ", singapore",
                    stopover: true,
                });
            }

            console.log(waypts)

            directionsService
                .route({
                    origin: "768442, singapore",
                    destination: last,
                    waypoints: waypts,
                    optimizeWaypoints: true,
                    travelMode: google.maps.TravelMode.DRIVING,
                })
                .then((response) => {
                    directionsRenderer.setDirections(response);
                    console.log(response)
                    console.log(response.routes[0].legs)
                })
                .catch((e) => window.alert("Directions request failed due to " + status));
            mappy.scrollIntoView({ behavior: "smooth", block: "end", inline: "nearest" });
        }
    })
}


$('#calcRoute').click(function () {
    /*var route = "West"*/
    var route = $("#calcRoute").val()
    console.log(route)
    //$.ajax({
    //    type: "POST",
    //    beforeSend: function (xhr) {
    //        xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val())
    //    },
    //    dataType: "json",
    //    data: { route: route },
    //    url: "/Admin/DeliveryManagement/DeliveryRouting?handler=RetrieveRoutes",
    //    success: function (data) {
    //        console.log(data.list)
    //        var list = data.list
    //        var last = list[list.length - 1] + ", singapore"
    //        list.pop();
    //        console.log(last)
    //        console.log(list)
    //        const directionsService = new google.maps.DirectionsService();
    //        const directionsRenderer = new google.maps.DirectionsRenderer();
    //        const map = new google.maps.Map(document.getElementById("map"), {
    //            zoom: 11,
    //            center: { lat: 1.3742002339365351, lng: 103.81540409330088 },
    //        });
    //        directionsRenderer.setMap(map);

    //        const waypts = [];

    //        for (let i = 0; i < list.length; i++) {
    //            waypts.push({
    //                location: list[i] + ", singapore",
    //                stopover: true,
    //            });
    //        }

    //        console.log(waypts)

    //        directionsService
    //            .route({
    //                origin: "768442, singapore",
    //                destination: last,
    //                waypoints: waypts,
    //                optimizeWaypoints: true,
    //                travelMode: google.maps.TravelMode.DRIVING,
    //            })
    //            .then((response) => {
    //                directionsRenderer.setDirections(response);
    //            })
    //            .catch((e) => window.alert("Directions request failed due to " + status));
    //    }
    //})
})


function initMap() {
    const directionsService = new google.maps.DirectionsService();
    const directionsRenderer = new google.maps.DirectionsRenderer();
    const map = new google.maps.Map(document.getElementById("map"), {
        zoom: 11,
        center: { lat: 1.3742002339365351, lng: 103.81540409330088 },
    });
    directionsRenderer.setMap(map);

    const onChangeHandler = function () {
        calculateAndDisplayRoute(directionsService, directionsRenderer);
    };
    document.getElementById("submit").addEventListener("click", () => {
        calculateAndDisplayRoute(directionsService, directionsRenderer);
    });

    document.getElementById("start").addEventListener("change", onChangeHandler);
    document.getElementById("end").addEventListener("change", onChangeHandler);
}

function calculateAndDisplayRoute(directionsService, directionsRenderer) {
    const waypts = [];
    console.log(waypts)
    const checkboxArray = document.getElementById("waypoints");

    for (let i = 0; i < checkboxArray.length; i++) {
        if (checkboxArray.options[i].selected) {
            waypts.push({
                location: checkboxArray[i].value,
                stopover: true,
            });
        }
    }

    directionsService
        .route({
            origin: "763115",
            destination: "310145",
            waypoints: waypts,
            optimizeWaypoints: true,
            travelMode: google.maps.TravelMode.DRIVING,
        })
        .then((response) => {
            directionsRenderer.setDirections(response);
        })
        .catch((e) => window.alert("Directions request failed due to " + status));
}


window.initMap = initMap;