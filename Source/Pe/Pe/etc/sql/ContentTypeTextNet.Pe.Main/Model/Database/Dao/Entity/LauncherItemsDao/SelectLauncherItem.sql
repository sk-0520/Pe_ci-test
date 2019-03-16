select
	LauncherItems.LauncherItemId,
	LauncherItems.Name,
	LauncherItems.Code,
	LauncherItems.Kind,
	LauncherItems.IsEnabledCommandLauncher,
	LauncherItems.Note,
	LauncherItems.Command,
	LauncherItems.Option,
	LauncherItems.WorkDirectory
from
	LauncherItems
where
	LauncherItems.LauncherItemId = @LauncherItemId

