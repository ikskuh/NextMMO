var app = require('http').createServer(handler)
var io = require('socket.io')(app);
var fs = require('fs');
var url = require('url');

app.listen(88);

function handler (req, res) {
	res.writeHead(500);
	return res.end('Not allowed');
}

io.on('connection', function (socket) {
	socket.on('login', function (data) {
		console.log(data);
		
		socket.emit('login-response', { name: data.name, success: true });
	});
});