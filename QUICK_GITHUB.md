# Команды для подключения к GitHub (готовые к копированию)

## 1. Создайте репозиторий на GitHub

Перейдите: https://github.com/new
- **Repository name**: `Vinokurnya`
- **Description**: `Приложение для самогоноварения и дистилляции`
- **Public/Private**: выберите
- ⚠️ НЕ ставьте галочку "Initialize with README"
- Нажмите "Create repository"

## 2. Подключите удаленный репозиторий

**Ваш GitHub никнейм:** ___________________________

Скопируйте и выполните эту команду:

```bash
cd /root/.openclaw/workspace/VinokurnyaWpf && git remote add origin https://github.com/ВАШ-НИКНЕЙМ/Vinokurnya.git && git branch -M main && git push -u origin main
```

Замените `ВАШ-НИКНЕЙМ` на ваш реальный никнейм!

## 3. После успешной загрузки

Перейдите на GitHub и проверьте:
https://github.com/ВАШ-НИКНЕЙМ/Vinokurnya

## 4. Дополнительные команды

### Установить credential helper (рекомендуется):

```bash
git config --global credential.helper manager
```

### Обновить имя автора (если нужно):

```bash
git config user.name "Ваше Имя"
git commit --amend --reset-author
git push -f
```

### Проверить статус:

```bash
git status
```

### Просмотреть лог:

```bash
git log --oneline
```

---

**Пример (замените на свой никнейм):**

```bash
cd /root/.openclaw/workspace/VinokurnyaWpf && git remote add origin https://github.com/RomaST17/Vinokurnya.git && git branch -M main && git push -u origin main
```

---

**Если возникают ошибки, сообщите:**
1. Команду, которую вы выполнили
2. Точное сообщение об ошибке
3. Ваш GitHub никнейм

Я помогу решить проблему! 🚀