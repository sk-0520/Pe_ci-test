﻿Param(
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$currentDirPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$scriptFileNames = @(
	'version.ps1'
);
foreach ($scriptFileName in $scriptFileNames) {
	$scriptFilePath = Join-Path $currentDirPath $scriptFileName
	. $scriptFilePath
}
$rootDirPath = Split-Path -Parent $currentDirPath
$outputDirectory = Join-Path $rootDirPath 'Output'

$rawChangelogsFile = Join-Path $rootDirPath "Source/Documents/source/script/changelogs.ts"
$rawChangelogLinkFile = Join-Path $rootDirPath "Source/Documents/source/script/changelog-link.js"
$templateHtmlFile = Join-Path $currentDirPath 'release-note.html'

# ノード作らず適当に
class Element {
	[string] $elementName;
	[Element[]] $children = @()
	[hashtable] $attributes = @{ }
	[string] $text

	Element([string] $elementName) {
		$this.elementName = $elementName
	}

	[Element] CreateChild([string] $elementName) {
		$elm = New-Object Element $elementName
		$this.children += $elm
		return $elm
	}

	[void] CreateText([string] $text) {
		$elm = New-Object Element ''
		$elm.text = $text
		$this.children += $elm
	}

	[string] Escape([string] $s) {
		$s = $s.Replace('&', '&amp;')
		$s = $s.Replace('<', '&lt;')
		$s = $s.Replace('>', '&gt;')
		$s = $s.Replace('"', '&quot;')
		$s = $s.Replace(' ', '&nbsp;')
		return $s
	}


	[string] ToHtml() {
		if ($this.elementName) {
			$html = @()
			if ( 0 -eq $this.attributes.Count) {
				$html += "<$($this.elementName)>"
			}
			else {
				$attrs = @()
				foreach ($key in $this.attributes.Keys) {
					$val = $this.attributes[$key]
					if ($val) {
						$attrs += "${key}=`"$($this.Escape(${val}))`""
					}
					else {
						$attrs += "${key}"
					}
				}

				$html += "<$($this.elementName) $($attrs -join ' ')>"
			}
			foreach ($child in $this.children) {
				$childHtml = $child.ToHtml()
				$html += $childHtml
			}

			$html += "</$($this.elementName)>"
			return $html -join ''
		}
		else {
			return $this.Escape($this.text)
		}
	}

}

$contentMap = @{
	'note'      = 'メモ'
	'features'  = '機能'
	'fixes'     = '修正'
	'developer' = '開発'
}

# 無理やりjsonにする
$rawChangelogsContent = Get-Content $rawChangelogsFile -Raw -Encoding UTF8
$headMark = '/*--------RELEASE HEAD--------*/'
$tailMark = '/*--------RELEASE TAIL--------*/'
$prevHeaderIndex = $rawChangelogsContent.IndexOf($headMark)
$prevHeaderContent = $rawChangelogsContent.Substring($prevHeaderIndex + $headMark.Length)

$tailIndex = $prevHeaderContent.IndexOf($tailMark);
$prevContent = $prevHeaderContent.Substring(0, $tailIndex)
$prevContent = $prevContent.Substring(0, $prevContent.LastIndexOf(';'))

$json = '[' + $prevContent.Substring($prevContent.IndexOf('{')) | ConvertFrom-Json

$currentVersion = $json[0]

# 速度とかどうでもいい
$body = New-Object Element 'body'
$headline = $body.CreateChild('h2') # changelogs.ts のスタイル流用のため
$headline.CreateText($currentVersion.version);
$headline.CreateText(': ');
$headline.CreateText($currentVersion.date);
$contents = $body.CreateChild('div');
$contents.attributes['id'] = 'content'
foreach ($content in $currentVersion.contents) {
	if (!($content.PSObject.Properties.Match('logs').Count)) {
		continue;
	}

	$section = $contents.CreateChild('section')
	$sectionHeader = $section.CreateChild('h3') # changelogs.ts のスタイル流用のため
	$sectionHeader.CreateText($contentMap[$content.type])
	$sectionHeader.attributes['class'] = $content.type
	$logs = $section.CreateChild('ul')
	foreach ($log in $content.logs) {
		$logItem = $logs.CreateChild('li')
		if ($log.PSObject.Properties.Match('class').Count) {
			$logItem.attributes['class'] = $log.class
		}

		$logHeader = $logItem.CreateChild('span')
		$logHeader.attributes['class'] = 'header'

		$logSubject = $logHeader.CreateChild('span')
		$logSubject.CreateText($log.subject)
		$logSubject.attributes['class'] = 'subject'

		if ($log.PSObject.Properties.Match('revision').Count) {
			$logRevision = $logHeader.CreateChild('a')
			$logRevision.CreateText($log.revision)
			$logRevision.attributes['class'] = 'revision'
		}


		if ($log.PSObject.Properties.Match('comments').Count) {
			$logComments = $logItem.CreateChild('ul')
			$logComments.attributes['class'] = 'comments'

			foreach ($comment in $log.comments) {
				$commentItem = $logComments.CreateChild('li')
				$commentItem.CreateText($comment)
			}
		}
	}

}

$htmlContent = (Get-Content $templateHtmlFile -Encoding UTF8 -Raw)
$htmlContent = $htmlContent.Replace('<body></body>', $body.ToHtml())
$htmlContent = $htmlContent.Replace('//SCRIPT', (Get-Content $rawChangelogLinkFile -Raw -Encoding UTF8))

$version = GetAppVersion
Set-Content (Join-Path $outputDirectory (ConvertReleaseNoteFileName $version)) -Value $htmlContent -Encoding UTF8
