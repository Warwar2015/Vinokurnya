# Инструкция: Подключение репозитория к GitHub

## Шаг 1: Создайте репозиторий на GitHub

1. Перейдите на [github.com/new](https://github.com/new)
2. Заполните форму:
   - **Repository name**: `Vinokurnya`
   - **Description**: `Приложение для самогоноварения и дистилляции с инженерными калькуляторами`
   - **Public** или **Private**: выберите нужное
   - ⚠️ **Важно**: НЕ ставьте галочку "Initialize this repository with a README" (мы уже создали локальный репозиторий)
3. Нажмите "Create repository"

## Шаг 2: Свяжите локальный репозиторий с GitHub

1. Скопируйте команду ниже (замените `ваш-никнейм` на ваш реальный никнейм на GitHub):

```bash
git remote add origin https://github.com/ваш-никнейм/Vinokurnya.git
```

2. Проверьте, что удаленный репозиторий добавлен:

```bash
git remote -v
```

## Шаг 3: Установите HTTPS-credential helper (опционально)

Чтобы не вводить пароль при каждом пуше:

### Windows (Git Credential Manager):
```bash
git config --global credential.helper manager
```

### Linux/Mac (Python):
```bash
git config --global credential.helper store
```

## Шаг 4: Загрузите код на GitHub

Выполните команду:

```bash
git branch -M main
git push -u origin main
```

### Что произойдёт:
- Местная ветка `master` переименовывается в `main`
- Все файлы будут загружены на GitHub
- Первая загрузка может занять несколько минут (зависит от размера проекта)

## Шаг 5: Проверьте результат

1. Перейдите на свой репозиторий на GitHub: `https://github.com/ваш-никнейм/Vinokurnya`
2. Вы должны увидеть все файлы проекта

## Шаг 6: Обновите имя автора (опционально)

Если хотите изменить имя автора в коммитах:

```bash
git config user.name "Ваше Имя"
git config user.email "ваша-почта@example.com"
git commit --amend --reset-author
git push -f
```

## 🎉 Готово!

Теперь вы можете:
- Создавать Pull Requests для новых изменений
- Участвовать в разработке
- Делать форки (forks) проекта
- Делиться кодом с другими

## 📋 Полезные команды

```bash
# Просмотр истории коммитов
git log

# Просмотр статуса
git status

# Создание новой ветки
git checkout -b feature/новая-функция

# Создание коммита
git add .
git commit -m "Описание изменений"

# Отправка изменений на GitHub
git push origin main

# Получение изменений с GitHub
git pull origin main
```

## 🆘 Troubleshooting

### Ошибка: "remote origin already exists"
```bash
git remote remove origin
git remote add origin https://github.com/ваш-никнейм/Vinokurnya.git
```

### Ошибка: "fatal: unable to access ... HTTPS proxy"
Проверьте сетевое подключение или попробуйте использовать SSH вместо HTTPS:
```bash
git remote remove origin
git remote add origin git@github.com:ваш-никнейм/Vinokurnya.git
```

### Ошибка: "failed to push some refs"
Если вы меняли имя автора или вносили изменения локально:
```bash
git push -f
```

## 📸 Как проверить

После выполнения всех шагов, откройте в браузере:
```
https://github.com/ваш-никнейм/Vinokurnya
```

Вы должны увидеть:
- 📁 Структуру папок проекта
- 📄 Файлы с кодом
- 📝 Коммит с описанием

---

**Вопросы?** Если возникают проблемы, сообщите мне сообщение об ошибке! 🚀