resource "random_string" "function_name" {
  length  = 8
  special = false
  upper   = false
}

resource "random_string" "storage_account_name" {
  length  = 10
  special = false
  lower   = true
  upper   = false
}

resource "azurerm_storage_account" "function" {
  name                     = random_string.storage_account_name.result
  resource_group_name      = azurerm_resource_group.main.name
  location                 = azurerm_resource_group.main.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_service_plan" "function" {
  name                = "asp-func-${var.application_name}-${var.environment_name}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  os_type             = "Linux"
  sku_name            = "S1"
}

resource "azurerm_linux_function_app" "prepare_meal" {
  name                = "func-prepare-meal-${var.application_name}-${var.environment_name}-${var.function_name}"
  # name                = "func-${var.application_name}-${var.environment_name}-${random_string.function_name.result}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location

  storage_account_name       = azurerm_storage_account.function.name
  storage_account_access_key = azurerm_storage_account.function.primary_access_key
  service_plan_id            = azurerm_service_plan.function.id

  app_settings = {
    "ENABLE_ORYX_BUILD"              = "true"
    "SCM_DO_BUILD_DURING_DEPLOYMENT" = "true"
    "FUNCTIONS_WORKER_RUNTIME"       = "python"
    "AzureWebJobsFeatureFlags"       = "EnableWorkerIndexing"
  }

  site_config {
    application_stack {
      python_version = "3.11"
    }
  }
}

resource "azurerm_linux_function_app" "send_mail" {
  name                = "func-send-mail${var.application_name}-${var.environment_name}-${var.function_name}"
  # name                = "func-${var.application_name}-${var.environment_name}-${random_string.function_name.result}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location

  storage_account_name       = azurerm_storage_account.function.name
  storage_account_access_key = azurerm_storage_account.function.primary_access_key
  service_plan_id            = azurerm_service_plan.function.id

  app_settings = {
    "ENABLE_ORYX_BUILD"              = "true"
    "SCM_DO_BUILD_DURING_DEPLOYMENT" = "true"
    "FUNCTIONS_WORKER_RUNTIME"       = "python"
    "AzureWebJobsFeatureFlags"       = "EnableWorkerIndexing"
    "EMAIL_USERNAME"                 = "carlie44@ethereal.email"
    "EMAIL_PASSWORD"                 = "8fUAYpt3upPGcUwUEn"
  }

  site_config {
    application_stack {
      python_version = "3.11"
    }
  }
}
