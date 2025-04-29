#clean up temp folders
Get-ChildItem -Path "C:\Temp\Sonar" -Directory |
    Where-Object { ($_.CreationTime -lt (Get-Date).AddDays(-3)) } |
    Remove-Item -Recurse -Force