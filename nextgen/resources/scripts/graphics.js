function initializePIXI(initialized) {
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
	
	// create a new loader
	game.graphics.loader = new PIXI.AssetLoader(game.spritesheets);
	
	// use callback
	game.graphics.loader.onComplete = function onAssetsLoaded()
	{
		if(initialized != undefined) {
			initialized();
		}
	
		// start animating
		requestAnimFrame( animate );
	}
	
	//begin load
	game.graphics.loader.load();
}

function loadTexture(name) {
	if(game.metadata.frames[name] != undefined) {
		var texture = new PIXI.Texture.fromFrame(name);
		texture.anchor = { }
		texture.anchor.x = game.metadata.frames[name].anchor.x;
		texture.anchor.y = game.metadata.frames[name].anchor.y;
		texture.animationSpeed = game.metadata.frames[name].animationSpeed || 0.15;
		return texture;
	} else {
		console.log("Fallback to non-atlas: ", name);
		var texture = new PIXI.Texture.fromImage("textures/" + name);
		texture.anchor = { x: 0.0, y: 0.0 }
		texture.animationSpeed = 0.15;
		return texture;
	}
}

function createSprite(name) {
	var sprite = null;
	var match = /.+\*\d+\.png/.exec(name);
	if(match != null) {
		match = /\d+\.png/g.exec(name);
		
		var texName = name.substring(0, name.length - match[0].length - 1);
		var texCount = match[0].substring(0, match[0].length - 4);
		
		var frames = [];
		for(var i = 1; i <= texCount; i++) {
			var fileName = texName + "_" + i + ".png";
			var tex = loadTexture(fileName);
			frames.push(tex);
		}

		var sprite = new PIXI.MovieClip(frames);
		
		sprite.play();
		sprite.anchor.x = frames[0].anchor.x;
		sprite.anchor.y = frames[0].anchor.y;
		sprite.animationSpeed = frames[0].animationSpeed;
	} else {
		var tex = loadTexture(name);
		
		sprite = new PIXI.Sprite(tex);
		sprite.anchor.x = tex.anchor.x;
		sprite.anchor.y = tex.anchor.y;
	}
	
	return sprite;
}

function animate() {

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
	
	requestAnimFrame( animate );
}