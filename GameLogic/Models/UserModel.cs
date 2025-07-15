using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicLibrary.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public List<GridSpot> GridLocation { get; set; } = new List<GridSpot>();

       // public List<GridSpot> ShotRecorded { get; set; } = new List<GridSpot>();
    }
}
