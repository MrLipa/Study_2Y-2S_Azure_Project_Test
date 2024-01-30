resource "azurerm_eventgrid_topic" "main" {
  name                = "egt-${var.application_name}-${var.environment_name}"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
}