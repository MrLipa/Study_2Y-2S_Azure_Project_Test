IF OBJECT_ID('dbo.UserMeals', 'U') IS NOT NULL
    DROP TABLE dbo.UserMeals;
IF OBJECT_ID('dbo.MealProducts', 'U') IS NOT NULL
    DROP TABLE dbo.MealProducts;
IF OBJECT_ID('dbo.AppUsers', 'U') IS NOT NULL
    DROP TABLE dbo.AppUsers;
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL
    DROP TABLE dbo.Products;
IF OBJECT_ID('dbo.Meals', 'U') IS NOT NULL
    DROP TABLE dbo.Meals;

CREATE TABLE dbo.AppUsers (
    UserId INT IDENTITY PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME NOT NULL,
    DailyCalorieGoal FLOAT NULL,
    DailyProteinGoal FLOAT NULL,
    DailyFatGoal FLOAT NULL,
    DailyCarbohydratesGoal FLOAT NULL,
    RefreshToken NVARCHAR(1000) NULL
);

CREATE TABLE dbo.Products (
    ProductId INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Calories FLOAT NOT NULL,
    Protein FLOAT NOT NULL,
    Fat FLOAT NOT NULL,
    Carbohydrates FLOAT NOT NULL
);

CREATE TABLE dbo.Meals (
    MealId INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE dbo.MealProducts (
    MealProductId INT IDENTITY PRIMARY KEY,
    MealId INT NOT NULL,
    ProductId INT NOT NULL,
    QuantityInGrams FLOAT NOT NULL,
    FOREIGN KEY (MealId) REFERENCES Meals(MealId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

CREATE TABLE dbo.UserMeals (
    UserMealId INT IDENTITY PRIMARY KEY,
    UserId INT NOT NULL,
    MealId INT NOT NULL,
    ConsumedAt DATETIME NOT NULL,
    FOREIGN KEY (UserId) REFERENCES AppUsers(UserId),
    FOREIGN KEY (MealId) REFERENCES Meals(MealId)
);

INSERT INTO dbo.AppUsers (Username, PasswordHash, Email, CreatedAt, DailyCalorieGoal, DailyProteinGoal, DailyFatGoal, DailyCarbohydratesGoal)
VALUES 
    ('admin', '4H7QJp7SR2b6mvXKZu', 'margaretta.boehm@ethereal.email', GETUTCDATE(), 2500, 150, 70, 300),
    ('user', 'x5MHGvu2vBbVjmxMXu', 'shaniya30@ethereal.email', GETUTCDATE(), 2000, 120, 50, 250);

INSERT INTO dbo.Products (Name, Calories, Protein, Fat, Carbohydrates)
VALUES 
    ('Mleko', 42, 3.4, 1, 4.8),
    ('Chleb', 265, 9, 3.2, 49),
    ('Ser żółty', 402, 25, 33, 1.3),
    ('Makaron', 131, 5, 1.1, 25),
    ('Pomidor', 18, 0.9, 0.2, 3.9),
    ('Cebula', 40, 1.1, 0.1, 9.3),
    ('Jajko', 155, 13, 11, 1.1),
    ('Szynka', 145, 24, 6, 0),
    ('Oliwa z oliwek', 884, 0, 100, 0),
    ('Boczek', 541, 37, 42, 1.4),
    ('Mąka', 364, 10, 1, 76),
    ('Ricotta', 174, 11, 13, 3),
    ('Szpinak', 23, 2.9, 0.4, 3.6),
    ('Pieczarki', 22, 3.1, 0.3, 3.3),
    ('Papryka', 31, 1, 0.3, 6);

INSERT INTO dbo.Meals (Name)
VALUES 
    ('Spaghetti'),
    ('Kanapka'),
    ('Jajecznica'),
    ('Ravioli');

INSERT INTO dbo.MealProducts (MealId, ProductId, QuantityInGrams)
VALUES 
    (1, 4, 100),  -- Makaron dla spaghetti
    (1, 5, 50),   -- Pomidor dla spaghetti
    (1, 6, 30),   -- Cebula dla spaghetti
    (2, 2, 50),   -- Chleb dla kanapki
    (2, 8, 30),   -- Szynka dla kanapki
    (2, 3, 20),   -- Ser ��ty dla kanapki
    (3, 7, 100),  -- Jajko dla jajecznicy
    (3, 10, 30),  -- Boczek dla jajecznicy
    (4, 11, 100), -- M�ka dla ravioli
    (4, 12, 50),  -- Ricotta dla ravioli
    (4, 13, 30);  -- Szpinak dla ravioli

INSERT INTO dbo.UserMeals (UserId, MealId, ConsumedAt)
VALUES 
    (1, 1, GETDATE()), -- Spaghetti dla admina
    (1, 2, GETDATE()), -- Kanapka dla admina
    (2, 3, GETDATE()), -- Jajecznica dla usera
    (2, 4, GETDATE()); -- Ravioli dla usera


SELECT * FROM dbo.AppUsers;