// Local function
var createBasePlayer = function () {

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
					//console.log("1");
					player.lastDirection = "sw";
				} else if(dy < cos(-22.5)) {
					//console.log("2");
					player.lastDirection = "w";
				} else if(dy < cos(22.5)) {
					//console.log("3");
					player.lastDirection = "nw";
				} else if(dy < cos(22.5 * 3)) {
					//console.log("4");
					player.lastDirection = "n";
				} else {
					//console.log("5");
					player.lastDirection = "ne";
				}
			} else {
				if(dy < cos(-22.5 * 3)) {
					//console.log("6");
					player.lastDirection = "sw";
				} else if(dy < cos(-22.5)) {
					//console.log("7");
					player.lastDirection = "s";
				} else if(dy < cos(22.5)) {
					//console.log("8");
					player.lastDirection = "se";
				} else if(dy < cos(22.5 * 3)) {
					//console.log("9");
					player.lastDirection = "e";
				} else {
					//console.log("10");
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

function createProxyPlayer(name) {
	name = name || "unnamed";
	var proxy = createBasePlayer();
	proxy.update = function () {
		proxy.move();
		proxy.baseUpdate();
	}
	
	var text = new PIXI.Text(
		name, {
			font: "12px Tahoma", 
			align: "center",
			fill: "white",
		});
	text.position.x = -0.5 * text.width;
	text.position.y = -65;
	proxy.sprite.addChild(text);
	
	proxy.setName = function (name) {
		text.setText(name || "unnamed");
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