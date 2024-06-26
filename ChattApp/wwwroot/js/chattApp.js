(async function () {
    const currentUser = document.getElementById("user").value;
    const userMessage = document.getElementById("user-message");
    const btnSend = document.getElementById("button-send");
    const chatWindow = document.getElementById("chat-window");

    btnSend.addEventListener("click", async () => {
        sendData();
    });

    document.addEventListener("keypress", async (e) => {
        if (e.key === 'Enter')
            sendData();
    });

    async function sendData() {
        const message = userMessage.value;
        if (!message || message === '')
            return;
        try {
            await connection.invoke("SendMessage", { message: message });
            userMessage.value = '';
        } catch (err) {
            console.error(err);
        }
    }

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    async function start() {
        try {
            await connection.start();
            console.log(`SignalR Connected - ${currentUser} joined`);
        } catch (err) {
            console.log(err);
            setTimeout(start, 5000);
        }
    };

    connection.onclose(async () => {
        await start();
    });

    connection.on("ReceiveMessage", (data) => {
        const { userName, message, formattedCreatedOn } = data;

        const parsedDate = new Date(formattedCreatedOn.split(", ")[0]);
        const currentDate = new Date();
        const msgHeader = document.createElement('p');

        if (isSameDate(parsedDate, currentDate)) {
            msgHeader.innerHTML = `<strong>${userName}</strong> -- ${formattedCreatedOn.split(", ")[1]}`;
        } else {
            msgHeader.innerHTML = `<strong>${userName}</strong> -- ${formattedCreatedOn.split(", ")[0]}`;
        }

        const message_text_el = document.createElement('p');
        message_text_el.innerHTML = `${message}`;

        const message_el = document.createElement("div");

        message_el.classList.add("message-element")
        if (currentUser === userName) {
            message_el.classList.add("user")
        } else {
            message_el.classList.add("other")
        }
        message_el.appendChild(msgHeader);
        message_el.appendChild(message_text_el);

        chatWindow.appendChild(message_el);

        chatWindow.scrollTop = chatWindow.scrollHeight;
    });

    // Load initial messages
    const initialMessages = JSON.parse(document.getElementById('initial-messages').value);
    initialMessages.forEach(data => {
        const { userName, message, formattedCreatedOn } = data;

        const parsedDate = new Date(formattedCreatedOn.split(", ")[0]);
        const currentDate = new Date();
        const msgHeader = document.createElement('p');

        if (isSameDate(parsedDate, currentDate)) {
            msgHeader.innerHTML = `<strong>${userName}</strong> -- ${formattedCreatedOn.split(", ")[1]}`;
        } else {
            msgHeader.innerHTML = `<strong>${userName}</strong> -- ${formattedCreatedOn.split(", ")[0]}`;
        }

        const message_text_el = document.createElement('p');
        message_text_el.innerHTML = `${message}`;

        const message_el = document.createElement("div");

        message_el.classList.add("message-element")
        if (currentUser === userName) {
            message_el.classList.add("user")
        } else {
            message_el.classList.add("other")
        }
        message_el.appendChild(msgHeader);
        message_el.appendChild(message_text_el);

        chatWindow.appendChild(message_el);

        chatWindow.scrollTop = chatWindow.scrollHeight;
    });

    start();
})();

function isSameDate(date1, date2) {
    return (
        date1.getFullYear() === date2.getFullYear() &&
        date1.getMonth() === date2.getMonth() &&
        date1.getDate() === date2.getDate()
    );
}
