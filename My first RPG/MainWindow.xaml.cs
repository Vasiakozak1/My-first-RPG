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
using System.Windows.Shapes;

namespace My_first_RPG
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
                   
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window1 wnd1 = new Window1();
            wnd1.Show();
            
        }

        private void label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Ellipse_MouseLeftButtonDown(sender, e);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Window2 wnd2 = new Window2();
            wnd2.Show();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            GameWindow game;
            if (label1.Visibility == Visibility.Visible || textBox1.Visibility == Visibility.Visible)
            {
                if (textBox1.Text != string.Empty)
                {
                    game = new GameWindow(textBox1.Text);
                    game.Show();
                }
            }
            else
            {
                label1.Visibility = Visibility.Visible;
                textBox1.Visibility = Visibility.Visible;
            }
        }
    }
}
