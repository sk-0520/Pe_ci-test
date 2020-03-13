﻿insert into
	LauncherItemIcons
	(
		[LauncherItemId],
		[IconBox],
		[Sequence],
		[Image],

		[CreatedTimestamp],
		[CreatedAccount],
		[CreatedProgramName],
		[CreatedProgramVersion]
	)
	values
	(
/* LauncherItemId           */ @LauncherItemId,
/* IconBox                  */ @IconBox,
/* Sequence                 */ @Sequence,
/* Image                    */ @Image,

/* CreatedTimestamp         */ CURRENT_TIMESTAMP,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion
	)