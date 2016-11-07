using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace My_first_RPG
{

    /// <summary>
    /// Важливий клас, в якому виконуються основні дії гравця
    /// </summary>
    ///
    [Serializable]
    public class Place
    {
        public Place(MyPoint NewCoord,MiniLocation Location,params Monster[] NewMobs)
        {
            this.Coordinats = NewCoord;
            this.Mobs = NewMobs.ToList();
            int CountOfMobs = new Random().Next(2, 6);
            for(int i = 0; i < CountOfMobs; i++)
            {
                this.Mobs.Add(Location.ListOfMonsters[new Random().Next(0, Location.ListOfMonsters.Count + 1)]);
            }
        }

        public MyPoint Coordinats { get; private set; }

        public List<Monster> Mobs { get; private set; }
        /*
         Треба буде добавити ще шось, наприклад шось зв'язане із квестами, наприклад якийсь евент або поява рідкісного моба;
         Ще можна добавити шось зв'язане із збором ресів
         */
        
     }

    // Можна створити клас Place місце, де иожуть бути моби, якийсь посьолок з НПС, травами, іншими об'єктами з якими можна взаємодіяти
    interface IMiniLocation
    {
        string Name { get; }
        uint MinLevelMobs { get; }
        uint MaxlevelMobs { get; }

        string EnterLocation();
        string MoveIn();// Треба буде добавити аргументи, наприклад:   (в сторону якої локації, і ще шось в перспективі) Хі-хі)))) 
        Dictionary<int,Item> GlobalDropList { get; }
        List<Monster> ListOfMonsters { get; }
    }

    [Serializable]
    public class MyPoint
    {
        public int X;
        public int Y;
        public MyPoint(int x,int y)
        {
            Y = y;
            X = x;
        }
    }

    [Serializable]
    public struct Poligone
    {
        // Є 4 точки полігону
        public MyPoint A { get; private set; }
        public MyPoint B { get; private set; }
        public MyPoint C { get; private set; }
        public MyPoint D { get; private set; }
        public static Poligone CreatePoligone(MyPoint A, MyPoint B, MyPoint C, MyPoint D)
        {
            Poligone tmp = new Poligone();
            tmp.A = A;
            tmp.B = B;
            tmp.C = C;
            tmp.D = D;
            return tmp;
        }
        
    }
    [Serializable]
    public class MiniLocation : IMiniLocation
    {
        /// <summary>
        /// Item - це предмет який може випасти з любого моба у цій локації
        /// int - це шанс випадіння
        /// </summary>
        

        private string name;
        private uint minlvlmobs;
        private uint maxlvlmobs;
        private List<Place> SpecialPlaces;//Місця із іменними мобами, квестовими предметами і т.д
        private Dictionary<int, Item> globalDrop;
        private Poligone poligoneOfMiniLocation;
        private List<Monster> monsterslist;

        public MiniLocation(string Name,string MinMaxLvlsMobs,Poligone PoligoneOfMiniLocation,params Monster[] MobsInThisLocation)
        {   
            this.name = Name;
            this.poligoneOfMiniLocation = PoligoneOfMiniLocation;
            string[] minmaxlvls = MinMaxLvlsMobs.Split('-');
            this.minlvlmobs = uint.Parse(minmaxlvls[0]);
            this.maxlvlmobs = uint.Parse(minmaxlvls[1]);

            this.monsterslist = new List<Monster>(MobsInThisLocation);
        }


        

        public Poligone PoligoneOfMiniLocation
        {
            get { return this.poligoneOfMiniLocation; }
        }

        public uint MaxlevelMobs
        {
            get { return this.maxlvlmobs; }
        }

        public uint MinLevelMobs
        {
            get { return this.minlvlmobs; }
        }

        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Ці предмети падають з любого моба в цій локації
        /// </summary>
        public Dictionary<int,Item> GlobalDropList { get { return this.globalDrop; } }

        public List<Monster> ListOfMonsters { get { return this.monsterslist; } }

        public string EnterLocation()
        {
            throw new NotImplementedException();
        }

        public string MoveIn()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// У малій локації:1-3 підлокації, середня-4-7, велика 7-10
    /// </summary>
    class Location
    {
        private string name;
        private List<MiniLocation> minilocations;


        public MiniLocation this[int index]
        {
            get { return minilocations[index]; }
           private set { minilocations[index] = value; }
        }


        /// <summary>
        /// Можна розширити або перегрузити конструктор
        /// </summary>
        /// <param name="Name">Назва локації</param>
        /// <param name="locations">Мінілокації вказуються через кому, бо - params</param>
        public Location(string Name, params MiniLocation[] locations)
        {
            this.minilocations = locations.ToList();
            this.name = Name;
            

        }
        
        public string Name { get { return this.name; } }

        /// <summary>
        /// Показує скільки мінілокацій у об'єкті даної локації
        /// </summary>
        public uint Size { get { return (uint)minilocations.Count; } }
    }
    
}
