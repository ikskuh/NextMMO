
Level = { }

Level.initialize = function (level) {
	level.at = function (x,y) {
		if(level.data[x+";"+y] == null) {
			level.data[x+";"+y] = { x: x, y: y, height: 0, textureName: "" };
		}
		return level.data[x+";"+y];
	};
}

Level.create = function (width, height) {
	var level = { };
	level.width = width;
	level.height = height;
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