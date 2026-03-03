Write-Host "=== Atualizando Banco de Dados ===" -ForegroundColor Green

dotnet ef database update `
    --project src/Domus.Infrastructure `
    --startup-project src/Domus.Api

Write-Host "`n✅ Processo finalizado." -ForegroundColor Green