var game = { };

function initializeGame() {
	game.listener = new window.keypress.Listener();
	game.resources = { };
	game.user = { loggedIn: false }
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
	
	initializeMetaData(function () {
		console.log("metadata loaded" , game.metadata);
		initializePIXI(function () {
			// Initialize everything when PIXI is
			// initialized and resources are loaded.
			initializePivot();
			intializeNetwork();
			initializeInput();
			initializeAudio();
			//initializeMusic();
		});
	});
}

function initializeMetaData(callback) {
	
	var metaData = [ "ground.png.json", "top.png.json" ];
	var counter = 0;
	
	game.metadata = { }
	
	function mergeInto(data) {
		game.metadata = deepmerge(game.metadata, data);
	}
	
	function load(url) {
		downloadFile(url, function (data) {
		
			var obj = JSON.parse(data);
			console.log(obj);
			
			mergeInto(obj);
			
			counter++;
			if(counter < metaData.length) {
				load(metaData[counter]);
			} else {
				callback();
			}
		});
	}
	
	load(metaData[0]);
	
}

function initializeAudio() {
	
	var soundCache = { }
	
	game.audio = { }
	game.audio.get = function (name, loop) {
		if(soundCache[name] != undefined) {
			return soundCache[name];
		}
			
		soundCache[name] = currentMusic = new Howl({
			urls: ["sounds/" + name + ".ogg", "sounds/" + name + ".mp3", "sounds/" + name + ".wav"],
			autoplay: false,
			loop: loop || false,
			volume: 1.0,
		});
		
		return soundCache[name];
	}
}

function initializeMusic() {
	
	var currentMusic = null;
	
	game.music = {}
	
	game.music.play = function (song) {
		
		if(currentMusic != null) {
			function fadeOld(music) {
				music.fade(0.4, 0.0, 1000, function (e) {
					music.stop();
					music.unload();
				});
			}
			fadeOld(currentMusic);
		}
		if(song != null) {
			currentMusic = new Howl({
				urls: ["music/" + song + ".ogg", "music/" + song + ".mp3", "music/" + song + ".wav"],
				autoplay: false,
				loop: true,
				//buffer: true,
				volume: 0.5,
			});
			
			currentMusic.play();
			currentMusic.fade(0.0, 0.4, 1000);
		} else {
			currentMusic = null;
		}
	}
	
	game.music.play("Crystal_Palace");
}

function initializeInput() {
	game.listener.simple_combo("enter", function() {
		if(game.user.loggedIn) {
			if(!isChatInputVisible()) {
				setChatInputVisible(true);
				document.getElementById("chatmsg").focus();
			} else {
				chat(document.getElementById('chatmsg').value);
				document.getElementById('chatmsg').value = "";
			}
		} else {
			
		}
	});
	game.listener.simple_combo("escape", function() {
		if(isChatInputVisible()) {
			setChatInputVisible(false);
		} else {
			// Other escape stuff
		}
	});
}

function login(name, password) {

	if(name === undefined) {
		name = document.getElementById("username").value;
		password = document.getElementById("password").value;
	} else {
		password = password || "";
	}
	
	game.user.name = name;
	game.socket.emit('login', {
		name: name,
		password: password
	});
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
	game.socket = io('http://localhost');
	
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
			player = createProxyPlayer(data.id);
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
		game.user.loggedIn = data.success;
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
	
	game.socket.on('chat', function (data) {
		chatLog(data.msg, data.sender);
	});
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


function chat(text) {
	text = text || "";
	if(text == "") return;
	game.socket.emit('chat', { msg: text });
	chatLog(text);
}

function chatLog(msg, sender) {
	sender = sender || game.user.name || "Me";
	var log = document.getElementById('chatlog');
	log.innerText += "\n" + sender + ": " + msg;
	log.scrollTop = log.scrollHeight;
	
	game.audio.get("chatclick").play();
}

/*
 * Taken from:
 * http://jaskokoyn.com/2013/07/24/external-json-file/
 */
function downloadFile(url, callback)
{
    var AJAX_req = new XMLHttpRequest();
    AJAX_req.open( "GET", url, true );
    AJAX_req.setRequestHeader("Content-type", "application/json");

    AJAX_req.onreadystatechange = function()
    {
        if( AJAX_req.readyState == 4 && AJAX_req.status == 200 )
        {
            callback(AJAX_req.responseText);
        }
    }
    AJAX_req.send();
}