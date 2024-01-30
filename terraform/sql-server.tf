# MYSQL Server
resource "azurerm_mssql_server" "main" {
  name                         = "sql-s-${var.application_name}-${var.environment_name}"
  location                     = azurerm_resource_group.main.location
  resource_group_name          = azurerm_resource_group.main.name
  version                      = "12.0"
  administrator_login          = var.sql_server_login
  administrator_login_password = var.sql_server_password

  tags = {
    environment = "Production"
  }
}
