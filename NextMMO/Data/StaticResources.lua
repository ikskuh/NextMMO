
wellAnim = Resource:Animation("well-1", "WaterFalls", 4, 4)

Resource:Template(
	"well-1",
	Resource:CreateSprite(wellAnim, 48, 48, 2, 8.0),
	{ Resource:Rectangle(-48, -8, 96, 56) },
	function (self, other)
		Game:PlaySound("BitchSlap")
		if other ~= nil then
			if other.Y < self.Y then
				self:Translate(0, 0.2)
			else
				self:Translate(0, -0.2)
			end
		else
			self:Translate(0, 0.2)
		end
	end)