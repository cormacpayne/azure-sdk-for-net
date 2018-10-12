Add-Type -Path Microsoft.Azure.Test.dll
$cache = New-Object Microsoft.Azure.Test.Authentication.CredManCache "SpecTestSupport"
$credential = Get-Credential
$cache[$credential.UserName] = $credential.GetNetworkCredential().Password