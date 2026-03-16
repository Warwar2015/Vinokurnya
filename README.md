# Винокурня 🥃

<div align="center">

**Professionally crafted tool for home distillation**

[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![Windows](https://img.shields.io/badge/Windows-10%20%2F%2011-green)](https://www.microsoft.com/windows/)
[![License](https://img.shields.io/badge/License-MIT-yellow)](LICENSE)

![GitHub stars](https://img.shields.io/github/stars/ваш-никнейм/Vinokurnya?style=social)
![GitHub forks](https://img.shields.io/github/forks/ваш-никнейм/Vinokurnya?style=social)

</div>

---

## 📖 О проекте

**Винокурня** — профессиональный инструмент для самогоноварения и дистилляции с инженерными калькуляторами, базой рецептов и системой заметок.

Приложение создано на **.NET 8.0 + WPF** с использованием **MVVM архитектуры** и **Material Design** стилей.

## ✨ Возможности

### 🧮 Инженерные калькуляторы

1. **Разбавление спирта** — расчет с учетом температуры
   - Объем воды для разбавления
   - Коррекция плотности по температуре
   - Табличные данные плотности спирта

2. **Дробная перегонка** — расчет фракций
   - Головы, тело, хвосты
   - Процентное соотношение
   - Оценка крепости тела

3. **Скорость отбора** — оптимальная скорость
   - Капель в секунду
   - Интенсивность отбора
   - Рекомендации по оборудованию

4. **Температурные режимы** — точные расчеты
   - Температура кипения воды
   - Коррекция по атмосферному давлению
   - Оптимальная температура перегонки

### 📖 Рецепты

- **Виски**: Single Malt, торфяной, бюджетные
- **Бурбон**: Классический, ржаной, кукурузно-пшеничный
- **Настойки**: Цитрусовые, ягодные, травяные

**Функции:**
- 🔍 Поиск и фильтрация по категориям
- ⭐ Добавление в избранное
- 📊 Рейтинг и сложность
- 📝 Детальная информация

### 📝 Заметки

- ✍️ Создание, редактирование, удаление
- 📂 Категоризация по этапам:
  - Брага
  - Перегонка
  - Выдержка
  - Дегустация
- 🔍 Поиск по содержимому
- 📤 Экспорт/импорт в JSON
- ⭐ Отметка как любимые

### 📊 Статистика

- 📈 Общее количество рецептов
- 🔥 Рецепты в избранном
- ⭐ Средний рейтинг
- 📊 Популярные категории

### 🎨 Интерфейс

- 🌙 **Темная тема**: Фон `#1A1A2E`, акцент `#00FFAB`
- ☀️ **Светлая тема**: Фон `#FFFFFF`, акцент `#008F6B`
- 🎨 Material Design стиль
- 📱 Адаптивный дизайн

## 🚀 Быстрый старт

### Требования

- **.NET 8.0 SDK**
- **Windows 10 версии 1809 или новее**
- **Windows 11** (любая версия)

### Установка

#### Вариант 1: Через командную строку

```bash
# Клонирование (если репозиторий уже создан)
git clone https://github.com/ваш-никнейм/Vinokurnya.git
cd Vinokurnya

# Или если вы работаете локально
cd /path/to/VinokurnyaWpf

# Восстановление зависимостей
dotnet restore

# Запуск
dotnet run
```

#### Вариант 2: Через Visual Studio

1. Откройте `VinokurnyaWpf.csproj`
2. Нажмите кнопку "Запустить"

## 📦 Структура проекта

```
VinokurnyaWpf/
├── VinokurnyaWpf.csproj         # .NET проект
├── App.xaml                     # Главное окно
├── Resources/
│   ├── Styles.xaml              # Стили Material Design
│   └── Themes/
│       ├── Dark.xaml            # Темная тема
│       └── Light.xaml           # Светлая тема
├── Models/                      # Модели данных
│   ├── Recipe.cs                # Рецепт
│   └── Note.cs                  # Заметка
├── Data/
│   └── AppDbContext.cs          # EF Core контекст
├── Services/
│   ├── CalculationService.cs    # Инженерные расчеты
│   ├── DataService.cs           # CRUD операции
│   └── ThemeService.cs          # Управление темами
├── ViewModels/                  # MVVM ViewModels
├── Views/                       # WPF Views
└── README.md                    # Этот файл
```

## 🛠️ Технологии

- **.NET 8.0** — Основная платформа
- **WPF (Windows Presentation Foundation)** — UI фреймворк
- **MVVM (Model-View-ViewModel)** — Архитектурный паттерн
- **Entity Framework Core** — ORM для SQLite
- **SQLite** — База данных
- **Material Design** — Стили и компоненты
- **CommunityToolkit.Mvvm** — Пакет для MVVM

## 📦 Зависимости

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.0" />
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.0" />
<PackageReference Include="MaterialDesignThemes" Version="4.9.0" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```

## 🚦 Использование калькуляторов

### Разбавление спирта

1. Откройте вкладку "🧮 Калькуляторы"
2. Нажмите "💧 Разбавление спирта"
3. Введите:
   - Объем: `1000` мл
   - Текущая крепость: `50`%
   - Целевая крепость: `40`%
4. Нажмите "Рассчитать"

### Дробная перегонка

1. Нажмите "🔥 Дробная перегонка"
2. Введите:
   - Объем: `10` л
   - Крепость: `25`%
3. Нажмите "Рассчитать"

## 📤 Экспорт/Импорт

### Экспорт рецептов

1. Перейдите на вкладку "📖 Рецепты"
2. Нажмите кнопку "Экспорт"
3. Выберите формат: JSON или TXT

### Экспорт заметок

1. Перейдите на вкладку "📝 Заметки"
2. Нажмите кнопку "Экспорт"
3. Выберите формат: JSON или TXT

## 🤝 Участие

Мы приветствуем вклад сообщества!

### Как внести вклад

1. Форкните этот репозиторий
2. Создайте ветку для новой функции:
   ```bash
   git checkout -b feature/AmazingFeature
   ```
3. Закоммитьте изменения:
   ```bash
   git commit -m 'Add some AmazingFeature'
   ```
4. Запушьте ветку:
   ```bash
   git push origin feature/AmazingFeature
   ```
5. Откройте Pull Request

## 📋 Команды Git

```bash
# Создание новой ветки
git checkout -b feature/новая-функция

# Внесение изменений
git add .
git commit -m "Описание изменений"

# Отправка на GitHub
git push origin main

# Получение изменений
git pull origin main
```

## 🎯 Roadmap

### Планируемые функции

- [ ] Редактор заметок (создание, редактирование)
- [ ] Детальный экран рецепта с картинками
- [ ] Графики и диаграммы
- [ ] Анимации и переходы
- [ ] Dark/Light mode переключатель в интерфейсе
- [ ] Мультиязычность (русский/английский)
- [ ] Импорт рецептов из файлов
- [ ] Экспорт в PDF
- [ ] Сохранение настроек пользователя
- [ ] Тестирование с использованием xUnit

## 📄 Лицензия

Этот проект распространяется под лицензией **MIT**. См. файл `LICENSE` для деталей.

## 🙏 Благодарности

- [Material Design for XAML](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) за стиль
- [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) за MVVM пакет
- [Entity Framework Core](https://docs.microsoft.com/ef/core/) за ORM
- [SQLite](https://www.sqlite.org/) за базу данных

## 📞 Поддержка

Если у вас есть вопросы или предложения:

1. Откройте Issue в этом репозитории
2. Свяжитесь с разработчиком
3. Участвуйте в обсуждениях

## 🎉 Автор

Создано с ❤️ для сообщества самогонщиков.

---

<div align="center">

**Made with .NET 8.0 and WPF**

[⬆ Back to Top](#винокурня-🥃)

</div>