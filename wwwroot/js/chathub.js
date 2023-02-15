///<reference path="../lib/jquery/dist/jquery.js" />
"use strict";

dayjs().calendar()

function toNotifTime(string) {
    return dayjs(string).calendar(null, {
        sameDay: 'h:mm A', // The same day ( Today at 2:30 AM )
        lastDay: '[Yesterday]', // The day before ( Yesterday at 2:30 AM )
        lastWeek: 'DD/MM/YYYY',
        sameElse: 'DD/MM/YYYY' // Everything else ( 7/10/2011 )
    })
}

function updateBadge(num = 1) {
    if (num == 0) {
        return
    }
    if ($("#NotificationBadge").length) {
        $("#NotificationBadge").children().first().text(function (i, origText) {
            var i = parseInt(origText)
            i += num
            return i
        })
    } else {
        $("#NotificationBtn").append(
            `
        <span class="position-absolute end-0 top-0 badge p-2 rounded-circle border-2 border-white bg-danger" id="NotificationBadge">
            <span class="position-absolute top-50 start-50 translate-middle fw-normal">${num}</span>
            <span class="visually-hidden">unread messages</span>
        </span>
        `
        )

    }
}

function formatterMSG(req) {
    var sender = $("#sender").val();
    var just = req.sender == sender ? " justify-content-end" : ""
    var side = req.sender == sender ? "right" : "left"
    return `
            <div class="d-flex${just}">
                <div class="chat-bubble chat-bubble--${side} text-break" style="max-width: 50%;">
                    ${req.message}
                </div>
            </div>
            `
}

//var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

$("#sendButton").prop('disabled', true);

connection.on("ReceiveMessage", function (user, message) {
    var msg = `<div class="d-flex">
                    <div class="chat-bubble chat-bubble--left text-break" style="max-width: 50%;">
                        ${message}
                    </div>
                </div>
                `
    $("#chat-panel").append(msg);
    $('#chat-panel').animate({ scrollTop: $('#chat-panel').prop("scrollHeight") }, 500);
});
connection.on("RetrieveChat", (req) => {
    var unread = 0
    for (let i = 0; i < req.length; i++) {
        $("#chat-panel").append(formatterMSG(req[i]));
        //if (!req[i].seen) {
        //    unread++
        //}
    }
    $('#chat-panel').animate({ scrollTop: $('#chat-panel').prop("scrollHeight") }, 500);

    //updateBadge(unread)
    //$(".notification-unread").click(function () {
    //    var id = $(this).attr("data-id");
    //    id = parseInt(id)
    //    connection.invoke("ReadNotification", id).catch(function (err) {
    //        return console.error(err.toString());
    //    });
    //});
});

//connection.start().then(function () {
//    $("#sendButton").prop('disabled', false);
//    var sender = $("#sender").val();
//    var user = $("#username").text().replace("@", "");
//    connection.invoke("GetChat", sender, user)
//}).catch(function (err) {
//    return console.error(err.toString());
//});

$("#sendButton").click((event) => {
    var message = $("#messageInput").val();
    if (!message) {
        return
    }
    $("#messageInput").val("");
    var sender = $("#sender").val();
    var user = $("#username").text().replace("@", "");
    var msg = `<div class="d-flex justify-content-end">
                    <div class="chat-bubble chat-bubble--right text-break" style="max-width: 50%;">
                        ${message}
                    </div>
                </div>
                `
    $("#chat-panel").append(msg);
    connection.invoke("SendMessage", user, message, sender).then(() => {
        $('#chat-panel').animate({ scrollTop: $('#chat-panel').prop("scrollHeight") }, 500);
    }).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});