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
    [Serializable]
    public class Equipment
    {
        public Armor Helmet { get; set; }//Шолом
        public Armor Breastplate { get; set; }//Нагрудник
        public Armor Shoulders { get; set; }//Наплечниики
        public Armor Pants { get; set; }//Штани
        public Armor Gloves { get; set; }//Рукавиці
        public Armor Boots { get; set; }//Черевики
        public Weapon CurrentWeapon { get; set; }//Зброя

        public uint TotalProtectionPoint { get; private set; }

        public Player Owner;

        public Equipment(Player Owner)
        {
            this.Boots = new Armor("Обмотки", 1, 1, @"Resourses\A_Shoes01.png", ArmorType.Черевики, 0, ItemActions.Викинути, ItemActions.Зняти);
            this.Pants = new Armor("Ножні обиотки", 1, 1, @"Resourses\MyPants.png", ArmorType.Брюки, 1, ItemActions.Викинути, ItemActions.Зняти);
            this.CurrentWeapon = new Weapon("Камiнь", "1-2", WeaponType.Інше, 1, 1, 4.5f, 5, 10, @"Resourses\Rock01.png");
            this.TotalProtectionPoint = this.Boots.ProtectionPoints + this.Pants.ProtectionPoints;
            this.Owner = Owner;
        }
        
    }

    public partial class WindowEquipment : Window
    {
        private Equipment equip;
        private PlayersInventory inventory;

        private void RefreshImages()
        {
            //tmp.MouseRightButtonDown += this.ActionsOnItem; В Добавляється подія в бордер, якщо в ньому є Image із картинкою
            if (this.equip.Helmet != null)
            {
                Border tmp = this.HelmetImage.Parent as Border;
                tmp.Opacity = 100;
                tmp.MouseRightButtonDown += this.ShowActions;
                this.HelmetImage.Source = new BitmapImage(new Uri(this.equip.Helmet.PathIconOfItem, UriKind.RelativeOrAbsolute));
            }
                

            if (this.equip.Breastplate != null)
            {
                Border tmp = this.BreastPlateImage.Parent as Border;
                tmp.Opacity = 100;
                tmp.MouseRightButtonDown += this.ShowActions;
                this.BreastPlateImage.Source = new BitmapImage(new Uri(this.equip.Breastplate.PathIconOfItem, UriKind.RelativeOrAbsolute));
            }
                

            if (this.equip.Shoulders != null)
            {
                Border tmp = this.ShoulderLeftImage.Parent as Border;
                tmp.Opacity = 100;
                tmp.MouseRightButtonDown += this.ShowActions;
                tmp = this.ShoulderRightImage.Parent as Border;
                tmp.Opacity = 100;
                tmp.MouseRightButtonDown += this.ShowActions;
                this.ShoulderLeftImage.Source = new BitmapImage(new Uri(this.equip.Shoulders.PathIconOfItem, UriKind.RelativeOrAbsolute));
                this.ShoulderRightImage.Source = new BitmapImage(new Uri(this.equip.Shoulders.PathIconOfItem, UriKind.RelativeOrAbsolute));
            }
             
             if(this.equip.Pants != null)
            {
                Border tmp = this.PantsImage.Parent as Border;
                tmp.Opacity = 100;
                tmp.MouseRightButtonDown += this.ShowActions;
                this.PantsImage.Source = new BitmapImage(new Uri(this.equip.Pants.PathIconOfItem, UriKind.RelativeOrAbsolute));
            }
                

            if (this.equip.Boots != null)
            {
                this.BootsLeftImage.Source = new BitmapImage(new Uri(this.equip.Boots.PathIconOfItem, UriKind.RelativeOrAbsolute));
                Border tmp = BootsLeftImage.Parent as Border;
                tmp.Opacity = 100;
                tmp.MouseRightButtonDown += this.ShowActions;
                this.BootsRightImage.Source = new BitmapImage(new Uri(this.equip.Boots.PathIconOfItem, UriKind.RelativeOrAbsolute));
                tmp = BootsRightImage.Parent as Border;
                tmp.Opacity = 100;
                tmp.MouseRightButtonDown += this.ShowActions;
            }

            if (this.equip.Gloves != null)
            {
                Border tmp = this.GlovesLeftImage.Parent as Border;
                tmp.Opacity = 100;
                tmp.MouseRightButtonDown += this.ShowActions;
                tmp = this.GlovesRightImage.Parent as Border;
                tmp.Opacity = 100;
                tmp.MouseRightButtonDown += this.ShowActions;
                this.GlovesLeftImage.Source = new BitmapImage(new Uri(this.equip.Gloves.PathIconOfItem, UriKind.RelativeOrAbsolute));
                this.GlovesRightImage.Source = new BitmapImage(new Uri(this.equip.Gloves.PathIconOfItem, UriKind.RelativeOrAbsolute));
            }

            if (this.equip.CurrentWeapon != null)
            {
                Border tmp = this.WeaponImage.Parent as Border;
                tmp.Opacity = 100;
                tmp.MouseRightButtonDown += this.ShowActions;
                this.WeaponImage.Source = new BitmapImage(new Uri(this.equip.CurrentWeapon.PathIconOfItem, UriKind.RelativeOrAbsolute));
            }
           
        }
        public WindowEquipment(PlayersInventory Inventory, Equipment Equip)
        {
            InitializeComponent();                     
            this.equip = Equip;
            this.inventory = Inventory;
            this.RefreshImages();
            this.labelName.Content = this.equip.Owner.Name;
        }
        public void Wear(Armor ArmorToWear)
        {   
            Border tmp;
            switch (ArmorToWear.Type)
            {               
                case ArmorType.Шолом:
                    this.HelmetImage.Source = new BitmapImage(new Uri(ArmorToWear.PathIconOfItem, UriKind.RelativeOrAbsolute));
                    tmp = this.HelmetImage.Parent as Border;
                    tmp.MouseRightButtonDown += this.ShowActions;
                    tmp.Opacity = 100;
                    if (this.equip.Helmet == null)
                    {
                        this.equip.Helmet = ArmorToWear;
                        break;
                    }
                    
                    this.equip.Helmet = ArmorToWear;
                    break;
                case ArmorType.Нагрудник:
                    this.BreastPlateImage.Source = new BitmapImage(new Uri(ArmorToWear.PathIconOfItem, UriKind.RelativeOrAbsolute));
                    tmp = this.BreastPlateImage.Parent as Border;
                    tmp.MouseRightButtonDown += this.ShowActions;
                    tmp.Opacity = 100;
                    if (this.equip.Breastplate==null)
                    {
                        this.equip.Breastplate = ArmorToWear;
                        break;
                    }
                    
                    this.equip.Breastplate = ArmorToWear;
                    break;
                case ArmorType.Наплечник:
                    this.ShoulderLeftImage.Source = new BitmapImage(new Uri(ArmorToWear.PathIconOfItem, UriKind.RelativeOrAbsolute));
                    this.ShoulderRightImage.Source = new BitmapImage(new Uri(ArmorToWear.PathIconOfItem, UriKind.RelativeOrAbsolute));
                    tmp = this.ShoulderLeftImage.Parent as Border;
                    tmp.MouseRightButtonDown += this.ShowActions;
                    tmp.Opacity = 100;
                    tmp = this.ShoulderRightImage.Parent as Border;
                    tmp.MouseRightButtonDown += this.ShowActions;
                    tmp.Opacity = 100;
                    if (this.equip.Shoulders == null)
                    {
                        this.equip.Shoulders = ArmorToWear;
                        break;
                    }
                    this.equip.Shoulders = ArmorToWear;
                    break;
                case ArmorType.Брюки:
                    this.PantsImage.Source = new BitmapImage(new Uri(ArmorToWear.PathIconOfItem, UriKind.RelativeOrAbsolute));
                    tmp = this.PantsImage.Parent as Border;
                    tmp.MouseRightButtonDown += this.ShowActions;
                    tmp.Opacity = 100;
                    if (this.equip.Pants == null)
                    {
                        this.equip.Pants = ArmorToWear;
                        break;
                    }
                    this.equip.Pants = ArmorToWear;
                    break;
                case ArmorType.Черевики:
                    this.BootsLeftImage.Source = new BitmapImage(new Uri(ArmorToWear.PathIconOfItem, UriKind.RelativeOrAbsolute));
                    this.BootsRightImage.Source = new BitmapImage(new Uri(ArmorToWear.PathIconOfItem, UriKind.RelativeOrAbsolute));
                    tmp = this.BootsLeftImage.Parent as Border;
                    tmp.MouseRightButtonDown += this.ShowActions;
                    tmp.Opacity = 100;
                    tmp = this.BootsRightImage.Parent as Border;
                    tmp.MouseRightButtonDown += this.ShowActions;
                    tmp.Opacity = 100;
                    if (this.equip.Boots == null)
                    {
                        this.equip.Boots = ArmorToWear;
                        break;
                    }
                    this.equip.Boots = ArmorToWear;
                    break;
                case ArmorType.Рукавиці:
                    this.GlovesLeftImage.Source = new BitmapImage(new Uri(ArmorToWear.PathIconOfItem, UriKind.RelativeOrAbsolute));
                    this.GlovesRightImage.Source = new BitmapImage(new Uri(ArmorToWear.PathIconOfItem, UriKind.RelativeOrAbsolute));
                    tmp = this.GlovesLeftImage.Parent as Border;
                    tmp.MouseRightButtonDown += this.ShowActions;
                    tmp.Opacity = 100;
                    tmp = this.GlovesRightImage.Parent as Border;
                    tmp.MouseRightButtonDown += this.ShowActions;
                    tmp.Opacity = 100;
                    if (this.equip.Gloves == null)
                    {
                        this.equip.Gloves = ArmorToWear;
                        break;
                    }
                    this.equip.Gloves = ArmorToWear;
                    break;
                        
            }
        }

        public void Wear(Weapon WeaponToWear)
        {
            this.WeaponImage.Source = new BitmapImage(new Uri(WeaponToWear.PathIconOfItem,UriKind.RelativeOrAbsolute));
            Border tmp = this.WeaponImage.Parent as Border;
            tmp.MouseRightButtonDown += this.ShowActions;
            tmp.Opacity = 100;
            if (this.equip.CurrentWeapon == null)
            {
                this.equip.CurrentWeapon = WeaponToWear;
                return;
            }
            this.inventory.AddItem(this.equip.CurrentWeapon);
            this.equip.CurrentWeapon = WeaponToWear;
            
        }

        /// <summary>
        /// Зняти предмет з гравця
        /// </summary>
        /// <param name="ArmorToUnwear"></param>
        public void UnWear(Armor ArmorToUnwear)
        {
            Border br;
            switch (ArmorToUnwear.Type)
            {
                case ArmorType.Шолом:
                    this.equip.Helmet = null;
                    this.HelmetImage.Source = null;
                    br = this.HelmetImage.Parent as Border;
                    br.Opacity = 0.45;
                    break;
                case ArmorType.Нагрудник:
                    this.equip.Breastplate = null;
                    this.BreastPlateImage.Source = null;
                    br = this.BreastPlateImage.Parent as Border;
                    br.Opacity = 0.45;
                    break;
                case ArmorType.Наплечник:
                    this.equip.Shoulders = null;
                    this.ShoulderLeftImage.Source = null;
                    this.ShoulderRightImage.Source = null;
                    br = this.ShoulderLeftImage.Parent as Border;
                    br.Opacity = 0.45;
                    br = this.ShoulderRightImage.Parent as Border;
                    br.Opacity = 0.45;
                    break;
                case ArmorType.Брюки:
                    this.equip.Pants = null;
                    this.PantsImage.Source = null;
                    br = this.PantsImage.Parent as Border;
                    br.Opacity = 0.45;
                    break;
                case ArmorType.Черевики:
                    this.equip.Boots = null;
                    this.BootsLeftImage.Source = null;
                    this.BootsRightImage.Source = null;
                    br = this.BootsLeftImage.Parent as Border;
                    br.Opacity = 0.45;
                    br = this.BootsRightImage.Parent as Border;
                    br.Opacity = 0.45;
                    break;
                case ArmorType.Рукавиці:
                    this.equip.Gloves = null;
                    this.GlovesLeftImage.Source = null;
                    this.GlovesRightImage.Source = null;
                    br = this.GlovesLeftImage.Parent as Border;
                    br.Opacity = 0.45;
                    br = this.GlovesRightImage.Parent as Border;
                    br.Opacity = 0.45;
                    break;

            }
        }

        public void UnWear(Weapon WeaponToUnwear)
        {
            this.equip.CurrentWeapon = null;
            this.WeaponImage.Source = null;
            Border br = this.WeaponImage.Parent as Border;
            br.Opacity = 0.45;
        }

        private void ShowActions(object sender, MouseEventArgs e)
        {
            Border br = sender as Border;
            Image img = br.Child as Image;
            ActionsOnItem ActionsWindow = null; 
            switch (img.Name)
            {
                case "HelmetImage":
                    ActionsWindow = new ActionsOnItem(this.equip.Helmet,this.inventory);
                    break;
                case "BreastPlateImage":
                    ActionsWindow = new ActionsOnItem(this.equip.Breastplate, this.inventory);
                    break;
                case "PantsImage":
                    ActionsWindow = new ActionsOnItem(this.equip.Pants, this.inventory);
                    break;
                case "BootsLeftImage":
                    ActionsWindow = new ActionsOnItem(this.equip.Boots, this.inventory);
                    break;
                case "BootsRightImage":
                    ActionsWindow = new ActionsOnItem(this.equip.Boots, this.inventory);
                    break;
                case "GlovesRightImage":
                    ActionsWindow = new ActionsOnItem(this.equip.Gloves, this.inventory);
                    break;
                case "GlovesLeftImage":
                    ActionsWindow = new ActionsOnItem(this.equip.Gloves, this.inventory);
                    break;
                case "ShoulderRightImage":
                    ActionsWindow = new ActionsOnItem(this.equip.Shoulders, this.inventory);
                    break;
                case "ShoulderLeftImage":
                    ActionsWindow = new ActionsOnItem(this.equip.Shoulders, this.inventory);
                    break;
                case "WeaponImage":
                    ActionsWindow = new ActionsOnItem(this.equip.CurrentWeapon, this.inventory);
                    break;
                default:
                    MessageBox.Show("Помилка");
                    break;
            }
            ActionsWindow.Show();
        }
        private void ElipseClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
        }
    }
}
