function prepareLevel(level)  {
	
	var tileWidth = 192;
	var tileHeight = 95;
	
	level = level || Level.create(11, 11);
	
	level.stage = new PIXI.DisplayObjectContainer();
	game.graphics.levelStage.addChild(level.stage);
	
	if(level.sprites === undefined) {
		level.sprites = { };
		level.spriteAt = function (x,y) {
			var coord = x + ";" + y;
			if(level.sprites[coord] === undefined) {
				var tile = level.at(x,y);
				level.sprites[coord] = createSprite("top/" + tile.topTexture);
				level.sprites[coord].alpha = tile.alpha || 1.0;
				
				level.stage.addChild(level.sprites[coord]);
				
				if(tile.groundTexture != "") {
					
					var wall = createSprite("ground/" + tile.groundTexture);
					wall.alpha = tile.alpha || 1.0;
					level.sprites[coord].addChild(wall);
					
					level.sprites[coord].groundRef = wall;
				}
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
						//sprite.hitArea = new PIXI.Polygon(
						//	0.5 * tileWidth, 0,
						//	tileWidth, 0.5 * tileHeight,
						//	0.5 * tileWidth, tileHeight,
						//	0, 0.5 * tileHeight);
						sprite.hitArea = new PIXI.Polygon(
							0.0, -0.5 * tileHeight,
							0.5 * tileWidth, 0.0,
							0.0, 0.5 * tileHeight,
							-0.5 * tileWidth, 0.0);
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
		pos.x = 
			0.5 * game.graphics.renderer.width + 
			game.pivot.x + 
			0.5 * tileWidth * x + 
			0.5 * tileWidth * y;
		pos.y =
			0.5 * game.graphics.renderer.height + 
			game.pivot.y + 
			0.5 * tileHeight * x - 
			0.5 * tileHeight * y - h;
		return pos;
	}
	
	level.transformBack = function(x, y, h) {
		var pos = { };
		
		x -= 0.5 * game.graphics.renderer.width + game.pivot.x;
		y -= 0.5 * game.graphics.renderer.height + game.pivot.y - h;
		
		x *= 1.0 / (0.5 * tileWidth);
		y *= 1.0 / (0.5 * tileHeight);
		
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
				//sprite.x -= tileWidth * 0.5; // Manual, pixel perfect anchor
				//sprite.y -= tileHeight * 0.5;
			}
		}
	}
	
	level.update();
	
	return level;
}