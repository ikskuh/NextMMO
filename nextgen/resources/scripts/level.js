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