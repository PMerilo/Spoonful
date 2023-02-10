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

function formatter(req) {
    var unreadClass = req.seen ? "" : "notification-unread"
    var bold = req.seen ? "" : "fw-bold"
    return `
            <a class="dropdown-item py-2 px-4 ${unreadClass}" href="${req.url}" data-id="${req.id}">
                <div class="d-flex justify-content-between align-items-baseline">
                    <span class="text-black text-wrap text-break ${bold}">
                        ${req.title}
                    </span>
                    <span class="small float-end text-muted ms-2">${toNotifTime(req.dateCreated)}</span>
                </div>
                <div class="dropdown-text text-wrap text-break">${req.body}</div>
            </a>
            `
}

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").withAutomaticReconnect().build();
connection.on("PushNotification", (req) => {
    $("#Notifications").find("p").remove()
    $("#Notifications").prepend(formatter(req));
    updateBadge()
});

connection.on("RetrieveNotifications", (req) => {
    $("#Notifications").find("p").remove()
    var unread = 0
    for (let i = 0; i < req.length; i++) {
        $("#Notifications").append(formatter(req[i]));
        if (!req[i].seen) {
            unread++
        }
    }
    updateBadge(unread)
    $(".notification-unread").click(function () {
        var id = $(this).attr("data-id");
        id = parseInt(id)
        connection.invoke("ReadNotification", id).catch(function (err) {
            return console.error(err.toString());
        });
    });
});

connection.start().then(function () {
    connection.invoke("GetNotifications")
}).catch(function (err) {
    return console.error(err.toString());
});