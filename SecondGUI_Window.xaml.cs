using System.Windows;
using System.Windows.Forms;
using System.Drawing;

namespace UnderwaterHockeyTimer
{
    /// <summary>
    /// Interaction logic for SecondGUI_Window.xaml
    /// </summary>
    public partial class SecondGUI_Window : Window
    {
        public SecondGUI_Window()
        {
            InitializeComponent();
        }
        public void SetSecondScreen()
        {
            int numScreens = 0;
            foreach (var screen in Screen.AllScreens)
            {
                numScreens++;
                if (numScreens == 2)
                {
                    this.Left = screen.WorkingArea.Left / 2 - 223;                    
                }
            }
            if (numScreens < 2)
            {
                System.Windows.MessageBox.Show("Second screen not found!");
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetSecondScreen();
            MainWindow.secondGUI = this;
        }
    }
}
