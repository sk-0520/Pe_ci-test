select
	LauncherItemIcons.Image
from
	LauncherItemIcons
where
	LauncherItemIcons.LauncherItemId = @LauncherItemId
	and
	LauncherItemIcons.IconScale = @IconScale

