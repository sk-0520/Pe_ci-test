﻿select
	count(*) = 1
from
	NoteLayouts
where
	NoteLayouts.NoteId = @NoteId
	and
	NoteLayouts.LayoutKind = @LayoutKind;

