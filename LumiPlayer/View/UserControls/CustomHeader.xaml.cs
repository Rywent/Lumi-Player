using System;
using System.Collections.Generic;
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

namespace LumiPlayer.View.UserControls
{
    /// <summary>
    /// Interaction logic for CustomHeader.xaml
    /// </summary>
    public partial class CustomHeader : UserControl
    {
        public CustomHeader()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }
        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if(window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void Drag_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                Window.GetWindow(this)?.DragMove();
        }
    }
}
