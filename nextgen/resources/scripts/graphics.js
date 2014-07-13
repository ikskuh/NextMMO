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