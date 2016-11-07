using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace My_first_RPG
{
    public interface IMonster
    {
        string Name { get; }
        uint Level { get; }
        uint Health { get; }
        uint MinDamage { get; }
        uint MaxDamage { get; }
        float AttackSpeed { get; }
        bool IsAlive { get; }
        string AutoAttack(Player player);
        string TakePhisicalDamage(float Damage,IPlayer player);
        Dictionary<int,Item> DropList { get; }
        float DropCoefficient { get; }
        int ExperienceFor { get; }
    }

   [Serializable]
   public class Monster : IMonster
    {
        #region Поля
        protected string name;
        protected uint level;
        protected uint health;
        protected uint minDamage;
        protected uint maxDamage;
        protected float attackSpeed;
        protected bool isAlive;
        protected float totalSpeed;
        protected uint maxhealth;
        protected int experiencefor;
        protected Dictionary<int,Item> dropList;
        protected float dropcoefficient;
        #endregion
        public Monster(string Name,uint Level,uint Health,string MinMaxDamage,float AttSpeed,float DropCoefficient)
        {
            this.name = Name;
            this.level = Level;
            this.health = Health;
            this.maxhealth = Health;
            this.attackSpeed = AttSpeed;
            string[] tmp = MinMaxDamage.Split('-');
            this.minDamage = uint.Parse(tmp[0]);
            this.maxDamage = uint.Parse(tmp[1]);
            this.isAlive = true;
            this.totalSpeed = 500 * AttSpeed;
            this.dropList = new Dictionary<int, Item>();
            this.dropcoefficient = DropCoefficient;
        }

        /// <summary>
        /// Ініціалізує об`єкт Monster з вказаним дропом
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Level"></param>
        /// <param name="Health"></param>
        /// <param name="MinMaxDamage"></param>
        /// <param name="AttSpeed"></param>
        /// <param name="DropCoefficient"></param>
        /// <param name="DropList"></param>
        public Monster(string Name, uint Level, uint Health, string MinMaxDamage, float AttSpeed, float DropCoefficient,Dictionary<int,Item> DropList) 
            : this(Name, Level, Health, MinMaxDamage, AttSpeed, DropCoefficient)
        {
            this.dropList = new Dictionary<int, Item>(DropList);
        }

        public override string ToString()
        {
            return string.Format($"{this.Name} \nрiвень:{this.Level} \nздоров`я:{this.Health} ");
        }

        #region Властивості
        public float AttackSpeed
        {
            get { return this.attackSpeed; }
        }
        public uint Level
        {
            get { return this.level; }
        }
        public uint Health
        {
            get { return this.health; }
        }
        public uint MaxDamage
        {
            get { return this.maxDamage; }
        }

        public uint MinDamage
        {
            get { return this.minDamage; }
        }

        public string Name
        {
            get { return this.name; }
        }
        public bool IsAlive { get { return this.isAlive; } }
        public float TotalSpeed { get { return this.totalSpeed; } }
        public uint MaxHealth { get { return this.maxhealth; } }
        public int ExperienceFor { get { return this.experiencefor; } }//Цей опит дається за кілл цього моба
        public Dictionary<int,Item> DropList { get { return this.dropList; } }//Локальний дроп з моба
        public float DropCoefficient { get { return this.dropcoefficient; } }//Цей коофіцієнт множиться на шанс випадіння глобального дропу
        #endregion

        public virtual string AutoAttack(Player player)
        {
            return player.TakePhisicalDamageFromMonster(new Random().Next((int)this.MinDamage, (int)this.MaxDamage + 1), this);
        }
        public virtual string TakePhisicalDamage(float Damage,IPlayer player)
        {
            if (this.health - Damage <= 0)
            {
                this.isAlive = false;
                return $"{player.Name} убив iстоту {this.Name}, нанесши {Damage} шкоди";
            }
            this.health -= (uint)Damage;
            if (this.health <= 0)
                this.isAlive = false;
            if (!this.isAlive)
            {
                return $"{player.Name} убив iстоту {this.Name}, нанесши {Damage} шкоди";
            }
            return $"{player.Name} нанiс {Damage} шкоди iстотi {this.Name}";
        }
    }
    [Serializable]
    class Wolf : Monster
    {

        /// <summary>
        /// Покищо не знаю що добавити
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Level"></param>
        /// <param name="Health"></param>
        /// <param name="MinMaxDamage"></param>
        /// <param name="AttSpeed"></param>
        /// <param name="DropCoefficient">Множник для глобального дропу; змінює шанс дропу глобал дропу</param>
        public Wolf(string Name, uint Level, uint Health, string MinMaxDamage, float AttSpeed,float DropCoefficient,Dictionary<int,Item> DropList) 
            : base(Name, Level, Health, MinMaxDamage, AttSpeed,DropCoefficient,DropList)
        {
            // Покищо не знаю що добавити...
        }
        /// <summary>
        /// Поки зробив так, потім можна переробити
        /// </summary>
        /// <param name="player">Гравець</param>
        public override string AutoAttack(Player player)
        {          
            return player.TakePhisicalDamageFromMonster(new Random().Next((int)this.MinDamage, (int)this.MaxDamage+1),this);
        }


        /// <summary>
        /// Треба добавити більший функціонал при смерті істоти, наприклад випадання луту...
        /// </summary>
        /// <param name="Damage"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public override string TakePhisicalDamage(float Damage, IPlayer player)
        {
            if (this.health - Damage <= 0)
            {
                this.isAlive = false;
                return $"{player.Name} убив iстоту {this.Name}, нанесши {Damage} шкоди";
            }
            this.health -= (uint)Damage;
            if (this.health <= 0)
                this.isAlive = false;
            if (!this.isAlive)
            {
                return $"{player.Name} убив iстоту {this.Name}, нанесши {Damage} шкоди";
            }
            return $"{player.Name} нанiс {Damage} шкоди iстотi {this.Name}";
        }
    }
}
