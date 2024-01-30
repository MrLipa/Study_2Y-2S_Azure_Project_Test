# Getting Started
- **Resource Visualizor:** ![rg-project-azure.png](rg-project-azure.png)







terraform init 
terraform validate
terraform apply -auto-approve

wejdz do servera networking i zaznacz allow
wejdz na database azure zaloguj sie Admin123 dodaj ip jeśli to potrzebne wklej sql
wejdz do funkcji 1 i 2 oraz web appa i zró deployment na githuba master
dostosuj plik githuba action według tego z other
event grid dodaj subskrypcje na funckje send email
dodaj do web appa event grid key
enable application ingisht na 4 aplikacjach 

dashborad pinuj pinestką metryki 
do metryuki dodaj add alert 

azure resource gorup Budgets zrób tam swój budget


ogarnąc logi
ogarnać metryki
ogarnąć alarmy

deployment sloty 


metryki na : request count, 
             time response avg
             http 200 400 500
             accumulated/forecasted costs
             dodac do dashboard



Remove-Migration InitialCreate
Add-Migration InitialCreate
Update-Database

dotnet build
dotnet tool install -g dotnet-ef
dotnet ef migrations remove
dotnet ef migrations add Migracja
dotnet ef database update
dotnet run seeddata

dotnet ef database update --project ./Project --connection "Server=tcp:project-sql-server-2137.database.windows.net,1433;Initial Catalog=project-sql-database;Persist Security Info=False;User ID=admin2137;Password=Admin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

netstat -ano | findstr :5147
taskkill /F /PID [PID]

pip install virtualenv
python -m venv env
env\Scripts\activate
pip install -r requirements.txt
func start
deactivate

ssh-keygen -t rsa -b 2048 -C "tomek.szkaradek1127@gmail.com"
cat ~/.ssh/id_rsa.pub

micorserwis 
rejestracja
logowanie
pobranie(wszystkie/konkretny) dodawanie usuwanie edycja usera
dodanie usuniecie posi�ku UserMeals zjedzonego

mikroserwis
pobranie(wszystkie/konkretny) dodawanie usuwanie edycja produkt�w

mikroserwis
pobranie(wszystkie/konkretny) dodawanie usuwanie edycja posi�k�w dodanie tranzakcji
Odpalenie Algorytmu Podajesz List(IDS) Limity Tworzy Posi�ek zapisuje do bazy danych

user dodaje do siebie posi�k�w ile chce 
wystielenie ile zjad� 
ustawnie limit�w
przy ka�dym posi�ku liczy i ostrzega
mo�e dodac sw�j albo poprosi alborytm o spersjonalizowanie
sworzenie posi�ku na podstaiw listy produkt�w 
odpalanie funckji po podaniu limity i odpalenie funkcji
zapisanie wyniku do hsitori 
wy�wietlenie histori

