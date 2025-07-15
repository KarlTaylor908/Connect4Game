using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameLogicLibrary.Models
{
    public class GridSpot
    {

        public int YAxis { get; set; }
        public int XAxis { get; set; }
        public EnumStatus Status { get; set; } = EnumStatus.Empty;

    }
}
