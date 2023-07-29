﻿function showLoadingIcon() {
    $('#blurOverlay').show();
    $('body').addClass('blur-overlay-visible');
    $('<i class="fas fa-spinner fa-spin loading-spinner"></i>').css('display', 'block').appendTo('body');
}
function hideLoadingIcon() {
    $('.loading-spinner').remove();
    $('#blurOverlay').hide();
    $('body').removeClass('blur-overlay-visible');
}

let isFirstSubmit = true;
function loading(time) {
    if (isFirstSubmit) {
        isFirstSubmit = false;
        loading(100);
    }
    else {
        showLoadingIcon();
        setTimeout(function () {
            hideLoadingIcon();
        }, time);
        const allSpans = document.getElementsByTagName("span");
        for (const span of allSpans) {
            if (span.classList.contains("field-validation-error")) {
                hideLoadingIcon();
                break;
            }
        }
    }
}





/*function initializeNotificationList() {
    var notificationList = $('#notification-list');
    var isOpen = false;
    $('#notification-link').click(function () {
        isOpen = !isOpen;
        notificationList.toggle(isOpen);
    });
}
window.onload = function () {
    initializeNotificationList();
    checkPlantNotifications();
};
function searchPlantByName() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("myInput");
    filter = input.value.toUpperCase();
    table = document.getElementById("searchTable");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[1];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}
function goBack() {
    history.back();
}
function checkPlantNotifications() {
    $.ajax({
        url: '/Plant/CheckNotifications',
        type: 'GET',
        success: function (notifications) {
            updateNotificationCount(notifications.length, notifications);
        },
        error: function () {
        }
    });
}
function updateNotificationCount(count, notifications) {
    console.log(notifications);
    var notificationBadge = document.getElementById('notification-count');
    var notificationList = document.getElementById('notification-list');
    if (count > 0) {
        notificationBadge.innerText = count;
        notificationBadge.style.display = 'inline-block';
        while (notificationList.firstChild) {
            notificationList.removeChild(notificationList.firstChild);
        }
        notifications.forEach(function (notification) {
            console.log(notification);
            var li = document.createElement('li');
            var plantName = document.createElement('span');
            plantName.innerText = notification.plant.name;
            li.appendChild(plantName);
            if (notification.plant.daysUntilWatering <= 0 && notification.plant.lastWateringDate !== "0001-01-01T00:00:00") {
                var wateringNotification = document.createElement('span');
                wateringNotification.innerText = ' - Konieczne podlewanie!';
                li.appendChild(wateringNotification);
            }
            if (notification.plant.daysUntilFertilizing <= 0 && notification.plant.lastFertilizingDate !== "0001-01-01T00:00:00") {
                var fertilizingNotification = document.createElement('span');
                fertilizingNotification.innerText = ' - Konieczne nawożenie!';
                li.appendChild(fertilizingNotification);
            }
            notificationList.appendChild(li);
        });
    } else {
        notificationBadge.innerText = "(0)";
        notificationBadge.style.display = 'inline-block';
        notificationList.style.display = 'none';
    }
}
*/