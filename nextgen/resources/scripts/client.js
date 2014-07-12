var game = { };

function initializeGame() {
	game.socket = io('http://localhost');
	game.listener = new window.keypress.Listener();
	game.stage = new PIXI.Stage(0x000022);
	game.levelStage = new PIXI.DisplayObjectContainer();
	game.stage.addChild(game.levelStage);
	game.resources = { };
	game.resources.emptyTileTexture = PIXI.Texture.fromImage("textures/empty.png");
	game.user = { name: undefined }
	game.proxyPlayers = { }
	
	game.loadLevel = function (level) {
		if(game.level != null) {
			game.level.destroy();
			game.level = null;
		}
		
		game.level = null;
		if(level != null) {
			// Prepare basic level object
			Level.initialize(level);
		
			// Prepare level for PIXI use
			game.level = prepareLevel(level);
			console.log("Loaded new level.");
		}
	}
	
	initializePIXI();
	initializePivot();
	intializeNetwork();

	requestAnimFrame( animate );
}

function login(name, password) {
	game.user.name = name;
	game.socket.emit('login', {
		name: name,
		password: password
	});
}

function initializePIXI() {
	// create a renderer instance.
	game.renderer = PIXI.autoDetectRenderer(window.innerWidth, window.innerHeight);
	game.renderer.view.className = 'pixiView';
	window.onresize = function(event) {
		game.renderer.resize(window.innerWidth, window.innerHeight);
	};
	document.body.appendChild(game.renderer.view);
}

function initializePivot() {
	game.pivot = { x: 64, y: 0.5 * game.renderer.height, move:false, lastX: 0, lastY: 0, invert: true };
	window.onmousedown = function (e) {
		if(e.button === 2) {
			game.pivot.move = true;
			game.pivot.lastX = e.screenX;
			game.pivot.lastY = e.screenY;
		}
	};
	window.onmousemove = function (e) {
		if(game.pivot.move) {
			var sign = 1
			if(game.pivot.invert)
				sign = -1;
			
			game.pivot.x += sign * (game.pivot.lastX - e.screenX);
			game.pivot.y += sign * (game.pivot.lastY - e.screenY);
			
			game.pivot.lastX = e.screenX;
			game.pivot.lastY = e.screenY;
		}
	};
	window.onmouseup = function (e) {
		if(e.button === 2) {
			game.pivot.move = false;
		}
	};
	window.onmouseleave = function (e) {
		game.pivot.move = false;
	};
}

function intializeNetwork() {
	game.socket.on('load-level', function (level) {
		game.loadLevel(level);
	});
	game.socket.on('spawn-player', function (data) {
		var player = undefined;
		if(data.id == game.user.name) {
			player = createPlayer();
			game.player = player;
			game.player.id = data.id;
			
		} else {
			player = createProxyPlayer();
			player.id = data.id;
			game.proxyPlayers[data.id] = player;
			console.log(player);
		}
		if(data.x != undefined) {
			// If forced spawn point
			player.x = data.x;
			player.y = data.y;
		}
	});
	game.socket.on('login-response', function (data) {
		game.socket.emit('request-spawn', { });
	});
	game.socket.on('update-player', function (data) {
		if(data.id == game.user.name) return;
		
		if(game.proxyPlayers[data.id] != undefined) {
			game.proxyPlayers[data.id].target = data;
		}
	});
	
	game.socket.on('despawn', function (data) {
		if(game.proxyPlayers[data.id] != undefined) {
			game.proxyPlayers[data.id].destroy();
			delete game.proxyPlayers[data.id];
		}
	});
}

/*
	
	socket.on('news', function (data) {
		console.log(data);
		
		var div = document.getElementById('log');
		
		var e = document.createElement("p");
		e.textContent = data.msg;
		e.innerText = data.msg;
		
		div.appendChild(e);
	});
	
	function send(msg) {
		socket.emit('my other event', { msg: msg });
	}
*/

function prepareLevel(level)  {
	
	level = level || Level.create(11, 11);
	
	level.stage = new PIXI.DisplayObjectContainer();
	game.levelStage.addChild(level.stage);
	
	if(level.sprites === undefined) {
		level.sprites = { };
		level.spriteAt = function (x,y) {
			var coord = x + ";" + y;
			if(level.sprites[coord] === undefined) {
				var tile = level.at(x,y);
				if(tile.textureName != "")
					level.sprites[coord] = PIXI.Sprite.fromImage("textures/" + tile.textureName);
				else
					level.sprites[coord] = new PIXI.Sprite(game.resources.emptyTileTexture);
				level.stage.addChild(level.sprites[coord]);
			}
			return level.sprites[coord];
		};
		
		for(var x = 0; x < level.width; x++)
		{
			for(var y = level.height - 1; y >= 0; y--)
			{
				function create() {
				
					var  sprite = level.spriteAt(x,y);
					
					sprite.hitArea = new PIXI.Polygon(
						48, 0,
						96, 24,
						48, 48,
						0, 24);
					sprite.interactive = true;
					sprite.mouseover = function (e) {
						e.target.tint = 0xAAAAAA;
					};
					sprite.mouseout = function (e) {
						e.target.tint = 0xFFFFFF;
					};
					sprite.click = function (e) {
						if(!game.pivot.move) {
							game.player.target = level.transformBack(e.global.x, e.global.y, e.target.tile.height);;
						}
					};
					
					var tile = level.at(x,y);
					tile.setTexture = function (fileName) {
						fileName = fileName || "";
						tile.textureName = fileName;
						var texture = null;
						if(fileName != "") {
							texture = PIXI.Texture.fromImage("textures/" + fileName);
						} else {
							texture = game.resources.emptyTileTexture;
						}
						sprite.setTexture(texture);
					}
					
					sprite.tile = tile; // Double linking
					
					// Set default texture
					if(tile.textureName == "") {
						tile.setTexture("dirt_grass.png");
					} else {
						tile.setTexture(tile.textureName);
					}
				}
				create();
			}
		}
	}
	
	level.transform = function (x, y, h) {
		var pos = { };
		pos.x = game.pivot.x + 48 * x + 48 * y;
		pos.y = game.pivot.y + 24 * x - 24 * y - h;
		return pos;
	}
	
	level.transformBack = function(x, y, h) {
		var pos = { };
		
		x -= game.pivot.x;
		y -= game.pivot.y - h;
		
		x *= 1.0 / 48.0;
		y *= 1.0 / 24.0;
		
		pos.x = 0.5 * x + 0.5 * y;
		pos.y = 0.5 * x - 0.5 * y;
		
		return pos;
	}
	
	level.destroy = function () {
		for(var key in level.sprites) {
			level.stage.removeChild(level.sprites[key]);
		}
		game.levelStage.removeChild(level.stage);
		level.stage = null;
	}
	
	level.update = function () {
		for(var x = 0; x < level.width; x++)
		{
			for(var y = 0; y < level.height; y++)
			{
				var tile = level.at(x,y);
				var sprite = level.spriteAt(x,y);
				sprite.position = level.transform(x, y, tile.height);
				sprite.x -= 48; // Manual, pixel perfect anchor
				sprite.y -= 24;
			}
		}
	}
	
	level.update();
	
	return level;
}

function createBasePlayer() {
	var player = {}
	player.sprite = new PIXI.Sprite.fromImage("images/player.png");
	player.sprite.anchor.x = 0.5;
	player.sprite.anchor.y = 0.9;
	game.stage.addChild(player.sprite);
	
	player.x = 0.0;
	player.y = 0.0;
	
	player.baseUpdate = function () {
		if(game.level != null) {
			// Update player position
			var tile = game.level.at(Math.floor(player.x + 0.5), Math.floor(player.y + 0.5));
			player.sprite.position = game.level.transform(player.x, player.y, tile.height);
		}
	}
	player.baseDestroy = function () {
		game.stage.removeChild(player.sprite);
	}
	
	player.move = function () {
		if(player.target == null)
			return 0.0;
		var deltaX = player.target.x - player.x;
		var deltaY = player.target.y - player.y;
		
		var spd = 0.05;
		var len = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
		deltaX *= spd / len;
		deltaY *= spd / len;
		
		if(len > 0.05)
		{
			player.x += deltaX;
			player.y += deltaY;
			return len;
		}
		else
		{
			return 0.0;
		}
	}
	
	player.update = player.baseUpdate;
	player.destroy = player.baseDestroy;
	
	return player;
}

function createProxyPlayer() {
	var proxy = createBasePlayer();
	proxy.update = function () {
		proxy.move();
		proxy.baseUpdate();
	}
	return proxy;
}

function createPlayer() {
	
	player = createBasePlayer();
	player.x = 5.0;
	player.y = 5.0;
	player.target = null;
	player.ticks = 0;
	player.update = function () {
		
		if(player.move() > 0.0) {
			if(player.ticks > 4) {
				game.socket.emit('update-player', { x: player.x, y: player.y });
				player.ticks = 0;
			}
		}
				
		// Adjust movement
		player.baseUpdate();
		
		player.ticks += 1;
	};
	
	return player;
}

function animate() {

	requestAnimFrame( animate );
	
	if(game.level != null) {
		game.level.update();
	}
	if(game.player != null) {
		game.player.update();
	}
	for(var id in game.proxyPlayers) {
		game.proxyPlayers[id].update();
	}

	// render the stage   
	game.renderer.render(game.stage);
}


// Some global stuff
Math.sign = function (v) {
	if(v > 0) return 1;
	if(v < 0) return -1;
	return 0;
}