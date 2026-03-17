@echo off
echo ========================================
echo Тестирование компиляции Винокурни
echo ========================================
echo.

cd /d "%~dp0"
echo Текущая директория: %CD%
echo.

echo Проверка .NET SDK...
dotnet --version
if %errorlevel% neq 0 (
    echo ОШИБКА: .NET SDK не установлен
    exit /b 1
)
echo.

echo Восстановление зависимостей...
dotnet restore
if %errorlevel% neq 0 (
    echo ОШИБКА: Ошибка при восстановлении зависимостей
    exit /b 1
)
echo.

echo Очистка предыдущей сборки...
dotnet clean
echo.

echo Сборка проекта...
dotnet build -c Release
if %errorlevel% neq 0 (
    echo ОШИБКА: Ошибка при сборке проекта
    echo.
    echo Пожалуйста, проверьте логи выше
    exit /b 1
)
echo.

echo ========================================
echo УСПЕХ! Проект успешно скомпилирован
echo ========================================
echo.
echo Для запуска приложения выполните:
echo   dotnet run
echo.
pause
