using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VinokurnyaWpf.ViewModels;

namespace VinokurnyaWpf.Views
{
    public partial class CalculatorView : UserControl
    {
        public CalculatorView()
        {
            InitializeComponent();

            // Set up calculator type visibility converter
            var converter = new CalculatorTypeToVisibilityConverter();
            BindingOperations.SetBinding(this,
                VisibilityProperty,
                new Binding("SelectedCalculator") { Source = DataContext, Converter = converter });
        }
    }

    public class CalculatorTypeToVisibilityConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return System.Windows.Visibility.Collapsed;

            if (parameter is CalculatorType requestedType && value is CalculatorType selectedType)
            {
                return selectedType == requestedType ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }

            return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}