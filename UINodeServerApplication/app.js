const amqp = require('amqplib');
const http = require('http');
const socketIo = require('socket.io');

const socket = io('http://localhost:11031');

socket.on('connect', () => {
    console.log('Connected to Socket.io server');
});

socket.on('disconnect', () => {
    console.log('Disconnected from Socket.io server');
});

// Listen for the 'ReceiveMessage' event and process the received data
socket.on('ReceiveMessage', (symbols) => {
    //console.log('Received symbols:', symbols);
});

// RabbitMQ configuration
const rabbitMQConfig = {
  host: 'localhost',
  port: 5672,
  username: 'guest',
  password: 'guest'
};

// Socket.io server configuration
const socketIoPort = 11031;

// Create an HTTP server
const server = http.createServer();

// Initialize the Socket.io server with CORS configuration
const io = socketIo(server, {
  cors: {
    origin: ['http://localhost:10204','http://localhost:10203'], 
    methods: ['GET', 'POST']
  }
});

// Socket.io connection handler
io.on('connection', (socket) => {
  console.log('Socket.io client connected');

  socket.on('disconnect', () => {
    console.log('Socket.io client disconnected');
  });
});

// Start the HTTP server
server.listen(socketIoPort, () => {
  console.log(`Socket.io server is listening on port ${socketIoPort}`);

  // After the HTTP server is started, connect to RabbitMQ
  connectToRabbitMQ().then(() => {
    console.log('Connected to RabbitMQ');
  });
});

// Create a connection to RabbitMQ and consume messages
async function connectToRabbitMQ() {
  const connection = await amqp.connect({
    hostname: rabbitMQConfig.host,
    port: rabbitMQConfig.port,
    username: rabbitMQConfig.username,
    password: rabbitMQConfig.password
  });

  const channel = await connection.createChannel();

  // Declare the queue
  const queueName = 'TseData';
  await channel.assertQueue(queueName, { durable: true });

  // Consume messages from the queue
  channel.consume(queueName, (message) => {
    if (message) {
      const symbols = JSON.parse(message.content.toString());
      io.emit('ReceiveMessage', symbols); // Emit to 'ReceiveMessage' event
    }
  }, { noAck: true });
}