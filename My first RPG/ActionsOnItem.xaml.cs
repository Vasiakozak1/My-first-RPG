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
        private PlayersInventory inventory;
        private Item selecteditem;
        public ActionsOnItem(Item thing,PlayersInventory Inventory)
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();
            this.inventory = Inventory;
            this.selecteditem = thing;
            ElipseClose.MouseDown += CloseWindow;
            List<ItemActions> actions = thing.AvailableActions;
            for(int i = 0; i < actions.Count; i++)
            {
                TextBlock TblockChoice = new TextBlock();
                TblockChoice.Background = Brushes.Brown;
                TblockChoice.Text = actions[i].ToString();
                TblockChoice.FontSize = 16;
                TblockChoice.MouseDown += this.DoSelectedWork;

                this.RegisterName("TblockChoice" + i, TblockChoice);
                StackPanel tmp = MainBorder.Child as StackPanel;
                tmp.Children.Add(TblockChoice);
                this.MainBorder.Margin = new Thickness(this.MainBorder.Margin.Left, this.MainBorder.Margin.Top
                    , this.MainBorder.Margin.Right, this.MainBorder.Margin.Bottom - 20);
            }
        }

        private void CloseWindow(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void DoSelectedWork(object sender, EventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            switch (tb.Text)
            {
                case "Зняти":
                    this.selecteditem.RemoveAction(ItemActions.Зняти);
                    this.selecteditem.AddAction(ItemActions.Одіти);
                    if (this.selecteditem is Armor)
                        this.inventory.WindowForEquipment.UnWear(selecteditem as Armor);
                    else
                        this.inventory.WindowForEquipment.UnWear((Weapon)selecteditem);
                    this.inventory.AddItem(selecteditem);
                    break;
                case "Викинути":
                    if (this.selecteditem is Armor)
                        this.inventory.WindowForEquipment.UnWear(selecteditem as Armor);
                    else
                        this.inventory.WindowForEquipment.UnWear((Weapon)selecteditem);
                    break;
                case "Одіти":
                    this.selecteditem.RemoveAction(ItemActions.Одіти);
                    this.selecteditem.AddAction(ItemActions.Зняти);
                    if (this.selecteditem is Armor)
                        this.inventory.WindowForEquipment.Wear(this.selecteditem as Armor);
                    else
                        this.inventory.WindowForEquipment.Wear(this.selecteditem as Weapon);
                    this.inventory.RemoveItem(this.selecteditem);
                    break;
            }
            this.Close();
        }
    }
}
