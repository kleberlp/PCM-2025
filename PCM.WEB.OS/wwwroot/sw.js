self.addEventListener('push', function (event) {
    const data = event.data.json();
    const options = {
        body: data.message,
        icon: '/images/notification_icon.png',
        badge: '/images/notification_badge.png'
    };

    event.waitUntil(
        self.registration.showNotification(data.title, options)
    );
});

self.addEventListener('notificationclick', function (event) {
    event.notification.close();
    event.waitUntil(
        clients.openWindow('/')
    );
});