var game = { };

function initializeGame() {
	game.socket = io('http://localhost');
	game.listener = new window.keypress.Listener();
	game.resources = { };
	game.resources.emptyTileTexture = PIXI.Texture.fromImage("textures/empty.png");
	game.user = {  }
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
}

function login(name, password) {

	if(name === undefined) {
		name = document.getElementById("username").value;
		password = document.getElementById("username").value;
	} else {
		password = password || "";
	}
	
	game.user.name = name;
	game.socket.emit('login', {
		name: name,
		password: password
	});
}

function initializePIXI() {
	// create a renderer instance.
	game.graphics = { }
	game.graphics.renderer = PIXI.autoDetectRenderer(window.innerWidth, window.innerHeight, null, true);
	game.graphics.renderer.view.className = 'pixiView';
	game.graphics.renderer.view.id = 'renderer';
	game.graphics.renderer.view.oncontextmenu = function () { return false; }
	window.onresize = function(event) {
		game.graphics.renderer.resize(window.innerWidth, window.innerHeight);
	};
	document.body.appendChild(game.graphics.renderer.view);
	
	game.graphics.stage = new PIXI.Stage(0x000022);
	game.graphics.levelStage = new PIXI.DisplayObjectContainer();
	game.graphics.stage.addChild(game.graphics.levelStage);
	
	// create an array of assets to load
	//var assetsToLoader = [ "sprites.json", "characters/default.json" ];
	//
	//// create a new loader
	//game.graphics.loader = new PIXI.AssetLoader(assetsToLoader);
	//
	//// use callback
	//game.graphics.loader.onComplete = function onAssetsLoaded()
	//{
	//	// start animating
	//	requestAnimFrame( animate );
	//}
	
	//begin load
	//game.graphics.loader.load();
	
	requestAnimFrame( animate );
}

function initializePivot() {
	game.pivot = {
		x: 0,
		y: 0, 
		move: false,
		lastX: 0,
		lastY: 0,
		invert: true
	};
	game.graphics.renderer.view.onmousedown = function (e) {
		if(e.button === 2) {
			game.pivot.move = true;
			game.pivot.lastX = e.screenX;
			game.pivot.lastY = e.screenY;
		}
	};
	game.graphics.renderer.view.onmousemove = function (e) {
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
	game.graphics.renderer.view.onmouseup = function (e) {
		if(e.button === 2) {
			game.pivot.move = false;
		}
	};
	game.graphics.renderer.view.onmouseleave = function (e) {
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
		if(data.success) {
			game.socket.emit('request-spawn', { });
			setLoginMenuVisible(false);
		} else {
			alert("Login failed: " + data.reason);
		}
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
	game.graphics.levelStage.addChild(level.stage);
	
	if(level.sprites === undefined) {
		level.sprites = { };
		level.spriteAt = function (x,y) {
			var coord = x + ";" + y;
			if(level.sprites[coord] === undefined) {
				var tile = level.at(x,y);
				if(tile.textureName != "") {
					var match = /.+\*\d+\.png/.exec(tile.textureName);
					if(match != null) {
						
						match = /\d+\.png/g.exec(tile.textureName);
						
						var texName = tile.textureName.substring(0, tile.textureName.length - match[0].length - 1);
						
						var texCount = match[0].substring(0, match[0].length - 4);
						
						var frames = [];
						for(var i = 1; i <= texCount; i++) {
							var fileName = texName + "_" + i + ".png";
							var tex = PIXI.Texture.fromImage("textures/" + fileName);
							frames.push(tex);
						}

						level.sprites[coord] = new PIXI.MovieClip(frames);
						level.sprites[coord].play();
						level.sprites[coord].animationSpeed = 0.2;
					} else {
						level.sprites[coord] = PIXI.Sprite.fromImage("textures/" + tile.textureName);
					}
				}
				else {
					level.sprites[coord] = new PIXI.Sprite(game.resources.emptyTileTexture);
				}
				level.sprites[coord].alpha = tile.alpha || 1.0;
				level.stage.addChild(level.sprites[coord]);
			}
			return level.sprites[coord];
		};
		
		for(var x = level.minX; x <= level.maxX; x++) {
			for(var y = level.maxY; y >= level.minY; y--) {
				function create() {
				
					var tile = level.at(x,y);
					var sprite = level.spriteAt(x,y);
					
					sprite.interactive = tile.isWalkable || false;
					if(sprite.interactive) {
						sprite.hitArea = new PIXI.Polygon(
							48, 0,
							96, 24,
							48, 48,
							0, 24);
						sprite.mouseover = function (e) {
							e.target.tint = 0xAAAAAA;
						};
						sprite.mouseout = function (e) {
							e.target.tint = 0xFFFFFF;
						};
						sprite.click = function (e) {
							if(!game.pivot.move && game.player != undefined) {
								game.player.target = level.transformBack(e.global.x, e.global.y, e.target.tile.height);
								//game.player.target = e.target.tile;
							}
						};
					}
					
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
				}
				create();
			}
		}
	}
	
	level.transform = function (x, y, h) {
		var pos = { };
		pos.x = 0.5 * game.graphics.renderer.width + game.pivot.x + 48 * x + 48 * y;
		pos.y = 0.5 * game.graphics.renderer.height + game.pivot.y + 24 * x - 24 * y - h;
		return pos;
	}
	
	level.transformBack = function(x, y, h) {
		var pos = { };
		
		x -= 0.5 * game.graphics.renderer.width + game.pivot.x;
		y -= 0.5 * game.graphics.renderer.height + game.pivot.y - h;
		
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
		game.graphics.levelStage.removeChild(level.stage);
		level.stage = null;
	}
	
	level.update = function () {
		for(var x = level.minX; x <= level.maxX; x++) {
			for(var y = level.minY; y <= level.maxY; y++) {
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

function loadCharacterSpriteSheet(name) {
	var directions = [ "n", "ne", "nw", "e", "w", "s", "sw", "se" ];
	var spriteSheet = { walk: { }, idle: { }}
	for(var animation in spriteSheet) {
		for(var i = 0; i < directions.length; i++) {
			var dir = directions[i];
			var sheet = spriteSheet[animation];
			
			var sprites = []
			for(var j = 0; j < 20; j++) {
				var fname = "characters/" +name+ "/" + animation +"/" + dir + "_" + (j+1) + ".png";
				sprites.push(new PIXI.Texture.fromImage(fname));
			}
			sheet[dir] = sprites;
		}
	}
	return spriteSheet;
}

function createBasePlayer() {

	var spriteSheet = loadCharacterSpriteSheet("default");

	var player = {}
	player.sprite = new PIXI.MovieClip(spriteSheet["walk"]["s"]);
	player.sprite.animationSpeed = 0.8;
	player.sprite.play();
	player.sprite.anchor.x = 0.5;
	player.sprite.anchor.y = 0.7;
	game.graphics.stage.addChild(player.sprite);
	
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
		game.graphics.stage.removeChild(player.sprite);
	}
	player.lastDirection = "s";
	
	player.move = function () {
		
		// Adjust animation here!
	
		if(player.target == null)
		{
			player.sprite.textures = spriteSheet["idle"][player.lastDirection];
			return 0.0;
		}
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
			
			var abs = Math.abs;
			var cos = function (f) { return Math.tan(f * 0.0174532925); } 
			if(deltaX == 0.0) deltaX = 0.001;
			
			var dy = deltaY / abs(deltaX);
			if(deltaX < 0) {
				if(dy < cos(-22.5 * 3)) {
					console.log("1");
					player.lastDirection = "sw";
				} else if(dy < cos(-22.5)) {
					console.log("2");
					player.lastDirection = "w";
				} else if(dy < cos(22.5)) {
					console.log("3");
					player.lastDirection = "nw";
				} else if(dy < cos(22.5 * 3)) {
					console.log("4");
					player.lastDirection = "n";
				} else {
					console.log("5");
					player.lastDirection = "ne";
				}
			} else {
				if(dy < cos(-22.5 * 3)) {
					console.log("6");
					player.lastDirection = "sw";
				} else if(dy < cos(-22.5)) {
					console.log("7");
					player.lastDirection = "s";
				} else if(dy < cos(22.5)) {
					console.log("8");
					player.lastDirection = "se";
				} else if(dy < cos(22.5 * 3)) {
					console.log("9");
					player.lastDirection = "e";
				} else {
					console.log("10");
					player.lastDirection = "ne";
				}
			}
			
			
			
			player.sprite.textures = spriteSheet["walk"][player.lastDirection];
			
			return len;
		}
		else
		{
			player.sprite.textures = spriteSheet["idle"][player.lastDirection];
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
	game.graphics.renderer.render(game.graphics.stage);
}


// Some global stuff
Math.sign = function (v) {
	if(v > 0) return 1;
	if(v < 0) return -1;
	return 0;
}