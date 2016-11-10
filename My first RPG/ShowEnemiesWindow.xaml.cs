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
using System.Runtime.Serialization.Formatters.Binary;
namespace My_first_RPG
{
    /// <summary>
    /// Interaction logic for ShowEnemiesWindow.xaml
    /// </summary>
    public partial class ShowEnemiesWindow : Window
    {
        /// <summary>
        /// Це вікно слугує для для виведення Monsters у вигляді кнопок
        /// </summary>
        /// <param name="Monsters"></param>

        Monster[] Mnstrs = null;
        Monster MonsterThatShoudReturn = null;
        int XMargin = 20;
        int YMargin = 20;
        int BorderWidth = 117;
        int BorderHeight = 77;
        int LabelHeight = 55;
        int LabelWidth = 95;
        public ShowEnemiesWindow(params Monster[] Monsters)
        {
            InitializeComponent();
            this.Mnstrs = Monsters;


            int MaxCountInColumn = 1;//максимально можна поставити 3 бордери в стовбець
            for (int i = 0; i < Monsters.Length; i++)
            {
                TextBlock textBlck = new TextBlock();
                textBlck.Height = LabelHeight;
                textBlck.Width = LabelWidth;
                textBlck.Margin = new Thickness(10);
                textBlck.Text = string.Format($"{Monsters[i].Name} \nрiвень:{Monsters[i].Level} \nздоров`я:{Monsters[i].Health} ");
                textBlck.TextWrapping = TextWrapping.Wrap;
                this.RegisterName("textblck" + i, textBlck);

               

                Border Bord = new Border();
                Bord.Width = BorderWidth;
                Bord.Height = BorderHeight;
                Bord.Margin = new Thickness(XMargin, YMargin, 0, 0);
                Bord.VerticalAlignment = VerticalAlignment.Top;
                Bord.HorizontalAlignment = HorizontalAlignment.Left;
                Bord.CornerRadius = new CornerRadius(15);
                Bord.Background = Brushes.LightGray;
                Bord.MouseDown += this.ReturnMonster;
                Bord.Name = "Bord" + i;
                this.RegisterName("Bord" + i, Bord);
                Bord.Child = textBlck;
                Grid1.Children.Add(Bord);

                YMargin += 100;

                MaxCountInColumn++;
                if (MaxCountInColumn > 3)
                {
                    YMargin = 20;
                    XMargin += 140;
                    MaxCountInColumn = 1;
                    continue;
                }


            }
        }
        public Monster RetMon()
        {
            return this.MonsterThatShoudReturn;
        }
        private void ReturnMonster(object sender, MouseButtonEventArgs e)
        {
            int index;
            char number;
            Border br = sender as Border;
            number = br.Name[br.Name.Length - 1];// получає номер бордера; відлік від нуля
            index = int.Parse(number.ToString());
            
            this.MonsterThatShoudReturn = this.Mnstrs[index];
            this.Close();
        }
        
    }
}
