# Инструкция по клонированию репозитория

Если у тебя нет репозитория Git, выполни следующие команды:

## 1. Проверь, есть ли Git:
```bash
git --version
```

## 2. Клонируй репозиторий (выполни в папке, где хочешь проект):
```bash
git clone https://github.com/Warwar2015/Vinokurnya.git
```

## 3. Перейди в папку проекта:
```bash
cd Vinokurnya
```

## 4. Теперь можешь обновлять проект:
```bash
git pull
```

## 5. Собери и запусти:
```bash
dotnet build -c Release
cd bin\Release\net8.0-windows
.\VinokurnyaWpf.exe
```

---

## Альтернатива - скопировать файлы вручную

Если не хочешь использовать Git, просто:

1. Скачай файлы с GitHub:
   - Открой https://github.com/Warwar2015/Vinokurnya
   - Нажми "Code" → "Download ZIP"
   - Распакуй архив

2. Открой в Visual Studio и собери проект

3. Запусти через F5

---

## Или используй Visual Studio напрямую

1. Открой Visual Studio
2. File → Open → Project/Solution
3. Выбери `VinokurnyaWpf.csproj`
4. Нажми "Start" (F5) для Debug режима

5. Если будут ошибки - появится окно с деталями

---

## Если хочешь продолжить через Git

1. Удали текущую папку Vinokurnya-main
2. Выполни `git clone https://github.com/Warwar2015/Vinokurnya.git`
3. Перейди в папку и выполняй команды выше

---

**Рекомендация:** Используй Visual Studio для запуска, это проще!
