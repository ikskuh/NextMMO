
Level = { }

Level.initialize = function (level) {
	level.at = function (x,y) {
		if(level.data[x+";"+y] == null) {
			level.data[x+";"+y] = { x: x, y: y, height: 0, textureName: "" };
		}
		return level.data[x+";"+y];
	};
}

Level.create = function (minX, maxX, minY, maxY) {
	var level = { };
	level.minX = minX;
	level.minY = minY;
	level.maxX = maxX;
	level.maxY = maxY;
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