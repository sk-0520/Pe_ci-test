﻿Param(
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

Import-Module "${PSScriptRoot}/Modules/Project"


$builToolDirPath = Join-Path -Path (Get-RootDirectory) -ChildPath 'Output' | Join-Path -ChildPath 'tools'

dotnet build Source/BuildTools/SqlPack/SqlPack.csproj --verbosity normal --configuration Debug /p:Platform=x64 --runtime win-x64 --output $builToolDirPath --no-self-contained
if (-not $?) {
	throw 'build error: Build Tool'
}

