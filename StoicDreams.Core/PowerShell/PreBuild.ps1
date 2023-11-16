# This pre-build process will update the current version number within this solution.

$rgxTargetGetVersion = '<Version>([0-9]+)\.([0-9]+)\.([0-9]+)</Version>'
Clear-Host;

while (Test-Path './StoicDreams.Core') {
	Set-Location './StoicDreams.Core'
}

if (!(Test-Path './PowerShell')) {
	Set-Location ..
}

if (!(Test-Path './StoicDreams.Core.csproj')) {
	throw "This script is expected to be run from the root of the StoicDreams.Core project."
}

$alphaversion = 1
$betaversion = 0
$rcversion = 0

Get-ChildItem -Path .\ -Filter *Core.csproj -Recurse -File | ForEach-Object {
	Write-Host $_
	$result = Select-String -Path $_.FullName -Pattern $rgxTargetGetVersion
	if ($result.Matches.Count -gt 0) {
		$alphaversion = [int]$result.Matches[0].Groups[1].Value
		$betaversion = [int]$result.Matches[0].Groups[2].Value
		$rcversion = [int]$result.Matches[0].Groups[3].Value
	}
}

function CheckIfAnyFilesUpdated {
	Param ()
	$folder = Get-Location
	Write-Host "Working Directory: $folder"
	$file = "$folder\lastupdate.txt"
	# check is not working consistently, bypassing until I have time to look into why it fails.
	return $true;
	if (Test-Path $file) {
		try {
			$lastupdate = Get-Content -Path $file
			$lastupdate = [DateTime]::ParseExact($lastupdate, 'M/dd/yyyy h:mm:ss tt', [cultureinfo]'en-US')
			Write-Host "Extracted last update: $lastupdate"
		}
		catch {
			Write-Host "Last update failed to load from file, defaulting to past 24 hours"
			$lastupdate = $(Get-Date).AddHours(-24)
		}
	}
 else {
		Write-Host "Last update file not found, defaulting to past 24 hours"
		$lastupdate = $(Get-Date).AddHours(-24)
	}
	$currentUpdate = $lastupdate;
	Write-Host "Loaded Last Update: $lastupdate"
	Get-Item "$folder\**\*.*" | where { $_.LastWriteTime -gt $lastupdate } | Foreach {
		# Ignore VS build folders
		if ($_.FullName.Contains("\bin\")) { return; }
		if ($_.FullName.Contains("\obj\")) { return; }
		$diff = $_.LastWriteTime - $lastupdate
		# Need to account for small milliseconds discrepency not saved to file
		if ($diff.TotalSeconds -lt 1) { return; }
		Write-Host �File Name: � + $_.FullName + "; Updated: " + $_.LastWriteTime
		if ($_.LastWriteTime -gt $currentUpdate) {
			$currentUpdate = $_.LastWriteTime
		}
	}

	if ($currentUpdate -gt $lastupdate) {
		Write-Host "Updating last update time to $currentUpdate"
		$currentUpdate | Set-Content -Path "$folder\lastupdate.txt"
		return $true
	}
	Write-Host "No updated files found."
	return $false
}

Write-Host "Release Version: [$($alphaversion)]_[$($betaversion)]_[$($rcversion)]";
if (CheckIfAnyFilesUpdated) {
	$rcversion = $rcversion + 1;
}

$version = "$alphaversion.$betaversion.$rcversion";
Write-Host "New Version: $($version)";

function UpdateProjectVersion {
	Param (
		[string] $projectPath,
		[string] $version
	)

	$rgxTargetXML = '<Version>([0-9\.]+)</Version>'
	$newXML = '<Version>' + $version + '</Version>'

	if (!(Test-Path -Path $projectPath)) {
		Write-Host "Not found - $projectPath" -BackgroundColor Red -ForegroundColor White
		return;
	}
	$content = Get-Content -Path $projectPath
	$oldMatch = $content -match $rgxTargetXML
	if ($oldMatch.Length -eq 0) {
		#Write-Host "Doesn't use Core - $projectPath"
		return;
	}
	$matches = $content -match $newXML
	if ($matches.Length -eq 1) {
		Write-Host "Already up to date - $projectPath" -ForegroundColor Cyan
		return;
	}
	$newContent = $content -replace $rgxTargetXML, $newXML
	$newContent | Set-Content -Path $projectPath
	Write-Host "Updated   - $projectPath" -ForegroundColor Green
}
Write-Host Version: $version
if ($version -ne $null) {
	$rootpath = Get-Location
	$rootpath = $rootpath.ToString().ToLower()

	while ($rootpath.Contains('Core')) {
		cd ..
		$rootpath = Get-Location
		$rootpath = $rootpath.ToString().ToLower()
		Write-Host $rootpath
	}

	Get-ChildItem -Path .\ -Filter *Core.csproj -Recurse -File | ForEach-Object {
		UpdateProjectVersion $_.FullName $version
	}
}
