Param(
	[Parameter(mandatory = $true)][string] $AssemblyDirectory,
	[switch] $ProductMode
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'project.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}

#/*[FUNCTIONS]-------------------------------------
#*/[FUNCTIONS]-------------------------------------

Write-Output $AssemblyDirectory
Write-Output $ProductMode

if ($ProductMode) {
	$removeTargets = @(
		Get-ChildItem -Path $AssemblyDirectory -File -Filter "*.ilk" -Recurse
		Get-ChildItem -Path $AssemblyDirectory -File -Filter "*.iobj" -Recurse
		Get-ChildItem -Path $AssemblyDirectory -File -Filter "*.ipdb" -Recurse
		Get-ChildItem -Path $AssemblyDirectory -File -Filter "*.pdb" -Recurse
	) | Select-Object -ExpandProperty FullName

	foreach ($removeTarget in $removeTargets) {
		Write-Output "remove: $removeTarget"
		Remove-Item $removeTarget -Recurse -Force
	}
}
