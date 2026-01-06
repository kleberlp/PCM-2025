// Conecta no Hub SignalR
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/jarvis/notificationHub", { withCredentials: true }) // respeita app.UsePathBase("/jarvis")
    .build();

// Escuta notificações
connection.on("ReceiveNotification", (user, message) => {
    console.log(`📩 Mensagem recebida de ${user}: ${message}`);
    showNotification(user, message);
});

// Exibe a notificação do navegador
function showNotification(user, message) {
    if (Notification.permission === "granted") {
        new Notification(`📩 ${user}`, { body: message });
    } else if (Notification.permission !== "denied") {
        Notification.requestPermission().then(permission => {
            if (permission === "granted") {
                showNotification(user, message);
            }
        });
    }
}

// Conecta
connection.start()
    .then(() => console.log("Conectado ao SignalR"))
    .catch(err => console.error("Erro SignalR:", err));

//connection.on("ReceiveNotification", (user, message) => {
//    console.log("📩 RECEBIDA:", user, message);
//    showNotification(user, message);
//});



// Register Service Worker for background notifications (if needed)
navigator.serviceWorker.register('/jarvis/sw.js', { scope: '/jarvis/' })
    .then(reg => console.log("Service Worker Registered", reg))
    .catch(err => console.error("Service Worker registration failed:", err));

//// Connect to SignalR Hub
//const connection = new signalR.HubConnectionBuilder()
//    .withUrl("/notificationHub") // Adjusted for app.UsePathBase("/jarvis")a
//    .build();

//// Handle incoming notifications
//connection.on("ReceiveNotification", (user, message) => {
//    console.log(`Mensagem de ${user}: ${message}`);
//    showNotification(user, message);
//});

//// Function to display browser notification
//function showNotification(user, message) {
//    if (Notification.permission === "granted") {
//        new Notification(`📩 ${user} says:`, {
//            body: message,
//            icon: '/images/notification_icon.png' // Optional icon
//        });
//    } else if (Notification.permission !== "denied") {
//        Notification.requestPermission().then(permission => {
//            if (permission === "granted") {
//                showNotification(user, message);
//            }
//        });
//    }
//}

//// Start the SignalR connection
//connection.start().then(() => {
//    console.log("Connected to SignalR Hub");
//}).catch(err => console.error("SignalR Connection Error:", err.toString()));

// Send notification when button is clicked
//document.getElementById("sendNotificationBtn").addEventListener("click", async () => {
//    const user = document.getElementById("username").value;
//    const message = document.getElementById("message").value;
//    if (user && message) {
//        await connection.invoke("SendNotification", user, message).catch(err => console.error(err.toString()));
//    } else {
//        alert("⚠️ Please provide both user and message.");
//    }
//});
