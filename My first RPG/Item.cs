using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
namespace My_first_RPG
{
    [Serializable]
    public class Item : IThing
    {
        #region Поля
        protected List<ItemActions> availableactions;
        protected string name;
        protected uint pureworth;
        protected uint weight;
        protected string iconpath;
        #endregion

        public Item(string Name, uint PureWorth, uint Weight, string PathToImage, params ItemActions[] actions)
        {
            this.name = Name;
            this.pureworth = PureWorth;
            this.weight = Weight;
            this.availableactions = new List<ItemActions>(actions.ToList());
            this.iconpath = PathToImage;
            

        }

        public string PathIconOfItem { get { return this.iconpath; } }

        public List<ItemActions> AvailableActions
        {
            get { return this.availableactions; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public uint PureWorth
        {
            get { return this.pureworth; }
        }

        public uint Weight
        {
            get { return this.weight; }
        }
        public void AddAction(ItemActions Action)
        {
            this.availableactions.Add(Action);
        }
        public void RemoveAction(ItemActions Action)
        {
            this.availableactions.Remove(Action);
        }
    }
    public class HealingPotion:Item
    {
        uint minhealeffect;
        uint maxhealeffect;
        bool isused;

        public HealingPotion(uint MinHealing,uint MaxHealing,string Name,uint PureWorth,uint Weight, string PathToImage, params ItemActions[] actions):base(Name,PureWorth,Weight,PathToImage,actions)
        {
           this.isused = false;
            this.minhealeffect = MinHealing;
            this.maxhealeffect = MaxHealing;
        }

        public bool IsUsed { get { return this.isused; } }
        public uint MinHealEffect { get { return this.minhealeffect; } }
        public uint MaxHealEffect { get { return this.maxhealeffect; } }

        public void Use(Player Target,List<Item> Inventory)
        {
            uint Healing = (uint)new Random().Next((int)this.MinHealEffect, (int)this.MaxHealEffect);
            Target.RestoreHealth(Healing);
            this.isused = true;
            Inventory.Remove(this);
        }
    }
    [Serializable]
    public class Armor : Item
    {
        public ArmorType Type { get; private set; }
        public uint ProtectionPoints { get; private set; }

        public Armor(string Name,uint PureWorth,uint Weight,string PathToImage,ArmorType ArmType,uint ProtectionPoints,params ItemActions[] Actions)
            : base(Name,PureWorth,Weight,PathToImage,Actions)
        {
            this.Type = ArmType;
            this.ProtectionPoints = ProtectionPoints;
        }
    }
    [Serializable]
    public enum ArmorType
    {
        Шолом,
        Нагрудник,
        Наплечник,
        Брюки,
        Черевики,
        Рукавиці,
        Перстень,
        Намисто
    }
}
