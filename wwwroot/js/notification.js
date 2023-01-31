﻿"use strict";

dayjs().calendar()

function toNotifTime(string) {
    return dayjs(string).calendar(null, {
        sameDay: 'h:mm A', // The same day ( Today at 2:30 AM )
        lastDay: '[Yesterday]', // The day before ( Yesterday at 2:30 AM )
        sameElse: 'DD/MM/YYYY' // Everything else ( 7/10/2011 )
    })
}

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
                <span class="small float-end text-muted">${toNotifTime(req.dateCreated)}</span>
                <div class="dropdown-message">${req.body}</div>
            </a>
            `
        ].join(""));
    if ($("#NotificationBadge").length) {
        $("#NotificationBtn").children().first().text(function (i, origText) {
            return parseInt(origText)++
        })
    } else {
        $("#NotificationBtn").append(
            `
        <span class="position-absolute end-0 top-0 badge p-2 rounded-circle border-2 border-white bg-danger" id="NotificationBadge">
            <span class="position-absolute top-50 start-50 translate-middle fw-normal"></span>
            <span class="visually-hidden">unread messages</span>
        </span>
        `
        )
        console.log("EST")

    }
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
                <span class="small float-end text-muted">${toNotifTime(req[i].dateCreated)}</span>
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