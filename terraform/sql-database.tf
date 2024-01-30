# MYSQL Database
resource "azurerm_mssql_database" "main" {
  name      = "mysql-d-${var.application_name}-${var.environment_name}"
  server_id = azurerm_mssql_server.main.id
  sku_name  = "S0"

  tags = {
    environment = "Production"
  }
  lifecycle {
    prevent_destroy = true
  }
}
