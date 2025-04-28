Write-Host "=== Running SonarQube Scan ==="

# Define paths and project settings

try {
    $sonarToken = $env:SonarScanner_Token
    $tfsUser = "mitrais\andrya_A354"
    $tfsPassword = $env:SonarScanner_Tfs_Passwd
    $tfsLogin = "/login:$tfsUser,$tfsPassword"

    Write-Host "Checking env var..."
    Write-Host "tfsPassword: $tfsPassword"
    Write-Host "sonarToken: $sonarToken"

  
}
catch {
    Write-Error "An error occurred: $_"
    exit 1
}

