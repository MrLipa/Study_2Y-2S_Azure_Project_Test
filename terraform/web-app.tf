resource "random_string" "web_app_name" {
  length  = 8
  special = false
  upper   = false
}

# App Service Plan
resource "azurerm_service_plan" "web_app" {
  name                = "asp-web-app-${var.application_name}-${var.environment_name}"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  os_type             = "Windows"
  sku_name            = "S1"
}

# Web App
resource "azurerm_windows_web_app" "main" {
  name                = "wa-${var.application_name}-${var.environment_name}-${var.web_app_name}"
  # name                = "wa-${var.application_name}-${var.environment_name}-${random_string.web_app_name.result}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  service_plan_id     = azurerm_service_plan.web_app.id
  https_only          = true

  site_config {
    application_stack {
      current_stack  = "dotnet"
      dotnet_version = "v7.0"
    }
  }
  
  app_settings = {
    "ConnectionStrings__MyDbConnection" = "Server=tcp:${azurerm_mssql_server.main.name}.database.windows.net,1433;Initial Catalog=${azurerm_mssql_database.main.name};Persist Security Info=False;User ID=${var.sql_server_login};Password=${var.sql_server_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    "ExternalApi__BaseUrl" = "https://func-prepare-meal-${var.application_name}-${var.environment_name}-${var.function_name}.azurewebsites.net/api/optimize_meal",
    "EventGrid__TopicEndpoint" = "https://${azurerm_eventgrid_topic.main.name}.westeurope-1.eventgrid.azure.net/api/events",
    "EventGrid__TopicKey" = ""
  }
}

# Web App Slot
resource "azurerm_windows_web_app_slot" "devepopment" {
  name                = "wa-${var.application_name}-${var.environment_name}-${var.web_app_name}-deployment"
  app_service_id      = azurerm_windows_web_app.main.id

  site_config {
    application_stack {
      current_stack  = "dotnet"
      dotnet_version = "v7.0"
    }
  }

  app_settings = {
    "ConnectionStrings__MyDbConnection" = "Server=tcp:${azurerm_mssql_server.main.name}.database.windows.net,1433;Initial Catalog=${azurerm_mssql_database.main.name};Persist Security Info=False;User ID=${var.sql_server_login};Password=${var.sql_server_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    "ExternalApi__BaseUrl" = "https://func-prepare-meal-${var.application_name}-${var.environment_name}-${var.function_name}.azurewebsites.net/api/optimize_meal",
    "EventGrid__TopicEndpoint" = "https://${azurerm_eventgrid_topic.main.name}.westeurope-1.eventgrid.azure.net/api/events",
    "EventGrid__TopicKey" = "",
    "ASPNETCORE_ENVIRONMENT" = "Development"
  }

}