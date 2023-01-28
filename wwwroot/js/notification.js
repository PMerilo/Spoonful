"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("PushNotification", (req) => {
    //console.log(req)
    $("#Notifications").find("p").remove()
    $("#Notifications").append(
        [
            `
            <a class="dropdown-item py-2" href="${req.url}">
                <span class="text-black">
                    <strong>${req.title}</strong>
                </span>
                <span class="small float-end text-muted">${req.dateCreated}</span>
                <div class="dropdown-message">${req.body}</div>
            </a>
            `
        ].join(""));
});

connection.on("RetrieveNotifications", (req) => {
    console.log(req)
    $("#Notifications").find("p").remove()
    for (let i = 0; i < req.length; i++) {
        console.log(req[i])
        $("#Notifications").append(
            [
                `
            <a class="dropdown-item py-2" href="${req[i].url}">
                <span class="text-black">
                    <strong>${req[i].title}</strong>
                </span>
                <span class="small float-end text-muted">${Date.parse(req[i].dateCreated).toLocaleString()}</span>
                <div class="dropdown-message">${req[i].body}</div>
            </a>
            `
            ].join(""));
    }
    //console.log($("#Notifications"))
    //$("#Notifications").empty().html(
    //    [
    //        `
    //        <a class="dropdown-item py-2" href="${req.url}">
    //            <span class="text-black">
    //                <strong>${req.title}</strong>
    //            </span>
    //            <span class="small float-end text-muted">${req.time}</span>
    //            <div class="dropdown-message">${req.body}</div>
    //        </a>
    //        `
    //    ].join(""));
});

connection.start().then(function () {
    connection.invoke("GetNotifications")
}).catch(function (err) {
    return console.error(err.toString());
});