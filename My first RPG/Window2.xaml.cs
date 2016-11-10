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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace My_first_RPG
{
    class SerializeHistory
    {
        public static void First()
        {
            DirectoryInfo folder = Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Weapons");
            

            FileStream writeobject = new FileStream(folder.FullName + @"\Rock.dat", FileMode.Create);
            BinaryFormatter Serialization = new BinaryFormatter();
            Weapon rock = new Weapon("Камiнь", "1-2", WeaponType.Інше, 1, 1, 2.1f, 2, 10,@"Resourses\l_Rock01.png");
            Serialization.Serialize(writeobject, rock);
            writeobject.Close();
            MessageBox.Show("Серіалізовано в" + folder.FullName + @"\Rock.dat");
            
        }
        public static void Second()
        {
            DirectoryInfo folder = Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\MiniLocations");
            FileStream Serialization = new FileStream(folder.FullName + @"\1.SilentValley.dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            Poligone area = Poligone.CreatePoligone(new MyPoint(15, 210), new MyPoint(-95, -15), new MyPoint(5, -145), new MyPoint(65, 24));

            Monster gobl_work = null;Monster sick_wolf = null;
            using (FileStream fs = new FileStream(@"Mobs\1.Goblin_worker.dat", FileMode.Open))
                gobl_work = bf.Deserialize(fs) as Monster;
            using (FileStream fs = new FileStream(@"Mobs\1.Sick_wolf.dat", FileMode.Open))
                sick_wolf = bf.Deserialize(fs) as Monster;

            MiniLocation StartLocation = new MiniLocation("Тиха долина", "1-1", area,gobl_work,sick_wolf);
            StartLocation.GlobalDropList.Add(4, new Item("Блистючий камiнь", 20, 3, @"Resourses\Opal.png"));
            
            bf.Serialize(Serialization, StartLocation);
            Serialization.Close();
            MessageBox.Show("Серіалізовано в" + folder.FullName + @"\1.SilentValley.dat");
        }

        #region Моби та айтеми для першої мінілокації

        public static void Third()
        {
            DirectoryInfo folder = Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Mobs");
            FileStream Serialization = null;
            using (Serialization = new FileStream(folder.FullName + @"\1.Goblin_worker.dat", FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                Weapon GoblinShowel = new Weapon("Iржава гоблiнська лопата", "1-3", WeaponType.Двуручний_молот, 1, 3, 6.1f, 2, 10, @"Resourses\shovel1.png");
                Item GoblinIdol = new Item("Iдол габлiна", 4, 1, @"Resourses\GobIdol.png");
                Item FrogsLeg = new Item("Жаб`яча лапка", 1, 1, @"Resourses\FrogLeg1.png");
                Dictionary<int, Item> loot = new Dictionary<int, Item>();
                loot.Add(29, GoblinShowel); loot.Add(10, GoblinIdol); loot.Add(15, FrogsLeg);
                Monster Goblin1 = new Monster("Гоблiн-робочий", 1, 52, "2-3", 6.1f, 1f, loot);
                bf.Serialize(Serialization, Goblin1);

                MessageBox.Show("Серiалiзовано " + Goblin1.ToString());
            }
            
        }

        public static void SickWolf()
        {
            DirectoryInfo folder = Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\Mobs");
            FileStream Serialization = null;
            using(Serialization = new FileStream(folder.FullName + @"\1.Sick_wolf.dat", FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();

                Item BadWolfSkin = new Item("Рвана шкура вовка", 4, 5, @"Resourses\WolfFur.png");
                Item WolfFang = new Item("Iкло вовка", 3, 1, @"Resourses\Fang.png");
                Dictionary<int, Item> mobsLoot = new Dictionary<int, Item>();
                mobsLoot.Add(85, WolfFang); mobsLoot.Add(40, BadWolfSkin);
                Monster sickWolf = new Monster("Хворий вовк", 1, 50, "1-3", 5.1f, 1.0f, mobsLoot);
                bf.Serialize(Serialization, sickWolf);
                MessageBox.Show("Серiалiзовано " + sickWolf.ToString());
            }
        }

        #endregion
    }

    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    /// 
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }
        #region
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox1.Text == "wryw339v")
                Btn_Serialize.IsEnabled = true;
            else
                Btn_Serialize.IsEnabled = false;
        }
        #endregion

        private void Btn_Serialize_Click(object sender, RoutedEventArgs e)
        {
            SerializeHistory.Third();
            SerializeHistory.SickWolf();
            SerializeHistory.Second();
        }
    }
}
