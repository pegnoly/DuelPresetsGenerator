using DuelPresetsGenerator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuelPresetsGenerator.Entities.Creature {

    [Serializable]
    public class Creature {
        public int CreatureTier { get; set; }
        public TownType CreatureTown { get; set; }
        public int WeeklyGrowth { get; set; }
        public CreatureType BaseCreature { get; set; }
    }
}
