select
	LauncherItems.Kind,
	LauncherItems.Command as CommandPath,
	0 as CommandIndex,
	LauncherItems.IconPath,
	LauncherItems.IconINdex
from
	LauncherItems
where
	LauncherItems.LauncherItemId = @LauncherItemId

