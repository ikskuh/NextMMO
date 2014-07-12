#include <acknex.h>
#include <default.c>

var color;
var transparent;

BMAP *renderTargetMap = "#96x98x8888";

PANEL *panTarget = 
{
	bmap = renderTargetMap;
	pos_x = 32;
	pos_y = 32;
	scale_x = 2;
	scale_y = 2;
	flags = SHOW;
}

VIEW *isoCam = 
{
	left = -11.3;
	right = 11.5;
	top = 13.5;
	bottom = -19.5;
	pos_x = 0;
	pos_y = 0;
	size_x = 96;
	size_y = 98;
	layer = 2;
	bmap = renderTargetMap;
	flags = ISOMETRIC | SHOW;
}

function main(void)
{

	level_load(NULL);
	
	ENTITY *tile = ent_create("tile.mdl", vector(64, 0, 0), NULL);
	
	tile.pan = 135;
	
	isoCam.tilt = -45;
	isoCam.z = 64;
	
	sun_angle.pan = 150;
	sun_angle.tilt = 50;

	camera.x = -2;
	camera.y = -6;
	camera.z = 42;
	camera.pan = 5;
	camera.tilt = -32;
	
	color = pixel_for_vec(vector(255, 0, 255), 100, 8888);
	transparent = pixel_for_vec(COLOR_BLACK, 0, 8888);
	isoCam.bg = color;
	
	var f = file_open_read("input.txt");
	
	STRING *output = "#64";
	STRING *sides = "#64";
	STRING *top = "#64";
	
	wait(1);
	
	diag("\nStart reading file...");
	
	while(file_seek(f, 0, 4) < file_length(f))
	{
		file_str_readto(f, output, ";", 64);
		file_str_readto(f, sides, ";", 64);
		file_str_readto(f, top, ";", 64);
		
		diag("\nRead input:");
		diag(output);
		diag(", ");
		diag(sides);
		diag(", ");
		diag(top);

		ent_setskin(tile, bmap_to_mipmap(bmap_create(sides)), 1);
		ent_setskin(tile, bmap_to_mipmap(bmap_create(top)), 2);
		wait(1);
		isoCam.bg = transparent;
		wait(1); // Render frame
		isoCam.bg = color;
		bmap_to_format(renderTargetMap, 8888);
		bmap_save(renderTargetMap, output);
		beep();
		
		diag("\nSaved image to ");
		diag(output);
	}
	
	file_close(f);
	
	diag("\nAll done!");
	
	sys_exit("default exit");
}