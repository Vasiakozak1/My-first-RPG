using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_first_RPG
{
    interface IThing
    {
        string Name { get; }
        uint PureWorth { get; }
        uint Weight { get; }
    }
}
