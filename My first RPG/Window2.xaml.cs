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
            MiniLocation StartLocation = new MiniLocation("Тиха долина", "1-1", area);
            bf.Serialize(Serialization, StartLocation);
            Serialization.Close();
            MessageBox.Show("Серіалізовано в" + folder.FullName + @"\1.SilentValley.dat");
        }
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
            SerializeHistory.Second();
        }
    }
}
