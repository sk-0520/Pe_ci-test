
insert into
	Notes
	(
		NoteId,
		Title,
		ScreenName,
		LayoutKind,
		IsVisible,
		FontId,
		ForegdoundColor,
		BackgroundColor,
		IsLocked,
		IsCompact,
		TextWrap,
		ContentKind,

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
/* NoteId                */ @NoteId,
/* Title                 */ @Title,
/* ScreenName            */ @ScreenName,
/* LayoutKind            */ @LayoutKind,
/* IsVisible             */ @IsVisible,
/* FontId                */ @FontId,
/* ForegdoundColor       */ @ForegdoundColor,
/* BackgroundColor       */ @BackgroundColor,
/* IsLocked              */ @IsLocked,
/* IsCompact             */ @IsCompact,
/* TextWrap              */ @TextWrap,
/* ContentKind           */ @ContentKind,
/*                       */ 
/* CreatedTimestamp      */ @CreatedTimestamp,
/* CreatedAccount        */ @CreatedAccount,
/* CreatedProgramName    */ @CreatedProgramName,
/* CreatedProgramVersion */ @CreatedProgramVersion,
/* UpdatedTimestamp      */ @UpdatedTimestamp,
/* UpdatedAccount        */ @UpdatedAccount,
/* UpdatedProgramName    */ @UpdatedProgramName,
/* UpdatedProgramVersion */ @UpdatedProgramVersion,
/* UpdatedCount          */ 0
	)
