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
    }

   /// <summary>
   /// Треба як мінімум добавити систему лута і експу за моба
   /// </summary>
   [Serializable]
   public abstract class Monster : IMonster
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
        #endregion
        public Monster(string Name,uint Level,uint Health,string MinMaxDamage,float AttSpeed)
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
        #endregion

        public abstract string AutoAttack(Player player);
        public abstract string TakePhisicalDamage(float Damage,IPlayer player);    
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
        public Wolf(string Name, uint Level, uint Health, string MinMaxDamage, float AttSpeed) : base(Name, Level, Health, MinMaxDamage, AttSpeed)
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
