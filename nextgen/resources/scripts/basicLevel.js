
Level = { }

Level.initialize = function (level) {
	level.at = function (x,y) {
		// Create if necessary
		if(level.data[x+";"+y] == null) {
			level.data[x+";"+y] = { x: x, y: y, height: 0, topTexture: "", groundTexture: "" };
		}
		// Adjust bounds
		level.minX = Math.min(level.minX, x);
		level.minY = Math.min(level.minY, y);
		level.maxX = Math.max(level.maxX, x);
		level.maxY = Math.max(level.maxY, y);
		
		return level.data[x+";"+y];
	};
}

Level.create = function () {
	var level = { };
	level.minX = 0;
	level.minY = 0;
	level.maxX = 0;
	level.maxY = 0;
	level.data = { };
	
	Level.initialize(level);
	return level;
}

Level.load = function (jsonText) {
	level = JSON.parse(jsonText);
	
	Level.initialize(level);
	return level;
}

Level.save = function (level) {
	return JSON.stringify(level);
}