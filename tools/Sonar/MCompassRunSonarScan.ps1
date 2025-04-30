Write-Host "=== MCompass SonarQube Scheduler ==="

# Define paths and project settings
$tfPath = "C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\"
$sonarScannerPath = "C:\Program Files\sonar-scanner-10.1.2.114627-net-framework\"
$msbuildPath = "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\"

$guid = [guid]::NewGuid().ToString()
$localPath = "C:\Temp\Sonar\$guid"
New-Item -ItemType Directory -Path $localPath -Force | Out-Null

$collectionUrl = "http://tfs.mitrais.com:8080/tfs/Mitrais"
$tfsSourcePath = "$/CMS Enhancement/Development/CMS2025Q1/Source/"
$tfsToolsPath = "$/CMS Enhancement/Development/CMS2025Q1/Tools/"
$localSourcePath = Join-Path $localPath "Source"
$localToolsPath = Join-Path $localPath "Tools"
$workspaceName = "SonarScanWorkspace"

$sonarHost = "https://sonarqube.mitrais-dev.com"
$sonarToken = $env:SonarScanner_Token
$projectKey = "m-compass-backend"
$slnName = "CRS.sln"
$slnFullPath = Join-Path $localSourcePath $slnName

$sonarScanner = Join-Path $sonarScannerPath "SonarScanner.MSBuild.exe"
$msbuild = Join-Path $msbuildPath "MSBuild.exe"
$tf = Join-Path $tfPath "tf.exe"

$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$logPath = "C:\Log\Sonar\$timestamp-SonarScan.log"
$msbuildLogPath = "C:\Log\Sonar\$timestamp-MSBuildOutput.log"

try {
    # TFS login credentials (use env var for password)
    $tfsUser = "mitrais\andrya_A354"
    $tfsPassword = $env:SonarScanner_Tfs_Passwd
    $tfsLogin = "/login:$tfsUser,$tfsPassword"

    Write-Host "Checking paths..."
    Write-Host "SonarScanner: $sonarScannerPath"
    Write-Host "MSBuild: $msbuildPath"
    Write-Host "Local path: $localPath"

    # Validate required tools and paths
    if (!(Test-Path $sonarScannerPath)) { Write-Error "SonarScanner not found."; exit 1 }
    if (!(Test-Path $msbuildPath)) { Write-Error "MSBuild not found."; exit 1 }

    # Begin logging
    Start-Transcript -Path $logPath

    # Delete existing workspace
    $existingWs = & "$tf" workspaces /owner:* /collection:"$collectionUrl" | Select-String $workspaceName
    if ($existingWs) {
        & "$tf" workspace /delete "$workspaceName" /collection:"$collectionUrl" /noprompt $tfsLogin
        Write-Host "Existing workspace removed"
    }

    Set-Location "$localPath"
    Write-Host "Current directory: $(Get-Location)"

    # Create and map workspace
    & "$tf" workspace /new $workspaceName /collection:"$collectionUrl" /noprompt $tfsLogin
    & "$tf" workfold /map "$tfsSourcePath" "$localSourcePath" /workspace:$workspaceName /collection:"$collectionUrl" /noprompt $tfsLogin
    & "$tf" workfold /map "$tfsToolsPath" "$localToolsPath" /workspace:$workspaceName /collection:"$collectionUrl" /noprompt $tfsLogin

    # Get latest code
    & "$tf" get "$tfsSourcePath" /recursive /noprompt $tfsLogin
    & "$tf" get "$tfsToolsPath" /recursive /noprompt $tfsLogin
    Write-Host "Checkout completed"
    Write-Host ""  # Adds a blank line

    # Run SonarQube scan
    Write-Host "Running SonarQube Scan"
    $output = & $sonarScanner begin `
    "/k:$projectKey" `
    "/d:sonar.host.url=$sonarHost" `
    "/d:sonar.token=$sonarToken" `
    "/d:sonar.verbose=true"

    Write-Host $output

    & $msbuild $slnFullPath /t:Rebuild *> $msbuildLogPath
    if ($LASTEXITCODE -ne 0) {
        Write-Error "MSBuild failed with code $LASTEXITCODE. Check log: $msbuildLogPath"
        Stop-Transcript
        exit 1
    }

    & $sonarScanner end /d:sonar.token="$sonarToken"

    # Clean up
    Write-Host "Deleting workspace..."
    & "$tf" workspace /delete "$workspaceName" /collection:"$collectionUrl" /noprompt $tfsLogin

    if (Test-Path $localPath) {
        Set-Location "C:\"
        Write-Host "Cleaning up folder: $localPath"
        Remove-Item -Path $localPath -Recurse -Force
    }
}
catch {
    Write-Error "An error occurred: $_"
    Stop-Transcript
    exit 1
}

# End logging
Stop-Transcript
