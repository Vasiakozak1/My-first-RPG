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
using System.Collections;

namespace My_first_RPG
{
    /// <summary>
    /// Interaction logic for ShowLootWindow.xaml
    /// </summary>
    public partial class ShowLootWindow : Window
    {
        delegate void MyDelegate(object sender, EventArgs e);

        Dictionary<int, Item> LocalDrop;
        Dictionary<int, Item> GlobalDrop;
        PlayersInventory inventory;
        List<Item> ResultLoot;
        float Cooficient;
        public ShowLootWindow(Dictionary<int,Item> localDrop,Dictionary<int,Item> globalDrop,PlayersInventory inventory,float cooficient)
        {
            InitializeComponent();
            this.LocalDrop = localDrop;
            this.GlobalDrop = globalDrop;
            this.Cooficient = cooficient;
            this.ResultLoot = new List<Item>();
            this.inventory = inventory;

            this.GenerateLoot();
            this.ShowLoot();
        }
        private void GenerateLoot()
        {
            Random Rnd = new Random();
            if (Rnd.Next(0, 1) == 0)
                GlobalDrop.Reverse();

            foreach(KeyValuePair<int,Item> pair in GlobalDrop)
            {
                int percent = Rnd.Next(0, 100);
                if (percent < 100- (pair.Key / this.Cooficient))
                    continue;
                ResultLoot.Add(pair.Value);
            }
            foreach(KeyValuePair<int,Item> pair in LocalDrop)
            {
                int percent = Rnd.Next(0, 100);
                if (percent < 100- pair.Key)
                    continue;
                ResultLoot.Add(pair.Value);

                
            }
        }
        private void ShowLoot()
        {
            Queue<Item> ItemsQueue = new Queue<Item>();
            foreach (Item item in this.ResultLoot)
                ItemsQueue.Enqueue(item);
            
            int countOfItems = 0;
            while (ItemsQueue.Count != 0)
            {
                Item tempItem = ItemsQueue.Dequeue();
                Weapon tempWeapon;
                Armor tempArmor;

                DockPanel panel = new DockPanel();
                panel.Margin = new Thickness(0, 0, 0, 0);
                panel.Height = 50;

                //Обробника нажаття кнопки на мишці
                panel.MouseDown += (sender, e) =>
                {
                    Image temp;
                    DockPanel wrp = sender as DockPanel;
                    foreach (UIElement uiel in wrp.Children)
                        if (uiel is Image)
                        {
                            temp = uiel as Image;
                            foreach (Item itm in this.ResultLoot)
                            {
                                int start1 = temp.Source.ToString().LastIndexOf('/');
                                string tmp1 = "";
                                for (int i = start1 + 1; i < temp.Source.ToString().Length; i++) 
                                    tmp1 += temp.Source.ToString()[i];

                                int start2 = itm.PathIconOfItem.LastIndexOf(@"\");
                                string tmp2 = "";
                                for (int j = start2 + 1; j < itm.PathIconOfItem.Length; j++)
                                    tmp2 += itm.PathIconOfItem[j];
                                // Перевірія чи однакові картинки
                                if (tmp1 == tmp2) 
                                {
                                    this.inventory.AddItem(itm);
                                    break;
                                }
                            }
                            break;
                        }
                    PanelList.Children.Remove(sender as DockPanel);
                };
                if (countOfItems % 2 == 0)
                    panel.Background = Brushes.Gray;
                else
                    panel.Background = Brushes.LightGray;

                Image img = new Image();
                
                img.Height = 50;
            
                img.Margin = new Thickness(0, 0, 90, 0);
                img.Source = new BitmapImage(new Uri(tempItem.PathIconOfItem, UriKind.RelativeOrAbsolute));
                this.RegisterName("img" + countOfItems, img);
                panel.Children.Add(img);

                TextBlock blockOfText = new TextBlock();
                blockOfText.Margin = new Thickness(-90, 0, 0, 0);
                blockOfText.TextWrapping = TextWrapping.Wrap;
                
                
                if (tempItem is Weapon)
                {
                    tempWeapon = tempItem as Weapon;
                    blockOfText.Text = string.Format
                        ($"{tempWeapon.Name} урон {tempWeapon.MinDamage}-{tempWeapon.MaxDamage} {tempWeapon.WeaponType} швидкiсть {tempWeapon.Speed} цiна {tempWeapon.PureWorth}");
                }
                else if(tempItem is Armor)
                {
                    tempArmor = tempItem as Armor;
                    blockOfText.Text = string.Format
                        ($"{tempArmor.Name} броня {tempArmor.ProtectionPoints} {tempArmor.Type} цiна {tempArmor.PureWorth}");

                }
                else
                    blockOfText.Text = string.Format($"{tempItem.Name} цiна {tempItem.PureWorth}");
                this.RegisterName("textbl" + countOfItems, blockOfText);
                panel.Children.Add(blockOfText);

                this.RegisterName("dockpanel" + countOfItems, panel);
                PanelList.Children.Add(panel);

             
                countOfItems++;
            }
        }
    }
}
