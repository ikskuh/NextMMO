var app = require('http').createServer(handler)
var io = require('socket.io')(app);
var fs = require('fs');
var url = require('url');

require("./basicLevel.js");

app.listen(80);

var global = require('socket.io-client')('http://localhost:88');
// Add a connect listener
global.on('connect', function(socket) { 
    //console.log('Connected!');
});
global.emit('delta', { });

function prepareLevel(level) {
	function setTile(x, y, height, tex, alpha, blocked) {
		level.at(x,y).height = height;
		level.at(x,y).topTexture = tex;
		level.at(x,y).groundTexture = "dirt.png";
		level.at(x,y).alpha = alpha || 1.0;
		level.at(x,y).isWalkable = !!!blocked;
	}
	
	function setWater(x, y) {
		level.at(x,y).height = -15;
		level.at(x,y).topTexture = "water/water*20.png";
		level.at(x,y).groundTexture = "";
		level.at(x,y).alpha = 0.6;
		level.at(x,y).isWalkable = false;
	}
	
	for(var x = -7; x <= 7; x++) {
		for(var y = -7; y <= 7; y++) {
			setWater(x, y);
		}
	}
	
	for(var x = -5; x <= 5; x++) {
		for(var y = -5; y <= 5; y++) {
			setTile(x, y, 0, "gras.png");
		}
	}
	
	for(var y = -15; y <= 0; y++)
	{
		setTile(0, y, -5, "pave.png");	
	}
	
	setTile(-1, -1, -4, "pave.png");
	setTile(-1, 0, -4, "pave.png");
	setTile(-1, 1, -4, "pave.png");
	setTile(0, 1, -4, "pave.png");
	setTile(1, 1, -4, "pave.png");
	setTile(1, 0, -4, "pave.png");
	setTile(1, -1, -4, "pave.png");
	
	for(var x = -5; x <= 5; x++)
	{
		setTile(x, level.maxY-2, 32, "gravel.png");
	}
}

var level = null;
fs.readFile(__dirname + "/resources/levels/main.json",
	function (err, data) {
		if (err) {
			level = Level.create();
		} else {
			level = Level.load(data);
		}
		prepareLevel(level);
		console.log("Level ready!");
	});

function handler (req, res) {
	var uri = url.parse(req.url, true);
	var path = uri.pathname;
	if(path == "/") path = "/sites/index.html";
	
	fs.readFile(__dirname + "/../" + path,
	function (err, data) {
		if (err) {
			res.writeHead(404);
			return res.end('Error loading ' + path);
		}
		res.writeHead(200);
		res.end(data);
	});
}

var players = { }

io.on('connection', function (socket) {

	function handle(socket) {
		var player = {
			name: "",
			loggedIn: false,
			spawned: false,
			position: { x: 0.0, y: 0.0 },
			socket: socket
		};
		
		socket.emit("load-level", level);
		
		// Spawn all already-spawned players
		for(var key in players) {
			var p = players[key];
			if(!p.spawned) continue;
			socket.emit('spawn-player', {
				id: p.name,
				x: p.position.x,
				y: p.position.y,
			});
		}
		
		socket.on('login', function (data) {
			if(players[data.name] != undefined) {
				// Already logged in
				socket.emit('login-response', { success: false, reason: "already logged in" });
			} else {
				player.name = data.name;
				players[data.name] = player;
				global.emit('login', data);
			}
		});
		socket.on('request-spawn', function (data) {
			if(player.loggedIn) {
				player.spawned = true;
				io.emit('spawn-player', { id: player.name, x: 0.0, y:0.0 });
			} else {
				// TODO: Implement correct response
				console.log("try spawning non-logged-in player");
			}
		});
		socket.on('update-player', function (data) {
			player.position.x = data.x;
			player.position.y = data.y;
			
			io.emit('update-player', { id: player.name, x: data.x, y: data.y }); 
		});
		socket.on('chat', function (data) {
			if(!player.loggedIn) {
				// Just ignore chat message
				return;
			}
			
			data.sender = player.name;
			console.log("chat", data);
			socket.broadcast.emit('chat', data);
		});
		socket.on('disconnect', function (data) {
			console.log("disconnect.");
			console.log(data);
			
			if(player.spawned) {
				io.emit('despawn', { id: player.name });
			}
			
			delete players[player.name];
			player = undefined;
		});
	}
	handle(socket);
});

global.on('login-response', function (data) {
	var player = players[data.name]
	if(player === undefined) {
		console.log("Player " + data.name + " is not known to this server!");
	} else {
		player.loggedIn = data.success;
		if(data.success) {
			player.socket.emit('login-response', { success: true });
		} else {
			player.socket.emit('login-response', { success: false, reason: data.reason });
		}
	}
});