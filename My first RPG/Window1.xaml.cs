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
using System.Media;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace My_first_RPG
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        Warrior player = null;
        GameWindow game = null;
        public Window1()
        {
            
            InitializeComponent();
            
            Btn_Create.IsEnabled = false;

         
            
        }
        #region Гавно, не знаю як це правильно видалити...
        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {

        }
        #endregion
        private void TbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Btn_Create.IsEnabled = true;
        }
        private Warrior InitializeWarrior()
        {
            Warrior Player = null;
            BinaryFormatter load = new BinaryFormatter();
            FileStream file = null;
            if ((bool)radioButton1.IsChecked)
            {
                try
                {
                    Weapon firstWeapon = null;
                    file = new FileStream(@"Weapons\Rock.dat", FileMode.Open);

                    object tmp = load.Deserialize(file);
                    firstWeapon = (Weapon)tmp;
                    file = new FileStream(@"MiniLocations\1.SilentValley.dat", FileMode.Open);

                    tmp = load.Deserialize(file);
                    MiniLocation StartMiniLocation = (MiniLocation)tmp;

                    Wolf CommonWolf = new Wolf("Чахлий вовк", 1, 45, "1-2", 3.0f);


                    Place StartPlace = new Place(new MyPoint(0, 0), CommonWolf, CommonWolf, CommonWolf, CommonWolf, CommonWolf);// і так покищо сойдьот xD

                    Player = new Warrior(TbName.Text, StartMiniLocation,StartPlace);
                }
                catch (DirectoryNotFoundException exc)
                {
                    MessageBox.Show(@"Не знайдено файл Weapons\Rock... \n" + exc.HelpLink);
                }
                catch (System.Runtime.Serialization.SerializationException exc)
                {
                    MessageBox.Show("Проблеми iз серiалiзацiєю... \n" + exc.HelpLink + exc.Message);
                }
                finally
                {
                    file.Close();
                }
            }
            return Player;
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo dirinfo = Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Saves");
            
            SoundPlayer play = new SoundPlayer();                       
            play.SoundLocation = "Zapus.wav";
            play.Play();

            player = InitializeWarrior();
            FileStream fs = new FileStream(dirinfo.FullName +@"\" +player.Name+".dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, player);
            fs.Close();
            game = new GameWindow(player.Name);
            game.Show();
            this.Hide();
        }
    }
}
