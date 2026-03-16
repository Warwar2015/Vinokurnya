using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using VinokurnyaWpf.Data;
using VinokurnyaWpf.Services;
using VinokurnyaWpf.Helpers;

namespace VinokurnyaWpf.ViewModels
{
    public class NotesViewModel : BindableBase
    {
        private readonly DataService _dataService;

        private string _searchQuery = "";
        private string _selectedStage = "Все";
        private bool _showFavoritesOnly = false;
        private bool _isLoading;

        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        public string SelectedStage
        {
            get => _selectedStage;
            set => SetProperty(ref _selectedStage, value);
        }

        public bool ShowFavoritesOnly
        {
            get => _showFavoritesOnly;
            set => SetProperty(ref _showFavoritesOnly, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ObservableCollection<Note> Notes { get; }

        public ObservableCollection<string> Stages { get; }
        public ObservableCollection<Note> FilteredNotes { get; }

        public ICommand SearchCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand ShowFavoritesCommand { get; }
        public ICommand CreateNewNoteCommand { get; }
        public ICommand DeleteNoteCommand { get; }
        public ICommand ExportNotesCommand { get; }

        public NotesViewModel()
        {
            _dataService = App.DataService;

            Notes = new ObservableCollection<Note>();
            Stages = new ObservableCollection<string> { "Все", "Брага", "Перегонка", "Выдержка", "Дегустация", "Другое" };
            FilteredNotes = new ObservableCollection<Note>();

            SearchCommand = new RelayCommand(Search);
            FilterCommand = new RelayCommand(Filter);
            ShowFavoritesCommand = new RelayCommand(ShowFavorites);
            CreateNewNoteCommand = new RelayCommand(CreateNewNote);
            DeleteNoteCommand = new RelayCommand<Guid>(DeleteNote);
            ExportNotesCommand = new RelayCommand(ExportNotes);

            LoadNotesAsync();
        }

        private async void LoadNotesAsync()
        {
            IsLoading = true;
            try
            {
                var allNotes = await _dataService.GetAllNotesAsync();
                Notes.Clear();
                foreach (var note in allNotes)
                {
                    Notes.Add(note);
                }
                FilterNotes();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading notes: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void Search()
        {
            FilterNotes();
        }

        private void Filter()
        {
            FilterNotes();
        }

        private void ShowFavorites()
        {
            ShowFavoritesOnly = !ShowFavoritesOnly;
            FilterNotes();
        }

        private void CreateNewNote()
        {
            // TODO: Open note editor window
            System.Windows.MessageBox.Show("Редактор заметок будет доступен в будущих версиях", "Внимание",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void DeleteNote(Guid noteId)
        {
            try
            {
                var result = await _dataService.DeleteNoteAsync(noteId);

                if (result)
                {
                    var note = Notes.FirstOrDefault(n => n.Id == noteId);
                    if (note != null)
                    {
                        Notes.Remove(note);
                    }
                    System.Windows.MessageBox.Show("Заметка удалена", "Успешно",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterNotes()
        {
            FilteredNotes.Clear();

            var filtered = Notes.AsEnumerable();

            // Filter by stage
            if (!string.IsNullOrEmpty(SelectedStage) && SelectedStage != "Все")
            {
                filtered = filtered.Where(n => n.Stage.ToString() == SelectedStage);
            }

            // Filter by search query
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                filtered = filtered.Where(n => n.Title.ToLower().Contains(SearchQuery.ToLower()) ||
                                              n.Content.ToLower().Contains(SearchQuery.ToLower()));
            }

            // Filter by favorites
            if (ShowFavoritesOnly)
            {
                filtered = filtered.Where(n => n.IsFavorite);
            }

            foreach (var note in filtered)
            {
                FilteredNotes.Add(note);
            }
        }

        private async void ExportNotes()
        {
            try
            {
                var notesToExport = ShowFavoritesOnly ? Notes.Where(n => n.IsFavorite) : Notes;

                if (!notesToExport.Any())
                {
                    System.Windows.MessageBox.Show("Нет заметок для экспорта", "Внимание",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var json = System.Text.Json.JsonSerializer.Serialize(notesToExport, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

                var saveDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "JSON файлы (*.json)|*.json|Текстовые файлы (*.txt)|*.txt",
                    FileName = "vinokurnya_notes.json"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    await File.WriteAllTextAsync(saveDialog.FileName, json);
                    System.Windows.MessageBox.Show("Заметки успешно экспортированы", "Успешно",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}