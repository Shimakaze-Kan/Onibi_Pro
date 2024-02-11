# Pre run
Write-Output "Pre run..."
$configFile = 'Onibi_Pro/appsettings.Development.json'
$jsonContent = Get-Content $configFile | ConvertFrom-Json

$connectionString = $jsonContent.ConnectionStrings.SqlServerConnection_McDowel

$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = @"
SELECT MAX(OrderTime) AS LatestOrderTime
FROM Orders
"@

$command = $connection.CreateCommand()
$command.CommandText = $query
$latestOrderTime = $command.ExecuteScalar()

# Test
Write-Output "Running tests..."
$env:PWTEST_SKIP_TEST_OUTPUT = 1
Push-Location -Path "Onibi_Pro/ClientApp"
npx playwright test --workers=1 --project=chromium
Pop-Location

# Cleanup
Write-Output "Cleaning up..."
$deleteOrdersQuery = @"
DELETE FROM Orders
WHERE OrderTime > '$latestOrderTime'
"@

$deleteOrdersCommand = $connection.CreateCommand()
$deleteOrdersCommand.CommandText = $deleteOrdersQuery
$deleteOrdersCommand.ExecuteNonQuery()

$deleteEmployeesCommand = $connection.CreateCommand()
$deleteEmployeesCommand.CommandText = "DELETE dbo.Employees WHERE Email LIKE '%@e2e.com'"
$deleteEmployeesCommand.ExecuteNonQuery()

$connection.Close()
Write-Output "Script execution completed successfully."