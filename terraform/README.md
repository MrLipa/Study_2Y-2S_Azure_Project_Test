# Getting Started

## Sprawdzenie wersji Azure CLI i aktualizacja
az --version  
az upgrade  

## Logowanie do Azure
az login # Tutaj zaloguj się za pomocą swoich credentiale  

## Konfiguracja Azure CLI
az config set core.allow_broker=true  
az account clear  
az login # Ponowne logowanie, jeśli jest to konieczne  

## Operacje Terraform
terraform --version # Sprawdzenie wersji Terraform  
terraform init      # Inicjalizacja Terraform  
terraform fmt       # Formatowanie kodu Terraform  
terraform validate  # Walidacja kodu Terraform  
terraform plan      # Planowanie zmian infrastruktury  
terraform apply     # Zastosowanie zmian infrastruktury  
terraform apply -auto-approve # Automatyczne zastosowanie zmian  
terraform destroy   # Usunięcie infrastruktury
