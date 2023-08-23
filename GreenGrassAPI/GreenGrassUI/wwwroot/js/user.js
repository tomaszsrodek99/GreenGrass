function initializeNotificationList() {
    var notificationList = $('#notification-list');
    var isOpen = false;
    $('#notification-link').click(function () {
        isOpen = !isOpen;
        notificationList.toggle(isOpen);
    });
}

function checkUserNotifications(userId) {
    $.ajax({
        url: `https://localhost:44304/api/Plant/CheckNotifications${userId}`,
        type: 'GET',
        success: function (notifications) {
            updateNotificationCount(notifications.length, notifications);
        },
        error: function () {
            // Obs³uga b³êdu
        }
    });
}

function updateNotificationCount(count, notifications) {
    var notificationBadge = document.getElementById('notification-count');
    var notificationList = document.getElementById('notification-list');
    if (count > 0) {
        notificationBadge.innerText = count;
        notificationBadge.style.display = 'inline-block';
        while (notificationList.firstChild) {
            notificationList.removeChild(notificationList.firstChild);
        }
        notifications.forEach(function (notification) {
            console.dir(notification);
            var li = document.createElement('li');
            var plantName = document.createElement('span');
            li.appendChild(plantName);

            var nextFertilizingDate = new Date(notification.nextFertilizingDate);
            var nextWateringDate = new Date(notification.nextWateringDate);

            if (nextWateringDate <= new Date()) {
                plantName.innerText = notification.plant.name;
                var wateringNotification = document.createElement('span');
                wateringNotification.innerText = ' - Konieczne podlewanie!';
                li.appendChild(wateringNotification);
            }

            if (nextFertilizingDate <= new Date()) {
                plantName.innerText = notification.plant.name;
                var fertilizingNotification = document.createElement('span');
                fertilizingNotification.innerText = ' - Konieczne nawo¿enie!';
                li.appendChild(fertilizingNotification);
            }

            notificationList.appendChild(li);
        });
    } else {
        notificationBadge.innerText = "(0)";
        notificationBadge.style.display = 'inline-block';
        notificationList.style.display = 'none';

        var li = document.createElement('li');
        li.innerText = 'Brak powiadomieñ.';
        notificationList.appendChild(li);
    }
}

async function refreshToken() {
    try {
        await axios.post('/api/User/RefreshToken');
    } catch (error) {
        console.error('B³¹d podczas odœwie¿ania tokenu:', error);
    }
}

function startTokenRefreshInterval() {
    console.log("Odliczamy");
    setInterval(refreshToken, 1800000);
}

document.addEventListener('DOMContentLoaded', () => {
    startTokenRefreshInterval();
});

