using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using My_first_RPG;
using System.IO;
namespace SerializationObjects
{
    
    /// <summary>
    /// Використовується для створення об'єктів і їх серіалізації
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {


            SerializeHistory.First();
            DirectoryInfo folder = Directory.CreateDirectory(@"C:\Users\jungl\Documents\Visual Studio 2015\Projects\My first RPG\My first RPG\bin\Debug\Weapons");
            
            Console.ReadLine();

        }
    }
}
