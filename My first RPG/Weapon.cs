using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
namespace My_first_RPG
{
    public enum WeaponType
    {
        Меч,
        Двуручний_меч,
        Молот,
        Двуручний_молот,
        Щит,
        Інше
    }
    public interface IWeapon:IThing
    {
        uint Level { get; }
        uint MaxDamage { get; }
        uint MinDamage { get; }
        uint Strenght { get; }
        float Speed { get; }
        WeaponType WeaponType { get; }
    }
    [Serializable]
    public class Weapon : Item,IWeapon
    {
        public Weapon(string Name,string MinMaxDamage,WeaponType weaponType,uint Level,uint Worth,float Speed,
             uint Weight,uint Strenght,string Iconpath,params ItemActions[] ExtraActions):base(Name,Worth,Weight,Iconpath,ExtraActions)
        {
            this.name = Name;
            this.weapontype = weaponType;
            this.level = Level;
            this.pureworth = Worth;
            this.speed = Speed;
            this.strenght = Strenght;
            this.weight = Weight;

            string[] Damages = MinMaxDamage.Split('-');
           
            this.mindamage = uint.Parse(Damages[0]);
            this.maxdamage = uint.Parse(Damages[1]);
            if (this.mindamage > this.maxdamage)
            {
                this.mindamage= uint.Parse(Damages[1]);
                this.maxdamage = uint.Parse(Damages[0]);
            }
            //Добавлення дій на предметом
            this.availableactions = new List<ItemActions>() { ItemActions.Одіти, ItemActions.Зняти, ItemActions.Викинути };
        }



        #region Поля
        protected uint level;
        protected uint maxdamage;
        protected uint mindamage;
        protected uint strenght;
        protected float speed;
        protected WeaponType weapontype;
        #endregion
        #region Властивості
        public uint Level
        {
            get { return this.level; }
        }

        public uint MaxDamage
        {
            get { return this.maxdamage; }           
        }

        public uint MinDamage
        {
            get { return this.mindamage; }
        }


        public WeaponType WeaponType//Тип зброї
        {
            get { return this.weapontype; }
        }


        public uint Strenght//Міцність зброї
        {
            get { return this.strenght; }
        }


        public float Speed// Швидкість зброї; чим більше значення, тим повільніша зброя
        {
            get { return this.speed; }
        }

        #endregion


    }
}
