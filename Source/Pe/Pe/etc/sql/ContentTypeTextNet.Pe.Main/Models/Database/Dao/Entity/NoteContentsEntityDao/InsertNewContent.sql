
insert into
	NoteContents
	(
		NoteId,
		Content,

		Address,
		Encoding,
		DelayTime,
		BufferSize,
		RefreshTime,
		IsEnabledRefresh,

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
/* Content               */ @Content,
/* Address               */ @Address,
/* Encoding              */ @Encoding,
/* DelayTime             */ @DelayTime,
/* BufferSize            */ @BufferSize,
/* RefreshTime           */ @RefreshTime,
/* IsEnabledRefresh      */ @IsEnabledRefresh,
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
