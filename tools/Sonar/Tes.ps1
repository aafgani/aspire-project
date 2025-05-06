Write-Host "=== Running SonarQube Scan ==="

# Define paths and project settings
$tfPath = "C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\"

$guid = [guid]::NewGuid().ToString()
$localPath = "C:\Temp\Sonar\$guid"
New-Item -ItemType Directory -Path $localPath -Force | Out-Null

$collectionUrl = "http://tfs.mitrais.com:8080/tfs/Mitrais"
$tfsUser = "mitrais\andrya_A354"
$tfsPassword ="123"
$tfsLogin = "/login:$tfsUser,$tfsPassword"

$tf = Join-Path $tfPath "tf.exe"

$timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
$logPath = "C:\Log\Sonar\Test-$timestamp.log"

try {
    Start-Transcript -Path $logPath

    Write-Host "Starting..."    

    Write-Host "Attempting TFS command with invalid login..."

    # Simulate a TFS command (like checking workspace info) to trigger auth error
    & $tf workspaces /collection:$collectionUrl $tfsLogin

    Write-Host "TFS command executed."

}
catch {
    Write-Error "An error occurred: $_"
    Stop-Transcript
    exit 1
}

# End logging
Stop-Transcript
