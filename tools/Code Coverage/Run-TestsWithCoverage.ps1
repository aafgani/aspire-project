# Resolve the directory where the script lives
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
# Resolve the root directory (assuming it's two levels up from tools/Code Coverage)
$RootDir = Resolve-Path (Join-Path $ScriptDir "../..")

# === CONFIGURE YOUR PROJECT INFO HERE ===
$SonarProjectKey = "my-dotnet-app"
$SonarToken = "sqp_8bba8fd774830e7454142f5e90b10f4a080439f4"
$SonarHostUrl = "http://localhost:9000"
$SlnPath = Join-Path $RootDir "aspire-project.sln"
$UnitTestProj = Join-Path $RootDir "test/Test.UniTest.Business/Test.UnitTest.Business.csproj"
$IntegrationTestProj = Join-Path $RootDir "test/Test.IntegrationTest.Api/Test.IntegrationTest.Api.csproj"
$CoverageDir = "$RootDir/coverage"
$MergedCoverageFile = "$CoverageDir/integration/coverage.opencover.xml"
$HtmlReportDir = "$CoverageDir/report"
# ========================================

Write-Host "Cleaning previous coverage results..."
Remove-Item -Recurse -Force $CoverageDir -ErrorAction SilentlyContinue


Write-Host "Running unit tests with coverage..."
dotnet test $UnitTestProj `
  /p:CollectCoverage=true `
  /p:CoverletOutputFormat=opencover `
  /p:CoverletOutputFormatVersion=1.0 `
  /p:CoverletOutput="$CoverageDir/unit/"

Write-Host "Running integration tests with merged coverage..."
  dotnet test  $IntegrationTestProj `
  /p:CollectCoverage=true `
  /p:CoverletOutputFormat=opencover `
  /p:CoverletOutputFormatVersion=1.0 `
  /p:CoverletOutput="$CoverageDir/integration/" `
  /p:MergeWith="$CoverageDir/unit/coverage.json"

Write-Host "Coberture path : " $MergedCoverageFile

# Write-Host "Running SonarScanner begin..."
dotnet sonarscanner begin `
  /k:$SonarProjectKey `
  /d:sonar.token=$SonarToken `
  /d:sonar.host.url=$SonarHostUrl `
  /d:sonar.scanner.scanAll=false `
  /d:sonar.cs.opencover.reportsPaths="$MergedCoverageFile" `
  /d:sonar.coverage.exclusions="**Program.cs"
  # /d:sonar.coverageReportPaths=$MergedCoverageFile `

# Write-Host "Building the project..."
dotnet build $SlnPath

# Write-Host "Running SonarScanner end..."
dotnet sonarscanner end /d:sonar.login=$SonarToken

Write-Host "Generating HTML report locally..."
reportgenerator `
  -reports:$MergedCoverageFile `
  -targetdir:$HtmlReportDir `
  -reporttypes:Html

# Write-Host "Done! View your HTML report at: $HtmlReportDir/index.html"
Write-Host "Done!"