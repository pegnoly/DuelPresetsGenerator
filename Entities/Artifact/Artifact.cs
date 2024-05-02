using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DuelPresetsGenerator.Entities.Artifact {

    [Serializable]
    public class Artifact {
        public ArtifactType Type { get; set; }
        public ArtifactSlot Slot { get; set; }
        public int CostOfGold { get; set; }
    }
}
