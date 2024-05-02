using DuelPresetsGenerator.Common.Objects;
using DuelPresetsGenerator.Entities.Artifact;
using DuelPresetsGenerator.Entities.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DuelPresetsGenerator.Generator.Objects {

    [Serializable]
    public class AdvMapHero {
        [XmlElement("Pos")]
        public Vec3<int>? Position { get; set; } = new Vec3<int> { x = 0, y = 0, z = 0 };
        [XmlElement("Rot")]
        public float Rotation { get; set; }
        public short Floor { get; set; }
        public string? Name { get; set; }
        public FileRef? CombatScript { get; set; } = new FileRef();
        [XmlArrayItem("Item")]
        public List<PointLight>? pointLights { get; set; } = new List<PointLight>();
        public FileRef? Shared { get; set; } = new FileRef();
        public PlayerID PlayerID { get; set; }
        public int Experience { get; set; }
        [XmlArrayItem("Item")]
        public List<ArmySlot>? armySlots { get; set; } = new List<ArmySlot>();
        [XmlElement("artifactIDs")]
        public List<ArtifactID>? Artifacts { get; set; } = new List<ArtifactID>();
    }
}
