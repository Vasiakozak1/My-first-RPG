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
    /// Interaction logic for ActionsOnItem.xaml
    /// </summary>
    public partial class ActionsOnItem : Window
    {
        
        public ActionsOnItem(Item thing)
        {           
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ElipseClose.MouseDown += Funct;
            List<ItemActions> actions = thing.AvailableActions;
            for(int i = 0; i < actions.Count; i++)
            {
                TextBlock TblockChoice = new TextBlock();
                TblockChoice.Background = Brushes.Brown;
                TblockChoice.Text = actions[i].ToString();
                TblockChoice.FontSize = 16;

                this.RegisterName("TblockChoice" + i, TblockChoice);
                StackPanel tmp = MainBorder.Child as StackPanel;
                tmp.Children.Add(TblockChoice);
                this.MainBorder.Margin = new Thickness(this.MainBorder.Margin.Left, this.MainBorder.Margin.Top
                    , this.MainBorder.Margin.Right, this.MainBorder.Margin.Bottom - 20);
            }
        }
        private void ActionChoice(object sender, EventArgs e)
        {
            TextBlock bl = sender as TextBlock;
            MessageBox.Show(bl.Text);
            this.Close();
        }

        private void Funct(object sender, MouseEventArgs e)
        {
            this.Close();
        }
    }
}
