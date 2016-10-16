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
    /// Interaction logic for WindowEquipment.xaml
    /// </summary>
    
    public class Equipment
    {
        public Armor Helmet { get; set; }//Шолом
        public Armor Breastplate { get; set; }//Нагрудник
        public Armor Shoulders { get; set; }//Наплечниики
        public Armor Pants { get; set; }//Штани
        public Armor Gloves { get; set; }//Рукавиці
        public Armor Boots { get; set; }//Черевики

        public uint TotalProtectionPoint { get; private set; }

        private Player Owner;

        public Equipment(Player Owner)
        {
            this.Boots = new Armor("Обмотки", 1, 1, @"Resourses\A_Shoes01.png", ArmorType.Черевики, 0, ItemActions.Викинути, ItemActions.Зняти);
            this.Pants = new Armor("Ножні обиотки", 1, 1, @"A_Pants01.png", ArmorType.Брюки, 1, ItemActions.Викинути, ItemActions.Зняти);
            this.TotalProtectionPoint = this.Boots.ProtectionPoints + this.Pants.ProtectionPoints;
            this.Owner = Owner;
        }
        
    }

    public partial class WindowEquipment : Window
    {
        private Equipment equip;
        private PlayersInventory inventory;
        public WindowEquipment(PlayersInventory Inventory, Equipment Equip)
        {
            InitializeComponent();
            this.equip = Equip;
            this.inventory = Inventory;
        }
        public void Add(Armor ArmorToWear)
        {
            switch (ArmorToWear.Type)
            {
                case ArmorType.Шолом:
                    if (this.equip.Helmet == null)
                    {
                        this.equip.Helmet = ArmorToWear;
                        break;
                    }
                    inventory.AddItem(this.equip.Helmet);
                    this.equip.Helmet = ArmorToWear;
                    break;
                case ArmorType.Нагрудник:
                    if(this.equip.Breastplate==null)
                    {
                        this.equip.Breastplate = ArmorToWear;
                        break;
                    }
                    this.inventory.AddItem(this.equip.Breastplate);
                    this.equip.Breastplate = ArmorToWear;
                    break;
                case ArmorType.Наплечник:
                    if (this.equip.Shoulders == null)
                    {
                        this.equip.Shoulders = ArmorToWear;
                        break;
                    }
                    this.inventory.AddItem(this.equip.Shoulders);
                    this.equip.Shoulders = ArmorToWear;
                    break;
                case ArmorType.Брюки:
                    if (this.equip.Pants == null)
                    {
                        this.equip.Pants = ArmorToWear;
                        break;
                    }
                    this.inventory.AddItem(this.equip.Pants);
                    this.equip.Pants = ArmorToWear;
                    break;
                case ArmorType.Черевики:
                    if (this.equip.Boots == null)
                    {
                        this.equip.Boots = ArmorToWear;
                        break;
                    }
                    this.inventory.AddItem(this.equip.Boots);
                    this.equip.Boots = ArmorToWear;
                    break;
                case ArmorType.Рукавиці:
                    if (this.equip.Gloves == null)
                    {
                        this.equip.Gloves = ArmorToWear;
                        break;
                    }
                    this.inventory.AddItem(this.equip.Gloves);
                    this.equip.Gloves = ArmorToWear;
                    break;
                        
            }
        }
        private void ElipseClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
        }
    }
}
