var socket = io('http://localhost:15003');

socket.on('connect', function () {
    console.log("Connected to Socket.IO server");
});

 socket.on('ReceiveMessage', function (symbols) {
     symbols.forEach(function (symbol) {
         var row = document.querySelector('[data-id="' + symbol.symbolISIN + '"]');
         if (row) {
             var tds = row.querySelectorAll('td');
             if (symbol.lastTradedPrice) {
                 tds[0].textContent = symbol.lastTradedPrice;
                 tds[0].classList.add("flash");
             }

             if (symbol.closingPrice) {
                 tds[1].textContent = symbol.closingPrice;
                 tds[1].classList.add("flash");
             }


             setTimeout(() => {
                 tds[0].classList.remove("flash");
                 tds[1].classList.remove("flash");
             }, 1000);
         }
     });
     console.log("Received message:", symbols);
 });

const table = document.querySelector('.symbolsTable');

function getData() {
    fetch('http://localhost:11030/Symbol/v1/Get')
        .then((res) => res.json())
        .then((data) => {
            const tbody = table.querySelector('#priceTableBody');
            tbody.innerHTML = '';

            data.forEach(element => {
                const row = document.createElement('tr');
                row.setAttribute("data-id", `${element.symbolISIN}`);
                row.innerHTML = `
                    <td>${element.lastTradedPrice}</td>
                    <td>${element.closingPrice}</td>
                    <td>${element.symbolTitle}</td>
                `;
                tbody.appendChild(row);
            });
        })
        .catch((error) => {
            console.error('Error fetching data:', error);
        });
}

getData();