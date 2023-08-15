# Multi-App Data Pipeline and Real-time Monitoring System


![image](https://github.com/Alirale/TSE_DataCrawleringApp/assets/59726045/6b2b0601-ee71-4e65-8887-81dafd2c6f0a)


This repository contains a set of three interconnected applications that work together to create a data pipeline and real-time monitoring system. The applications are built using various technologies including .NET Core, Node.js, and pure JavaScript. Each app serves a specific purpose and collaborates with the others to achieve the overall functionality.
<br>
## Application Default URLs

| App Name            | URL                              |
|---------------------|----------------------------------|
| FrontAPP            | [http://localhost:10204](http://localhost:10204)           |
| ServerApp           | [http://localhost:10203](http://localhost:10203)           |
| CrawlerApp          | [http://localhost:11030](http://localhost:11030)           |
| RabbitMQ            | [http://localhost:5672](http://localhost:5672)            |
| SocketIO Server URL | [http://localhost:15003](http://localhost:15003)           |

<br>
## CrawlerApp (.NET Core Application)

The **CrawlerApp** is a .NET Core application designed with the Clean Architecture pattern. It is responsible for fetching data from the [Tsetmc](http://www.tsetmc.com/) (Tehran Securities Exchange Technology Management Co) website at regular intervals (every 3 seconds) using a Quartz job. The data is then stored in an in-memory SQL Server table. Additionally, the changes since the last job cycle are sent to a RabbitMQ publisher.

- **Application directory Path :** `./CrawlerApplication`
<br><br>
## UINodeServerApplication (Node.js Application)

The **UINodeServerApplication** is a Node.js application that serves as a Socket.IO server. It consumes data sent from the CrawlerApp via RabbitMQ and broadcasts this data to connected clients through Socket.IO. A setup script named `setup.sh` is provided in the `./UINodeServerApplication` folder that gets the required node modules.

- **Application directory Path:** `./UINodeServerApplication`
<br><br>
## Front Application (Pure JavaScript Application)

The **Front JS Application** is a pure JavaScript application that acts as a Socket.IO client. It connects to the UINodeServerApplication's Socket.IO server to receive real-time updates. It initially fetches data from the CrawlerApp using the API endpoint [http://localhost:11030/Symbol/v1/Get](http://localhost:11030/Symbol/v1/Get). The application then displays both the initial data and real-time updates received through the socket connection.

- **Application directory Path:** `./FrontApplication`

<br>
## Setup Instructions

1. Clone this repository to your local machine.
2. Navigate to the `./CrawlerApplication` folder and run the .NET Core application.
   ```bash
   dotnet run
   
3. Navigate to the `./UINodeServerApplication` folder and run the **'setup.sh'** file.
4. After setup done to run the Socket Server open gitbash and type:
   ```bash
   node app.js

