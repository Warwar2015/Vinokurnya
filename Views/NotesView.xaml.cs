using System.Windows.Controls;
using VinokurnyaWpf.Data;
using VinokurnyaWpf.ViewModels;

namespace VinokurnyaWpf.Views
{
    public partial class NotesView : UserControl
    {
        public NotesView()
        {
            InitializeComponent();
        }
    }

    public class StageToTextConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ProcessStage stage)
            {
                return stage switch
                {
                    ProcessStage.Braga => "Брага",
                    ProcessStage.Distillation => "Перегонка",
                    ProcessStage.Aging => "Выдержка",
                    ProcessStage.Tasting => "Дегустация",
                    ProcessStage.Other => "Другое",
                    _ => stage.ToString()
                };
            }
            return "Другое";
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}