using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_first_RPG
{
    public interface IThing
    {
        string Name { get; }
        uint PureWorth { get; }
        uint Weight { get; }
        List<ItemActions> AvailableActions { get; }
    }
    public enum ItemActions
    {
        Викинути,
        Одіти,
        Випити,
        Зняти
        
    }
}
