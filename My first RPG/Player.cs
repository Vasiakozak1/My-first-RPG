using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
namespace My_first_RPG
{
    
    public interface IPlayer
    {
        string Name { get; }
        ushort Level { get; }
        uint Health { get; }
        uint Exp { get; }
        uint MaxExpInLevel { get; }
        bool IsAlive { get; }
        MiniLocation CurrentLocation { get; }
        Place CurrentPlace { get; }
        uint MaxHealth { get; }

        void UpLevel();
        void AddExp(uint Exp);
        string Die(string Reason);
        string Die(string Reason, string Who);
        string ReduseHealth(float HomMuch, string Reason);
        void ReduseHealth(float HowMuch);
        string EnterMiniLocation(MiniLocation NextLoc,MiniLocation PrevLoc);
        string EnterMiniLocation(MiniLocation NextLoc);
        
    }
    /*
     * Треба добавити абстрактний клас Ремесло
     Можна добавити методи та властивості крафту: 1) SearchGoods(Place місцеумінілокакії, )
    */
   [Serializable]
    public abstract class Player : IPlayer
    {
        #region Поля
        protected string name;
        protected ushort level;
        protected uint health;
        protected uint exp;
        protected uint maxExpInLevel;
        protected bool isalive;
        protected uint maxhealth;
        protected MiniLocation currentminilocation;// Локація, де зараз перебуває перс
        protected Place currentplace;//Місце, де зараз перебуває персонаж
        #endregion
        #region Властивості
        public uint Exp
        {
            get { return this.exp; }
        }

        public ushort Level
        {
            get { return this.level; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public bool IsAlive
        {
            get { return this.isalive; }
        }

        public uint MaxExpInLevel
        {
            get { return this.maxExpInLevel; }
        }

        public uint Health
        {
            get { return this.health; }
        }

        public MiniLocation CurrentLocation
        {
            get { return this.currentminilocation; }
        }

        public Place CurrentPlace
        {
            get { return this.currentplace; }
        }

        public uint MaxHealth
        {
            get { return this.maxhealth; }
        }
        #endregion

        public Player(string Name,MiniLocation StartLocation,Place StartPlace)
        {
            this.exp = 0;
            this.isalive = true;
            this.level = 1;
            this.name = Name;
            this.maxExpInLevel = 100;
            
        }


        public void AddExp(uint Exp)
        {
            this.exp += Exp;
            CheckLevel();
        }
        private void CheckLevel()
        {
            if (this.Exp >= this.MaxExpInLevel)
                UpLevel();
        }

        public string Die(string Reason)
        {
            this.isalive = false;
            return string.Format("Гравець {0} помер {1}", this.Name, Reason);         
        }

        public string Die(string Reason, string Who)
        {
            this.isalive = false;
            return string.Format("Гравець {0} помер вiд рук {1}-а(я) {2}", this.Name, Who, Reason);
        }

        public void UpLevel()
        {
            this.level++;
            this.maxExpInLevel = (uint)(maxExpInLevel * Math.PI / 1.7);
        }
        

        public string ReduseHealth(float HowMuch, string Reason)
        {
            this.health -= (uint)HowMuch;

            if (this.health <= 0)
                this.isalive = false;
            return string.Format("Гравець {0} отримав {1} по причинi:{2}", this.Name, HowMuch, Reason);
        }

        public void ReduseHealth(float HowMuch)
        {
            this.health -= (uint)HowMuch;

            if (this.health <= 0)
                this.isalive = false;
        }


        public bool IsInTheMiniLocation(Poligone PoligoneOfMiniLocation)
        {
            
            int Quarter = 0;
            if (this.currentplace.Coordinats.X > 0 && this.currentplace.Coordinats.Y > 0)
                Quarter = 1;
            else if (this.currentplace.Coordinats.X > 0 && this.currentplace.Coordinats.Y < 0)
                Quarter = 4;
            else if (this.currentplace.Coordinats.X < 0 && this.currentplace.Coordinats.Y < 0)
                Quarter = 3;
            else if (this.currentplace.Coordinats.X < 0 && this.currentplace.Coordinats.Y > 0)
                Quarter = 2;
            switch (Quarter)
            { 
                case 1:
                    if (PoligoneOfMiniLocation.A.X + PoligoneOfMiniLocation.A.Y <= (this.currentplace.Coordinats.X + this.currentplace.Coordinats.Y))
                        return false;
                    break;
                case 4:
                    if (PoligoneOfMiniLocation.D.X + (-PoligoneOfMiniLocation.D.Y) <= (this.currentplace.Coordinats.X + (-this.currentplace.Coordinats.Y)))
                        return false;
                    break;
                case 3:
                    if (-PoligoneOfMiniLocation.C.X + (-PoligoneOfMiniLocation.C.Y) >= (this.currentplace.Coordinats.X + (this.currentplace.Coordinats.Y))) 
                        return false;
                    break;
                case 2:
                    if (-PoligoneOfMiniLocation.B.X + PoligoneOfMiniLocation.B.Y >= (this.currentplace.Coordinats.X + this.currentplace.Coordinats.Y))
                        return false;
                    break;
                
            }
            return true;
             
        }

        public abstract string Attack(Monster monster);
        public abstract string TakePhisicalDamageFromMonster(float Damage, Monster FromWho);
        public abstract string Move(Directions CelectedDirection,Place PrevPlace,params Monster[] Monsters);
        public abstract void RestoreHealth(uint Count);
        public string EnterMiniLocation(MiniLocation CurrLoc)
        {
            throw new NotImplementedException();
        }

        public string EnterMiniLocation(MiniLocation NextLoc, MiniLocation PrevLoc)
        {
            throw new NotImplementedException();
        }
    }
    [Serializable]
    public class Warrior : Player
    {
        
        float totalSpeed;
        ushort fury;
        Weapon currentWeapon;
        public ushort Fury { get { return this.fury; } }
        public Weapon CurrentWeapon { get { return this.currentWeapon; } }
        public float TotalSpeed { get { return this.totalSpeed; } }

        public Inventory Inventory { get; private set; }
        public Equipment Equip { get; private set; }

        public Warrior(string Name, Weapon StartWeapon, MiniLocation StartLocation, Place StartPlace) : base(Name,StartLocation,StartPlace)
        {
            this.health = 65;
            this.maxhealth = this.health;
            this.fury = 0;
            this.currentWeapon = StartWeapon;
            this.currentminilocation = StartLocation;
            this.currentplace = StartPlace;
            this.totalSpeed = 500 * this.CurrentWeapon.Speed;
            Inventory = new Inventory();
            Equip = new Equipment(this);
        }
        

        /// <summary>
        /// Цей метод може бути перероблений або перегружений. Може враховуватися швидкість самого героя, броня, ухилення противника і т.д
        /// </summary>
        /// <param name="monster">Моб</param>
        public override string Attack(Monster Enemy)
        {
            
            
            string Message = Enemy.TakePhisicalDamage(new Random().Next((int)currentWeapon.MinDamage, (int)currentWeapon.MaxDamage+1),this);
            this.fury += (ushort)new Random().Next(7,11);
            if (this.fury > 100)
                this.fury = 100;
            return Message;
        }

        /// <summary>
        /// Так само як public override void Attack(IMonster monster) може бути перероблений або перегружений
        /// Треба доробити функцію при смерті персонажа, завершити гру, чи воскресіння на кладовищі
        /// </summary>
        /// <param name="HowMany">Урон</param> 
        /// <param name="FromWho">Моб</param> 
        /// <returns></returns>
        public override string TakePhisicalDamageFromMonster(float HowMany, Monster FromWho)
        {
            this.health -= (uint)HowMany;

            if (this.health <= 0)
            {
                this.isalive = false;
                this.health = 0;
            }
            return string.Format("Гравець {0} отримав {1} шкоди вiд {2}-а(я)", this.Name, HowMany, FromWho.Name);
        }

        public override string Move(Directions CelectedDirection,Place PrevPlace,params Monster[] Monsters)
        {
            Place NewPlace;
            MyPoint NewCoords = null; 
            switch (CelectedDirection)
            {
                case Directions.Північ:
                    NewCoords = new MyPoint(PrevPlace.Coordinats.X, PrevPlace.Coordinats.Y + 30);
                    break;
                case Directions.Південь:
                    NewCoords = new MyPoint(PrevPlace.Coordinats.X, PrevPlace.Coordinats.Y - 30);
                    break;
                case Directions.Захід:
                    NewCoords = new MyPoint(PrevPlace.Coordinats.X - 30, PrevPlace.Coordinats.Y);
                    break;
                case Directions.Схід:
                    NewCoords = new MyPoint(PrevPlace.Coordinats.X + 30, PrevPlace.Coordinats.Y);
                    break;

            }
            if (Monsters.Length == 0)
            {
                NewPlace = new Place(NewCoords);               
            }
            else
            {
                NewPlace = new Place(NewCoords, Monsters);                
            }
            this.currentplace = NewPlace;
            return string.Format("Герой пiшов на {0}, тепер вашi координати:{1},{2}", CelectedDirection.ToString(), NewCoords.X, NewCoords.Y);
        }

        public override void RestoreHealth(uint Count)
        {
            this.health += Count;
            if (this.health > this.maxhealth)
                this.health = this.maxhealth;
        }
    }

    /// <summary>
    /// Покищо не реалізовано
    /// </summary>
    class Priest : Player
    {
        public Priest(string Name, MiniLocation StartLocation, Place StartPlace) : base(Name, StartLocation, StartPlace)
        {
        }

        public override string Attack(Monster monster)
        {
            throw new NotImplementedException();
        }

       

        public override string Move(Directions CelectedDirection, Place PrevPlace, params Monster[] Monsters)
        {
            throw new NotImplementedException();
        }

        public override void RestoreHealth(uint Count)
        {
            throw new NotImplementedException();
        }

        public override string TakePhisicalDamageFromMonster(float Damage, Monster FromWho)
        {
            throw new NotImplementedException();
        }
    }


    public enum Directions
    {
        Північ,
        Південь,
        Захід,
        Схід
    }
}
