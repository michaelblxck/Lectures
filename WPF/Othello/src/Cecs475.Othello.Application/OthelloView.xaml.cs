using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cecs475.Othello.Application {
	/// <summary>
	/// Interaction logic for OthelloView.xaml
	/// </summary>
	public partial class OthelloView : UserControl {
		public static SolidColorBrush RED_BRUSH = new SolidColorBrush(Colors.Red);
		public static SolidColorBrush GREEN_BRUSH = new SolidColorBrush(Colors.Green);

		public OthelloView() {
			InitializeComponent();
		}

		private void Border_MouseEnter(object sender, MouseEventArgs e) {
			Border b = sender as Border;
			var square = b.DataContext as OthelloSquare;
			var vm = FindResource("vm") as OthelloViewModel;
			if (vm.PossibleMoves.Contains(square.Position)) {
				b.Background = RED_BRUSH;
			}
		}

		private void Border_MouseLeave(object sender, MouseEventArgs e) {
			Border b = sender as Border;
			b.Background = GREEN_BRUSH;
		}

		public OthelloViewModel Model {
			get { return FindResource("vm") as OthelloViewModel; }
		}

		private void Border_MouseUp(object sender, MouseButtonEventArgs e) {
			Border b = sender as Border;
			var square = b.DataContext as OthelloSquare;
			var vm = FindResource("vm") as OthelloViewModel;
			if (vm.PossibleMoves.Contains(square.Position)) {
				vm.ApplyMove(square.Position);
			}
		}
	}

    /// <summary>
    /// Converts from an integer board value to a formatted string for the score
    /// </summary>
    public class OthelloScoreConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            int boardValue = (int)value;
            if (boardValue > 0) return "Mehrdad is winning by " + boardValue;
            else if (boardValue < 0) return "Frank is winning by " + Math.Abs(boardValue);
            else return "tie game";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts from an integer player to a string, either 'Mehrdad' or 'Frank'
    /// </summary>
    public class OthelloPlayerConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            int player = (int)value;
            if (player == 1) return "Mehrdad";
            else return "Frank";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

	/// <summary>
	/// Converts from an integer player number to an Ellipse representing that player's token.
	/// </summary>
	public class OthelloSquarePlayerConverter : IValueConverter {
		private static SolidColorBrush WHITE_BRUSH = new SolidColorBrush(Colors.White);
		private static SolidColorBrush BLACK_BRUSH = new SolidColorBrush(Colors.Black);

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			int player = (int)value;
			if (player == 0) {
				return null;
			}

			Ellipse token = new Ellipse() {
				Fill = GetFillBrush(player)
			};
			return token;
		}

        private static ImageBrush GetFillBrush(int player)
        {
            if (player == 1)
            {
                ImageBrush mehrdadBrush = new ImageBrush();
                mehrdadBrush.ImageSource = new BitmapImage(new Uri("C:\\Users\\013505594\\Desktop\\mehrdad.jfif"));
                return mehrdadBrush;
            }
            ImageBrush frankBrush = new ImageBrush();
            frankBrush.ImageSource = new BitmapImage(new Uri("C:\\Users\\013505594\\Desktop\\frank.jpg"));
            return frankBrush;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
