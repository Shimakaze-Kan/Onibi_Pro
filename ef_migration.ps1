$dateString = Get-Date -Format "yyyyMMddHHmmss"
$migrationName = "Migration_$dateString"
dotnet ef migrations add $migrationName -p .\Onibi_Pro.Infrastructure\ -s .\Onibi_Pro

dotnet ef database update -p .\Onibi_Pro.Infrastructure -s .\Onibi_Pro --connection "Server=localhost\MSSQLSERVER02;Database=Onibi_Pro;TrustServerCertificate=True;Integrated Security=true;"