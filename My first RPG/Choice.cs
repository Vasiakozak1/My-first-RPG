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
using System.Reflection;
namespace My_first_RPG
{
    class Choice
    {
        private string offer;
        private Button btn;
        public string Message { get { return this.offer; } }
        public Button Btn { get { return this.btn; } }

        public Choice(string Message,Button button)
        {
            this.offer = Message;
            this.btn = button;
            this.btn.Content = this.offer;
        }

        
        public override string ToString()
        {
            return this.Message;
        }

        //Далі йдуть перегрузки метода DoAction
        #region методи DoAction
        public string DoMethod(Func<string> Method)
        {
            if (Method == null)
            {
                MessageBox.Show("Помилка в string DoAction(Func<string> Method)");
                return "";
            }
            return Method();
        }
        public void DoMethod(Action Method)
        {
            if (Method == null)
            {
                MessageBox.Show("Помилка в string DoAction(Action Method)");
                return;
            }
            Method();
        }
        public string DoMethod(Func<float, Monster, string> Method,float parametr1,Monster parametr2)
        {          
            if (Method == null)
            {
                MessageBox.Show("Помилка в string DoAction(Func<float, Monster, string> Method)");
                return "";
            }
            return Method(parametr1, parametr2);
                
        }
        public string DoMethod(Func<Directions,Place,Monster[],string> Method,Directions Parametr1,Place Parametr2,params Monster[] Parametr3)
        {
            if(Method==null)
            {
                MessageBox.Show("Помилка в string DoAction(Func<Directions,Place,Monster[]> Method,Directions Parametr1,Place Parametr2,params Monster[] Parametr3)");
                return "";
            }
            return Method(Parametr1, Parametr2, Parametr3);
        }
        public string DoMethod(Func<IMonster,string> Method,IMonster parametr1)
        {
            if (Method == null)
            {
                MessageBox.Show("Помилка в DoAction(Func<IMonster,string> Method,IMonster parametr1)");
                return string.Empty;
            }
            return Method(parametr1);
        }
        public bool DoMethod(Func<Poligone,bool> Method,Poligone poligone)
        {
            if (Method == null)
            {
                MessageBox.Show("Помилка в DoMethod(Func<Poligone,bool> Method,Poligone poligone)");
                return false;
            }
            return Method(poligone);
        }
        public string DoMethod(Func<string,string,string> Method,string Parametr1,string Parametr2)
        {
            if (Method == null)
            {
                MessageBox.Show("Помилка в DoMethod(Func<string,string,string> Method,string Parametr1,string Parametr2)");
                return "";
            }
            return Method(Parametr1, Parametr2);
        }
        #endregion
    }
}
