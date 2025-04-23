Write-Host "=== Running SonarQube Scan ==="

# Define paths and project settings
$tfPath = "C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\"
$sonarScannerPath = "C:\Program Files\sonar-scanner-10.1.2.114627-net-framework\"
$msbuildPath = "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\"

$guid = [guid]::NewGuid().ToString()
$localPath = "C:\Temp\Sonar\$guid"
# Make sure the folder exists
New-Item -ItemType Directory -Path $localPath -Force | Out-Null

$collectionUrl = "http://tfs.mitrais.com:8080/tfs/Mitrais"
$tfsSourcePath = "$/CMS Enhancement/Development/CMS2025Q1/Source/"
$workspaceName = "SonarScanWorkspace"

$sonarHost = "http://localhost:9000"
$sonarToken = "sqp_2b3c5391c2d83a433eac326b1a4ff569193dc82f"
$projectKey = "MCompass---MITRAIS"
$slnName = "CRS.sln"
$slnFullPath = Join-Path $localPath $slnName

$sonarScanner =  Join-Path $sonarScannerPath "SonarScanner.MSBuild.exe"
$msbuild = Join-Path $msbuildPath "MSBuild.exe"
$tf = Join-Path $tfPath "tf.exe"

$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$logPath = "C:\Log\Sonar\SonarScan-$timestamp.log"

Write-Host "Checking sonarScannerPath path: $sonarScannerPath"
Write-Host "Checking MSBuild path: $msbuildPath"
Write-Host "Checking project path: $localPath"

# Validate required tools and paths
if (!(Test-Path $sonarScannerPath)) {
    Write-Error "sonarScannerPath not found at: $sonarScannerPath"
    exit 1
}

if (!(Test-Path $msbuildPath)) {
    Write-Error "MSBuild not found at: $msbuildPath"
    exit 1
}

# Begin logging
Start-Transcript -Path $logPath

# Check if workspace exists
$existingWs = & "$tf" workspaces /owner:* /collection:"$collectionUrl" | Select-String $workspaceName

if ($existingWs) {
    & "$tf" workspace /delete "$workspaceName" /collection:"$collectionUrl" /noprompt

    Write-Host "existingWs removed"
}

Set-Location "$localPath"
$currentDir = Get-Location
Write-Host "Current directory: $currentDir"

# Create a new workspace
& "$tf" workspace /new $workspaceName /collection:"$collectionUrl" /noprompt

# Map TFS folder to local
& "$tf" workfold /map "$tfsSourcePath" "$localPath" /workspace:$workspaceName /collection:"$collectionUrl" /noprompt

# Get latest code
& "$tf" get /recursive /noprompt

Write-Host "checkout completed"

# Run sonarScannerPath begin
& $sonarScanner begin `
  /k:"$projectKey" `
  /d:sonar.host.url="$sonarHost" `
  /d:sonar.token="$sonarToken"

# Build project using MSBuild (for .NET Framework)
& $msbuild $slnFullPath /t:Rebuild

# Run sonarScannerPath end
& $sonarScanner end /d:sonar.token="$sonarToken"

# Clean up the TFS workspace
Write-Host "Deleting TFS workspace '$workspaceName'..."
& "$tf" workspace /delete "$workspaceName" /collection:"$collectionUrl" /noprompt

# Clean up the local code folder
if (Test-Path $localPath) {
    Set-Location "C:\"  # move away from the folder
    Write-Host "Cleaning up local code directory $localPath..."
    Remove-Item -Path $localPath -Recurse -Force
}

# End logging
Stop-Transcript
