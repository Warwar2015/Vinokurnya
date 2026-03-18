using System.Windows.Controls;
using VinokurnyaWpf.ViewModels;

namespace VinokurnyaWpf.Views
{
    public partial class RecipesView : UserControl
    {
        public RecipesView()
        {
            InitializeComponent();

            // Set up converter instances
            Resources["DifficultyToStarsConverter"] = new DifficultyToStarsConverter();
            Resources["RatingToStarsConverter"] = new RatingToStarsConverter();
            Resources["BoolToTextConverter"] = new BoolToTextConverter();
        }
    }

    // Value converters
    public class DifficultyToStarsConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int difficulty)
            {
                string stars = "";
                for (int i = 0; i < 5; i++)
                {
                    stars += i < difficulty ? "★" : "☆";
                }
                return stars;
            }
            return "";
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }

    public class RatingToStarsConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double rating)
            {
                string stars = "";
                int maxStars = 5;
                for (int i = 0; i < maxStars; i++)
                {
                    if (i < rating)
                    {
                        stars += "★";
                    }
                    else
                    {
                        stars += "☆";
                    }
                }
                return stars;
            }
            return "";
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }

    public class BoolToTextConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool isChecked)
            {
                return isChecked ? "Да" : "Нет";
            }
            return "Нет";
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}