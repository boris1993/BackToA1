$ErrorActionPreference = 'Stop'

$certPath = Join-Path $PSScriptRoot 'BackToA1_Signing.cer'

# Replace this with the thumbprint of your published certificate.
$expectedThumbprint = 'B68CBB359417EAB50C2133E9D6AB51277267D7A5'

try {
    if (-not (Test-Path -LiteralPath $certPath)) {
        throw "Certificate file not found: $certPath"
    }

    if ($expectedThumbprint -notmatch '^[A-Fa-f0-9 ]{40}$') {
        throw "Configure the expected certificate thumbprint in this script first."
    }

    $expectedThumbprint = $expectedThumbprint.Replace(' ', '').ToUpperInvariant()

    $cert = [System.Security.Cryptography.X509Certificates.X509Certificate2]::new(
        $certPath
    )

    $actualThumbprint = $cert.Thumbprint.ToUpperInvariant()

    if ($actualThumbprint -ne $expectedThumbprint) {
        throw "Certificate thumbprint mismatch. Do not install this certificate."
    }

    if ($cert.NotAfter -le (Get-Date)) {
        throw "The certificate has expired."
    }

    Write-Host ""
    Write-Host "BackToA1 certificate information" -ForegroundColor Cyan
    Write-Host "Subject:    $($cert.Subject)"
    Write-Host "Thumbprint: $actualThumbprint"
    Write-Host "Expires:    $($cert.NotAfter)"
    Write-Host ""

    Write-Warning @"
This is a self-signed certificate.
Installing it changes Windows trust settings for your user account.
Only continue if you have independently verified the fingerprint
published in the official BackToA1 repository.
"@

    $confirmation = Read-Host 'Type TRUST to continue'

    if ($confirmation -cne 'TRUST') {
        Write-Host 'Installation cancelled.'
        return
    }

    $stores = @(
        'Cert:\CurrentUser\Root',
        'Cert:\CurrentUser\TrustedPublisher'
    )

    foreach ($store in $stores) {
        $existing = Get-ChildItem -Path $store |
            Where-Object { $_.Thumbprint -eq $actualThumbprint }

        if ($existing) {
            Write-Host "Certificate already present: $store"
            continue
        }

        Import-Certificate `
            -FilePath $certPath `
            -CertStoreLocation $store `
            -ErrorAction Stop |
            Out-Null

        Write-Host "Imported certificate into: $store" -ForegroundColor Green
    }

    Write-Host ""
    Write-Host "Certificate trust setup completed." -ForegroundColor Green
    Write-Host "You can now run setup.exe."
}
catch {
    Write-Error $_
    exit 1
}
