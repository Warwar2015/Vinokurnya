using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VinokurnyaWpf.Data;

namespace VinokurnyaWpf.Services
{
    public class DataService
    {
        private readonly AppDbContext _dbContext;

        public DataService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Recipe operations
        public Task<List<Recipe>> GetAllRecipesAsync()
        {
            return _dbContext.Recipes.ToListAsync();
        }

        public Task<Recipe?> GetRecipeByIdAsync(Guid id)
        {
            return _dbContext.Recipes.FindAsync(id).AsTask();
        }

        public Task<List<Recipe>> GetRecipesByCategoryAsync(string category)
        {
            return _dbContext.Recipes
                .Where(r => r.Category == category)
                .OrderByDescending(r => r.Rating)
                .ToListAsync();
        }

        public Task<List<Recipe>> GetRecipesBySubcategoryAsync(string subcategory)
        {
            return _dbContext.Recipes
                .Where(r => r.Subcategory == subcategory)
                .OrderByDescending(r => r.Rating)
                .ToListAsync();
        }

        public Task<List<Recipe>> GetFavoriteRecipesAsync()
        {
            return _dbContext.Recipes
                .Where(r => r.IsFavorite)
                .OrderByDescending(r => r.Rating)
                .ToListAsync();
        }

        public Task<List<Recipe>> SearchRecipesAsync(string query)
        {
            // Параметризованный поиск для предотвращения SQL Injection
            if (string.IsNullOrWhiteSpace(query))
            {
                return _dbContext.Recipes
                    .OrderByDescending(r => r.Rating)
                    .ToListAsync();
            }

            return _dbContext.Recipes
                .Where(r => EF.Functions.Like(r.Name.ToLower(), $"%{query.ToLower()}%") ||
                           EF.Functions.Like(r.Category.ToLower(), $"%{query.ToLower()}%") ||
                           EF.Functions.Like(r.Subcategory.ToLower(), $"%{query.ToLower()}%") ||
                           EF.Functions.Like(r.Notes.ToLower(), $"%{query.ToLower()}%"))
                .OrderByDescending(r => r.Rating)
                .ToListAsync();
        }

        public Task AddRecipeAsync(Recipe recipe)
        {
            // Используем TransactionScope для атомарного сохранения
            return Task.Run(() =>
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                try
                {
                    recipe.CreatedAt = DateTime.UtcNow;
                    recipe.UpdatedAt = DateTime.UtcNow;
                    _dbContext.Recipes.Add(recipe);

                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Ошибка при сохранении рецепта: {ex.Message}", ex);
                }
            });
        }

        public Task UpdateRecipeAsync(Recipe recipe)
        {
            // Используем TransactionScope для атомарного сохранения
            return Task.Run(() =>
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                try
                {
                    recipe.UpdatedAt = DateTime.UtcNow;
                    _dbContext.Recipes.Update(recipe);

                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Ошибка при обновлении рецепта: {ex.Message}", ex);
                }
            });
        }

        public Task DeleteRecipeAsync(Guid id)
        {
            return Task.Run(() =>
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                try
                {
                    var recipe = _dbContext.Recipes.Find(id);
                    if (recipe != null)
                    {
                        _dbContext.Recipes.Remove(recipe);
                        _dbContext.SaveChanges();
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Ошибка при удалении рецепта: {ex.Message}", ex);
                }
            });
        }

        public Task<bool> ToggleFavoriteAsync(Guid id)
        {
            return Task.Run(() =>
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                try
                {
                    var recipe = _dbContext.Recipes.Find(id);
                    if (recipe == null)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    recipe.IsFavorite = !recipe.IsFavorite;
                    recipe.UpdatedAt = DateTime.UtcNow;
                    _dbContext.Recipes.Update(recipe);

                    _dbContext.SaveChanges();
                    transaction.Commit();

                    return recipe.IsFavorite;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Ошибка при переключении избранного: {ex.Message}", ex);
                }
            });
        }

        // Note operations
        public Task<List<Note>> GetAllNotesAsync()
        {
            return _dbContext.Notes
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public Task<List<Note>> GetNotesByStageAsync(ProcessStage stage)
        {
            return _dbContext.Notes
                .Where(n => n.Stage == stage)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public Task<List<Note>> GetFavoriteNotesAsync()
        {
            return _dbContext.Notes
                .Where(n => n.IsFavorite)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public Task<Note?> GetNoteByIdAsync(Guid id)
        {
            return _dbContext.Notes.FindAsync(id).AsTask();
        }

        public Task AddNoteAsync(Note note)
        {
            return Task.Run(() =>
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                try
                {
                    note.CreatedAt = DateTime.UtcNow;
                    note.UpdatedAt = DateTime.UtcNow;
                    _dbContext.Notes.Add(note);

                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Ошибка при сохранении заметки: {ex.Message}", ex);
                }
            });
        }

        public Task UpdateNoteAsync(Note note)
        {
            return Task.Run(() =>
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                try
                {
                    note.UpdatedAt = DateTime.UtcNow;
                    _dbContext.Notes.Update(note);

                    _dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Ошибка при обновлении заметки: {ex.Message}", ex);
                }
            });
        }

        public Task<bool> DeleteNoteAsync(Guid id)
        {
            return Task.Run(() =>
            {
                using var transaction = _dbContext.Database.BeginTransaction();

                try
                {
                    var note = _dbContext.Notes.Find(id);
                    if (note != null)
                    {
                        _dbContext.Notes.Remove(note);
                        _dbContext.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Ошибка при удалении заметки: {ex.Message}", ex);
                }
            });
        }

        // Calculation history
        public Task<List<CalculationHistory>> GetCalculationHistoryAsync(int limit = 50)
        {
            return _dbContext.CalculationHistory
                .OrderByDescending(c => c.CalculatedAt)
                .Take(limit)
                .ToListAsync();
        }

        public Task AddCalculationHistoryAsync(CalculationHistory calculation)
        {
            _dbContext.CalculationHistory.Add(calculation);
            return _dbContext.SaveChangesAsync();
        }

        // Sample data
        public Task EnsureSampleDataAsync()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var hasRecipes = await _dbContext.Recipes.AnyAsync();
                    var hasNotes = await _dbContext.Notes.AnyAsync();

                    if (hasRecipes && hasNotes)
                        return;

                    await SeedSampleDataAsync();
                }
                catch
                {
                    throw;
                }
            });
        }

        private async Task SeedSampleDataAsync()
        {
            var recipes = new List<Recipe>
            {
                new Recipe
                {
                    Name = "Классический односолодовый виски",
                    Category = "Виски",
                    Subcategory = "Single Malt",
                    Difficulty = 4,
                    TimeDays = 14,
                    YieldLiters = 3.5,
                    Rating = 4.8,
                    RatingCount = 45,
                    IsFavorite = true,
                    IngredientsJson = "[{\"Name\":\"Ячмень\",\"Quantity\":5,\"Unit\":\"кг\",\"IsChecked\":false},{\"Name\":\"Вода\",\"Quantity\":15,\"Unit\":\"л\",\"IsChecked\":false},{\"Name\":\"Хмель\",\"Quantity\":0.2,\"Unit\":\"кг\",\"IsChecked\":false},{\"Name\":\"Дрожжи\",\"Quantity\":0.05,\"Unit\":\"г\",\"IsChecked\":false}]",
                    StepsJson = "[{\"Order\":1,\"Description\":\"Затирание – 180 минут при 65°C\",\"DurationMinutes\":180,\"Temperature\":65},{\"Order\":2,\"Description\":\"Ферментация – 7 дней при 22°C\",\"DurationMinutes\":10080,\"Temperature\":22},{\"Order\":3,\"Description\":\"Перегонка – первый проход\",\"Temperature\":78}]",
                    DistillationParametersJson = "{\"InitialABV\":25,\"TargetABV\":65,\"HeadsPercentage\":12,\"HeartsPercentage\":78,\"TailsPercentage\":10,\"Temperature\":78,\"AtmosphericPressure\":1013}",
                    Notes = "Классический рецепт односолодового виски"
                },
                new Recipe
                {
                    Name = "Бюджетный бурбон",
                    Category = "Бурбон",
                    Subcategory = "Bourbon",
                    Difficulty = 3,
                    TimeDays = 10,
                    YieldLiters = 4.0,
                    Rating = 4.6,
                    RatingCount = 32,
                    IsFavorite = true,
                    IngredientsJson = "[{\"Name\":\"Кукуруза\",\"Quantity\":7,\"Unit\":\"кг\",\"IsChecked\":false},{\"Name\":\"Ячмень\",\"Quantity\":2,\"Unit\":\"кг\",\"IsChecked\":false},{\"Name\":\"Пшеница\",\"Quantity\":1,\"Unit\":\"кг\",\"IsChecked\":false},{\"Name\":\"Дрожжи\",\"Quantity\":0.04,\"Unit\":\"г\",\"IsChecked\":false}]",
                    StepsJson = "[{\"Order\":1,\"Description\":\"Затирание – 120 минут при 68°C\",\"DurationMinutes\":120,\"Temperature\":68},{\"Order\":2,\"Description\":\"Ферментация – 5 дней при 20°C\",\"DurationMinutes\":4320,\"Temperature\":20},{\"Order\":3,\"Description\":\"Перегонка с хОС (холодное осахаривание)\",\"Temperature\":78}]",
                    DistillationParametersJson = "{\"InitialABV\":30,\"TargetABV\":62,\"HeadsPercentage\":10,\"HeartsPercentage\":80,\"TailsPercentage\":10,\"Temperature\":78,\"AtmosphericPressure\":1013}",
                    Notes = "Бюджетный вариант бурбона"
                },
                new Recipe
                {
                    Name = "Лимонная настойка",
                    Category = "Настойки",
                    Subcategory = "Цитрусовые",
                    Difficulty = 1,
                    TimeDays = 14,
                    YieldLiters = 1.0,
                    Rating = 4.9,
                    RatingCount = 68,
                    IngredientsJson = "[{\"Name\":\"Лимоны\",\"Quantity\":500,\"Unit\":\"г\",\"IsChecked\":false},{\"Name\":\"Водка 40%\",\"Quantity\":1,\"Unit\":\"л\",\"IsChecked\":false},{\"Name\":\"Сахар\",\"Quantity\":150,\"Unit\":\"г\",\"IsChecked\":false}]",
                    StepsJson = "[{\"Order\":1,\"Description\":\"Нарезать лимоны ломтиками\",\"DurationMinutes\":30},{\"Order\":2,\"Description\":\"Настаивать 2 недели в прохладном месте\",\"DurationMinutes\":20160},{\"Order\":3,\"Description\":\"Фильтровать через марлю\",\"DurationMinutes\":15}]",
                    Notes = "Быстрая настойка на лимонах"
                }
            };

            await _dbContext.Recipes.AddRangeAsync(recipes);
            await _dbContext.SaveChangesAsync();

            var notes = new List<Note>
            {
                new Note
                {
                    Title = "Рецепт: Виски из коньячного дистиллята",
                    Content = "Нужно сделать 5 литров коньячного дистиллята с крепостью 70%. Использовать новую бочку из-под коньяка, предварительно обожженную. Настаивать 3 года.",
                    Stage = ProcessStage.Aging,
                    IsFavorite = true,
                    Tags = "конняк, выдержка, бочка"
                },
                new Note
                {
                    Title = "Наблюдения за перегонкой",
                    Content = "При температуре 78°C крепость головы около 30%, тело – около 65%, хвосты – около 40%. Скорость отбора около 3 капель/секунда.",
                    Stage = ProcessStage.Distillation,
                    IsFavorite = false,
                    Tags = "перегонка, наблюдения"
                },
                new Note
                {
                    Title = "Инструкция по дегустации",
                    Content = "Дегустацию проводить при температуре +18°C. Прогреть вилку в руках, дать дегустировать 30 мл за раз. Делать перерывы между ритуалами.",
                    Stage = ProcessStage.Tasting,
                    IsFavorite = false,
                    Tags = "дегустация, методика"
                }
            };

            await _dbContext.Notes.AddRangeAsync(notes);
            await _dbContext.SaveChangesAsync();
        }

                var recipes = new List<Recipe>
                {
                    new Recipe
                    {
                        Name = "Классический односолодовый виски",
                        Category = "Виски",
                        Subcategory = "Single Malt",
                        Difficulty = 4,
                        TimeDays = 14,
                        YieldLiters = 3.5,
                        Rating = 4.8,
                        RatingCount = 45,
                        IsFavorite = true,
                        IngredientsJson = "[{\"Name\":\"Ячмень\",\"Quantity\":5,\"Unit\":\"кг\",\"IsChecked\":false},{\"Name\":\"Вода\",\"Quantity\":15,\"Unit\":\"л\",\"IsChecked\":false},{\"Name\":\"Хмель\",\"Quantity\":0.2,\"Unit\":\"кг\",\"IsChecked\":false},{\"Name\":\"Дрожжи\",\"Quantity\":0.05,\"Unit\":\"г\",\"IsChecked\":false}]",
                        StepsJson = "[{\"Order\":1,\"Description\":\"Затирание – 180 минут при 65°C\",\"DurationMinutes\":180,\"Temperature\":65},{\"Order\":2,\"Description\":\"Ферментация – 7 дней при 22°C\",\"DurationMinutes\":10080,\"Temperature\":22},{\"Order\":3,\"Description\":\"Перегонка – первый проход\",\"Temperature\":78}]",
                        DistillationParametersJson = "{\"InitialABV\":25,\"TargetABV\":65,\"HeadsPercentage\":12,\"HeartsPercentage\":78,\"TailsPercentage\":10,\"Temperature\":78,\"AtmosphericPressure\":1013}",
                        Notes = "Классический рецепт односолодового виски"
                    },
                    new Recipe
                    {
                        Name = "Бюджетный бурбон",
                        Category = "Бурбон",
                        Subcategory = "Bourbon",
                        Difficulty = 3,
                        TimeDays = 10,
                        YieldLiters = 4.0,
                        Rating = 4.6,
                        RatingCount = 32,
                        IsFavorite = true,
                        IngredientsJson = "[{\"Name\":\"Кукуруза\",\"Quantity\":7,\"Unit\":\"кг\",\"IsChecked\":false},{\"Name\":\"Ячмень\",\"Quantity\":2,\"Unit\":\"кг\",\"IsChecked\":false},{\"Name\":\"Пшеница\",\"Quantity\":1,\"Unit\":\"кг\",\"IsChecked\":false},{\"Name\":\"Дрожжи\",\"Quantity\":0.04,\"Unit\":\"г\",\"IsChecked\":false}]",
                        StepsJson = "[{\"Order\":1,\"Description\":\"Затирание – 120 минут при 68°C\",\"DurationMinutes\":120,\"Temperature\":68},{\"Order\":2,\"Description\":\"Ферментация – 5 дней при 20°C\",\"DurationMinutes\":4320,\"Temperature\":20},{\"Order\":3,\"Description\":\"Перегонка с хОС (холодное осахаривание)\",\"Temperature\":78}]",
                        DistillationParametersJson = "{\"InitialABV\":30,\"TargetABV\":62,\"HeadsPercentage\":10,\"HeartsPercentage\":80,\"TailsPercentage\":10,\"Temperature\":78,\"AtmosphericPressure\":1013}",
                        Notes = "Бюджетный вариант бурбона"
                    },
                    new Recipe
                    {
                        Name = "Лимонная настойка",
                        Category = "Настойки",
                        Subcategory = "Цитрусовые",
                        Difficulty = 1,
                        TimeDays = 14,
                        YieldLiters = 1.0,
                        Rating = 4.9,
                        RatingCount = 68,
                        IngredientsJson = "[{\"Name\":\"Лимоны\",\"Quantity\":500,\"Unit\":\"г\",\"IsChecked\":false},{\"Name\":\"Водка 40%\",\"Quantity\":1,\"Unit\":\"л\",\"IsChecked\":false},{\"Name\":\"Сахар\",\"Quantity\":150,\"Unit\":\"г\",\"IsChecked\":false}]",
                        StepsJson = "[{\"Order\":1,\"Description\":\"Нарезать лимоны ломтиками\",\"DurationMinutes\":30},{\"Order\":2,\"Description\":\"Настаивать 2 недели в прохладном месте\",\"DurationMinutes\":20160},{\"Order\":3,\"Description\":\"Фильтровать через марлю\",\"DurationMinutes\":15}]",
                        Notes = "Быстрая настойка на лимонах"
                    }
                };

                await _dbContext.Recipes.AddRangeAsync(recipes);
                await _dbContext.SaveChangesAsync();

                var notes = new List<Note>
                {
                    new Note
                    {
                        Title = "Рецепт: Виски из коньячного дистиллята",
                        Content = "Нужно сделать 5 литров коньячного дистиллята с крепостью 70%. Использовать новую бочку из-под коньяка, предварительно обожженную. Настаивать 3 года.",
                        Stage = ProcessStage.Aging,
                        IsFavorite = true,
                        Tags = "конняк, выдержка, бочка"
                    },
                    new Note
                    {
                        Title = "Наблюдения за перегонкой",
                        Content = "При температуре 78°C крепость головы около 30%, тело – около 65%, хвосты – около 40%. Скорость отбора около 3 капель/секунда.",
                        Stage = ProcessStage.Distillation,
                        IsFavorite = false,
                        Tags = "перегонка, наблюдения"
                    },
                    new Note
                    {
                        Title = "Инструкция по дегустации",
                        Content = "Дегустацию проводить при температуре +18°C. Прогреть вилку в руках, дать дегустировать 30 мл за раз. Делать перерывы между ритуалами.",
                        Stage = ProcessStage.Tasting,
                        IsFavorite = false,
                        Tags = "дегустация, методика"
                    }
                };

                await _dbContext.Notes.AddRangeAsync(notes);
                await _dbContext.SaveChangesAsync();

                // Подтверждаем изменения при наличии внешней транзакции
                if (transaction != null)
                {
                    transaction.Commit();
                }

                if (scope != null)
                {
                    scope.Complete();
                }
            }
            catch
            {
                if (scope != null)
                {
                    scope.Dispose();
                }
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw;
            }
        }
    }
}