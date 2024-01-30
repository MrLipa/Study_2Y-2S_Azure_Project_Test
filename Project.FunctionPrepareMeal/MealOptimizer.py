import numpy as np
import pygad


class MealOptimizer:
    def __init__(self, products, limits):
        self.products = products
        self.limits = limits
        self.ga_instance = self.create_ga_instance()

    def calculate_total_values(self, solution):
        total_calories = sum(solution[i] * self.products[i]['Calories'] / 100 for i in range(len(self.products)))
        total_protein = sum(solution[i] * self.products[i]['Protein'] / 100 for i in range(len(self.products)))
        total_fat = sum(solution[i] * self.products[i]['Fat'] / 100 for i in range(len(self.products)))
        total_carbs = sum(solution[i] * self.products[i]['Carbohydrates'] / 100 for i in range(len(self.products)))

        punishment = self.calculate_punishment(total_calories, total_protein, total_fat, total_carbs)
        return -punishment

    def calculate_punishment(self, total_calories, total_protein, total_fat, total_carbs):
        punishment = 0
        if total_carbs > self.limits['Carbohydrates']:
            punishment += total_carbs - self.limits['Carbohydrates']
        if total_fat > self.limits['Fat']:
            punishment += total_fat - self.limits['Fat']
        if total_protein < self.limits['Protein']:
            punishment += -total_protein + self.limits['Carbohydrates']
        if total_calories < self.limits['MinCalories']:
            punishment += -total_calories + self.limits['MinCalories']
        elif total_calories > self.limits['MaxCalories']:
            punishment += total_calories - self.limits['MaxCalories']
        return punishment
    
    def create_ga_instance(self):
        ga_instance = pygad.GA(num_generations=100,
                               num_parents_mating=2,
                               fitness_func=self.fitness_func,
                               sol_per_pop=10,
                               num_genes=len(self.products),
                               gene_space={'low': 0, 'high': 100},
                               parent_selection_type="sss",
                               keep_parents=1,
                               crossover_type="single_point",
                               mutation_type="random",
                               mutation_percent_genes=50)
        return ga_instance
    
    def fitness_func(self, ga_instance, solution, solution_idx):
        fitness = self.calculate_total_values(solution)
        return fitness

    def optimize_meal(self):
        self.ga_instance.fitness_func = self.fitness_func
        self.ga_instance.run()

        solution, solution_fitness, solution_idx = self.ga_instance.best_solution()
        products_quantities = self.get_product_quantities(solution)
        print()
        print("Ilości produktów (w gramach): ", solution)
        print("Ocena najlepszego rozwiązania : ", solution_fitness)

        total_calories, total_protein, total_fat, total_carbs = self.get_nutritional_values(solution)
        self.print_nutritional_info(total_calories, total_protein, total_fat, total_carbs)
        self.print_product_quantities(solution)

        return products_quantities
        
    def get_nutritional_values(self, solution):
        total_calories = sum(solution[i] * self.products[i]['Calories'] / 100 for i in range(len(self.products)))
        total_protein = sum(solution[i] * self.products[i]['Protein'] / 100 for i in range(len(self.products)))
        total_fat = sum(solution[i] * self.products[i]['Fat'] / 100 for i in range(len(self.products)))
        total_carbs = sum(solution[i] * self.products[i]['Carbohydrates'] / 100 for i in range(len(self.products)))
        return total_calories, total_protein, total_fat, total_carbs

    def print_nutritional_info(self, total_calories, total_protein, total_fat, total_carbs):
        print("\n\nSzczegółowe informacje o posiłku:")
        print(f"Kalorie: {total_calories:.2f}")
        print(f"Białko: {total_protein:.2f}g")
        print(f"Tłuszcz: {total_fat:.2f}g")
        print(f"Węglowodany: {total_carbs:.2f}g")
       
    def print_product_quantities(self, solution):
        print("\n\nRozkład produktów:")
        for i, quantity in enumerate(solution):
            if quantity > 0:
                product_name = self.products[i]['Name']
                print(f"{product_name}: {quantity:.2f}g")

    def get_product_quantities(self, solution):
        product_quantities = []
        for i, quantity in enumerate(solution):
            if quantity > 0:
                product_info = {
                    "ProductId": self.products[i]['ProductId'],
                    "Name": self.products[i]['Name'],
                    "QuantityInGrams": quantity
                }
                product_quantities.append(product_info)
        return product_quantities