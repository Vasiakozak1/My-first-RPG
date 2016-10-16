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
    /// Interaction logic for PlayersInventory.xaml
    /// </summary>

    /// <summary>
    /// Прийшлося розділити інфентар на два класи
    /// </summary>

    [Serializable]
    public class Inventory
    {
        public Item[,] Items { get; private set; }

        public Inventory()
        {
            Items = new Item[8, 8];
        }

    }
    public partial class PlayersInventory : Window
    {
        private Inventory InventoryItems;
        public PlayersInventory(Inventory PlayerInventory)
        {
            
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
 
            int VerticalMargin = 0;
            int HorisontalMargin = 0;
            this.InventoryItems = PlayerInventory;
            
            for (int i = 0; i < 8; i++) 
            {
                for (int j = 0; j < 8; j++) 
                {
                    Border CreatedBorder = new Border();
                    CreatedBorder.Name = "CreatedBorder" + i + j;
                    CreatedBorder.HorizontalAlignment = HorizontalAlignment.Left;
                    CreatedBorder.VerticalAlignment = VerticalAlignment.Top;
                    CreatedBorder.Margin = new Thickness(HorisontalMargin, VerticalMargin, 0, 0);
                    CreatedBorder.Height = 40;
                    CreatedBorder.Width = 40;
                    CreatedBorder.Background = Brushes.Gray;
                    CreatedBorder.BorderBrush = Brushes.LightGray;
                    CreatedBorder.BorderThickness = new Thickness(1);
                    CreatedBorder.CornerRadius = new CornerRadius(5);

                    Image img = new Image();
                    img.Margin = new Thickness(1);
                    CreatedBorder.Child = img;
                    
                    this.RegisterName("CreatedBorder" + i + j, CreatedBorder);
                    
                    
                    this.GridForItems.Children.Add(CreatedBorder);
                    HorisontalMargin += 40;
                }
                VerticalMargin += 40;
                HorisontalMargin = 0;
            }

            this.SynchronizeItems();
        }
        /// <summary>
        /// Синхронізовує Елементи інвентару гравця із цим вікном, покищо використовується лиш у конструкторі цього класу...
        /// </summary>
        private void SynchronizeItems()
        {
            for (int i = 0; i < this.InventoryItems.Items.GetLength(1); i++) 
            {
                for(int j = 0; j < this.InventoryItems.Items.GetLength(0); j++)
                {
                    if (this.InventoryItems.Items[i, j] == null)
                        continue;
                    int ColIndex, RowIndex;
                    foreach (Border tmpborder in this.GridForItems.Children)
                    {
                        ColIndex = int.Parse(tmpborder.Name[tmpborder.Name.Length - 2].ToString());
                        RowIndex = int.Parse(tmpborder.Name[tmpborder.Name.Length - 1].ToString());
                        
                        if ((int)ColIndex != i || (int)RowIndex != j)
                            continue;
                        // Індекс Бордера з картинкою відповідає індексу предмета в інфентарі гравця
                        tmpborder.MouseRightButtonDown += this.ShowActionsOnItem;
                        Image img = tmpborder.Child as Image;
                        img.Source = new BitmapImage(new Uri(this.InventoryItems.Items[i, j].PathIconOfItem, UriKind.RelativeOrAbsolute));
                        
                    }
                }
            }
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
        }
        
        public void RemoveItem(Item ItemToRemove)
        {
            foreach(Border br in this.GridForItems.Children)
            {
                Image img = br.Child as Image;
                if (img.Source.ToString() != ItemToRemove.PathIconOfItem)
                    continue;
                img.Source = null;
                int ColumnIndex = int.Parse(br.Name[br.Name.Length - 2].ToString());
                int RowIndex = int.Parse(br.Name[br.Name.Length - 1].ToString());
                this.InventoryItems.Items[ColumnIndex, RowIndex] = null;
                return;
            }
        }
        public void AddItem(Item ItemToAdd)
        {
            foreach (Border br in this.GridForItems.Children)
            {
                Image img = br.Child as Image;
                if (img.Source == null)
                {
                    int ColumnIndex = int.Parse(br.Name[br.Name.Length - 2].ToString());
                    int RowIndex = int.Parse(br.Name[br.Name.Length - 1].ToString());
                    this.InventoryItems.Items[ColumnIndex, RowIndex] = ItemToAdd;
                    img.Source = new BitmapImage(new Uri(ItemToAdd.PathIconOfItem, UriKind.RelativeOrAbsolute));
                    br.MouseRightButtonDown += this.ShowActionsOnItem;
                    return;
                }
            }

            MessageBox.Show("Рюкзак повний");
        }
        /// <summary>
        /// Right клік по предмету
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowActionsOnItem(object sender,MouseButtonEventArgs e)
        {
            Border br = sender as Border;
            int ColumnIndex = int.Parse(br.Name[br.Name.Length - 2].ToString());
            int RowIndex = int.Parse(br.Name[br.Name.Length - 1].ToString());
            ActionsOnItem actionswindow = new ActionsOnItem(this.InventoryItems.Items[ColumnIndex, RowIndex]);
            actionswindow.Show();
        }
    }
}
