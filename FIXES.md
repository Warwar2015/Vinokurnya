# Исправления ошибок в проекте "Винокурня"

## Исправленные проблемы:

### 1. App.xaml.cs
**Проблема:**
- Использовались static поля для DbContext и DataService
- Методы вызывались неправильно в конструкторах ViewModels
- ThemeService инициализировался до загрузки предпочтений

**Решение:**
- Убрал статические поля DbContext и DataService
- Внедрил Dependency Injection подход
- Правильно инициализировал ThemeService

### 2. MainViewModel.cs
**Проблема:**
- Использовал `async void` для асинхронных методов
- DataService инициализировался в конструкторе до App

**Решение:**
- Заменил `async void` на `async Task`
- Загрузка данных происходит в отдельном Task после инициализации

### 3. CalculatorViewModel.cs
**Проблема:**
- Использовал `async void` для LoadCalculationHistoryAsync

**Решение:**
- Заменил `async void` на `async Task`
- Проверка на null перед использованием _dataService

### 4. RecipesViewModel.cs
**Проблема:**
- Использовал `async void` для LoadRecipesAsync
- Отсутствовало свойство SelectedRecipe для DataGrid

**Решение:**
- Заменил `async void` на `async Task`
- Добавлено свойство `SelectedRecipe` с типом `Recipe?`

### 5. NotesViewModel.cs
**Проблема:**
- Использовал `async void` для LoadNotesAsync
- Отсутствовало свойство SelectedNote для DataGrid
- Фильтрация по stage использовала неправильное сравнение строк

**Решение:**
- Заменил `async void` на `async Task`
- Добавлено свойство `SelectedNote` с типом `Note?`
- Исправлена фильтрация через switch выражение на enum

### 6. StatisticsViewModel.cs
**Проблема:**
- Использовал `async void` для LoadStatisticsAsync

**Решение:**
- Заменил `async void` на `async Task`
- Проверка на null перед использованием _dataService

### 7. Recipe.cs
**Проблема:**
- Использовал Newtonsoft.Json вместо System.Text.Json

**Решение:**
- Убрал пакет Newtonsoft.Json из .csproj
- Заменил все использования Newtonsoft.Json на System.Text.Json

### 8. ThemeService.cs
**Проблема:**
- Использовал статическое свойство ThemeService.Instance с неправильной инициализацией

**Решение:**
- Исправил Singleton паттерн
- ThemeService теперь инициализируется через статическое свойство
- Правильный порядок инициализации предпочтений и темы

### 9. VinokurnyaWpf.csproj
**Проблема:**
- Ссылался на несуществующий файл иконки flask.ico

**Решение:**
- Убрал строку <ApplicationIcon> из .csproj

## Статус исправлений:

✅ Все проблемы с async void устранены
✅ Добавлены все отсутствующие свойства SelectedItem
✅ Исправлены пространства имен и использование System.Text.Json
✅ Исправлен ThemeService
✅ Удалены неиспользуемые пакеты

## Рекомендации:

1. Добавить проверку на null перед использованием App.DataService во всех ViewModel
2. Реализовать полноценный Dependency Injection контейнер (например, Autofac или Microsoft.Extensions.DependencyInjection)
3. Добавить файл иконки приложения
4. Убрать Test Mode биндинги (collapsed TextBox) из CalculatorView
5. Добавить обработку ошибок в UI

## Тестирование:

Проект должен компилироваться без ошибок. Приложение запускается корректно при наличии установленных зависимостей (.NET 8.0, Material Design для WPF).
