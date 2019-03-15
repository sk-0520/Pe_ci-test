insert into
	LauncherToolbars
	(
		[LauncherToolbarId],
		[ScreenName],
		[ScreenX],
		[ScreenY],
		[ScreenWidth],
		[ScreenHeight],
		[LauncherGroupId],
		[PositionKind],
		[IconScale],
		[FontId],
		[AutoHideTimeout],
		[TextWidth],
		[IsVisible],
		[IsTopmost],
		[IsAutoHide],
		[IsIconOnly],

		[CreatedTimestamp],
		[CreatedAccount],
		[CreatedProgramName],
		[CreatedProgramVersion],
		[UpdatedTimestamp],
		[UpdatedAccount],
		[UpdatedProgramName],
		[UpdatedProgramVersion],
		[UpdatedCount]
	)
	values
	(
/* ToolbarId                */ @LauncherToolbarId,
/* ScreenName               */ @ScreenName,
/* ScreenX                  */ @ScreenX,
/* ScreenY                  */ @ScreenY,
/* ScreenWidth              */ @ScreenWidth,
/* ScreenHeight             */ @ScreenHeight,
/* LauncherGroupId          */ '00000000-0000-0000-0000-000000000000',
/* PositionKind             */ 'right',
/* IconScale                */ 'normal',
/* FontId                   */ '00000000-0000-0000-0000-000000000000',
/* AutoHideTimeout          */ '00:00:03',
/* TextWidth                */ 80,
/* IsVisible                */ 1,
/* IsTopmost                */ 1,
/* IsAutoHide               */ 0,
/* IsIconOnly               */ 0,

/*                          */
/* CreatedTimestamp         */ CURRENT_TIMESTAMP,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion,
/* UpdatedTimestamp         */ CURRENT_TIMESTAMP,
/* UpdatedAccount           */ @UpdatedAccount,
/* UpdatedProgramName       */ @UpdatedProgramName,
/* UpdatedProgramVersion    */ @UpdatedProgramVersion,
/* UpdatedCount             */ 0
	)
