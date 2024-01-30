using Project.Data;
using Project.Models;


namespace Project
{
    public class Seed
    {
        private readonly DataContext dataContext;

        public Seed(DataContext context)
        {
            this.dataContext = context;
        }

        private void ClearDatabase()
        {
            dataContext.UserMeals.RemoveRange(dataContext.UserMeals);
            dataContext.MealProducts.RemoveRange(dataContext.MealProducts);
            dataContext.Meals.RemoveRange(dataContext.Meals);
            dataContext.Products.RemoveRange(dataContext.Products);
            dataContext.AppUsers.RemoveRange(dataContext.AppUsers);
            dataContext.SaveChanges();
        }

        public void SeedDataContext()
        {
            ClearDatabase();

            var appUsers = new List<AppUser>
            {
                new AppUser { Username = "admin", PasswordHash = "4H7QJp7SR2b6mvXKZu", Email = "margaretta.boehm@ethereal.email", CreatedAt = DateTime.UtcNow, DailyCalorieGoal = 2500f, DailyProteinGoal = 150f, DailyFatGoal = 70f, DailyCarbohydratesGoal = 300f },
                new AppUser { Username = "user", PasswordHash = "x5MHGvu2vBbVjmxMXu", Email = "shaniya30@ethereal.email", CreatedAt = DateTime.UtcNow, DailyCalorieGoal = 2000f, DailyProteinGoal = 120f, DailyFatGoal = 50f, DailyCarbohydratesGoal = 250f }
            };
            dataContext.AppUsers.AddRange(appUsers);
            dataContext.SaveChanges();

            var products = new List<Product>
            {
                new Product { Name = "Mleko", Calories = 42f, Protein = 3.4f, Fat = 1f, Carbohydrates = 4.8f },
                new Product { Name = "Chleb", Calories = 265f, Protein = 9f, Fat = 3.2f, Carbohydrates = 49f },
                new Product { Name = "Ser żółty", Calories = 402f, Protein = 25f, Fat = 33f, Carbohydrates = 1.3f },
                new Product { Name = "Makaron", Calories = 131f, Protein = 5f, Fat = 1.1f, Carbohydrates = 25f },
                new Product { Name = "Pomidor", Calories = 18f, Protein = 0.9f, Fat = 0.2f, Carbohydrates = 3.9f },
                new Product { Name = "Cebula", Calories = 40f, Protein = 1.1f, Fat = 0.1f, Carbohydrates = 9.3f },
                new Product { Name = "Jajko", Calories = 155f, Protein = 13f, Fat = 11f, Carbohydrates = 1.1f },
                new Product { Name = "Szynka", Calories = 145f, Protein = 24f, Fat = 6f, Carbohydrates = 0f },
                new Product { Name = "Oliwa z oliwek", Calories = 884f, Protein = 0f, Fat = 100f, Carbohydrates = 0f },
                new Product { Name = "Boczek", Calories = 541f, Protein = 37f, Fat = 42f, Carbohydrates = 1.4f },
                new Product { Name = "Mąka", Calories = 364f, Protein = 10f, Fat = 1f, Carbohydrates = 76f },
                new Product { Name = "Ricotta", Calories = 174f, Protein = 11f, Fat = 13f, Carbohydrates = 3f },
                new Product { Name = "Szpinak", Calories = 23f, Protein = 2.9f, Fat = 0.4f, Carbohydrates = 3.6f },
                new Product { Name = "Pieczarki", Calories = 22f, Protein = 3.1f, Fat = 0.3f, Carbohydrates = 3.3f },
                new Product { Name = "Papryka", Calories = 31f, Protein = 1f, Fat = 0.3f, Carbohydrates = 6f }
            };

            dataContext.Products.AddRange(products);
            dataContext.SaveChanges();

            var meals = new List<Meal>
            {
                new Meal { Name = "Spaghetti" },
                new Meal { Name = "Kanapka" },
                new Meal { Name = "Jajecznica" },
                new Meal { Name = "Ravioli" }
            };
            dataContext.Meals.AddRange(meals);
            dataContext.SaveChanges();

            var mealProducts = new List<MealProduct>
            {
                // Spaghetti
                new MealProduct { MealId = meals[0].MealId, ProductId = products[3].ProductId, QuantityInGrams = 100 }, // Makaron
                new MealProduct { MealId = meals[0].MealId, ProductId = products[4].ProductId, QuantityInGrams = 50 },  // Pomidor
                new MealProduct { MealId = meals[0].MealId, ProductId = products[5].ProductId, QuantityInGrams = 30 },  // Cebula

                // Kanapka
                new MealProduct { MealId = meals[1].MealId, ProductId = products[1].ProductId, QuantityInGrams = 50 },  // Chleb
                new MealProduct { MealId = meals[1].MealId, ProductId = products[7].ProductId, QuantityInGrams = 30 },  // Szynka
                new MealProduct { MealId = meals[1].MealId, ProductId = products[2].ProductId, QuantityInGrams = 20 },  // Ser żółty

                // Jajecznica
                new MealProduct { MealId = meals[2].MealId, ProductId = products[6].ProductId, QuantityInGrams = 100 }, // Jajko
                new MealProduct { MealId = meals[2].MealId, ProductId = products[9].ProductId, QuantityInGrams = 30 },  // Boczek

                // Ravioli
                new MealProduct { MealId = meals[3].MealId, ProductId = products[10].ProductId, QuantityInGrams = 100 },// Mąka
                new MealProduct { MealId = meals[3].MealId, ProductId = products[11].ProductId, QuantityInGrams = 50 }, // Ricotta
                new MealProduct { MealId = meals[3].MealId, ProductId = products[12].ProductId, QuantityInGrams = 30 }  // Szpinak
            };

            dataContext.MealProducts.AddRange(mealProducts);
            dataContext.SaveChanges();

            var userMeals = new List<UserMeal>
            {
                new UserMeal { UserId = appUsers[0].UserId, MealId = meals[0].MealId, ConsumedAt = DateTime.Now }, // Spaghetti dla admina
                new UserMeal { UserId = appUsers[0].UserId, MealId = meals[1].MealId, ConsumedAt = DateTime.Now }, // Kanapka dla admina
                new UserMeal { UserId = appUsers[1].UserId, MealId = meals[2].MealId, ConsumedAt = DateTime.Now }, // Jajecznica dla usera
                new UserMeal { UserId = appUsers[1].UserId, MealId = meals[3].MealId, ConsumedAt = DateTime.Now }  // Ravioli dla usera
            };
            dataContext.UserMeals.AddRange(userMeals);
            dataContext.SaveChanges();
        }
    }
}
