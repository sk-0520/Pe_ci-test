update
	AppNoteSetting
set
	FontId                = @FontId,
	TitleKind             = @TitleKind,
	LayoutKind            = @LayoutKind,
	ForegroundColor       = @ForegroundColor,
	BackgroundColor       = @BackgroundColor,
	IsTopmost             = @IsTopmost,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	AppNoteSetting.Generation = (
		select
			MAX(AppNoteSetting.Generation)
		from
			AppNoteSetting
	)