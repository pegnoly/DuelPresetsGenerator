using DuelPresetsGenerator.Common;
using DuelPresetsGenerator.Common.Objects;
using DuelPresetsGenerator.Entities;
using System.Xml.Serialization;

namespace DuelPresetsGenerator.Generator.Objects {

    [Serializable]
    public class Action {
        public string? FunctionName { get; set; }
    }

    [Serializable]
    public class Trigger {
        public Action? Action { get; set; }
    }

    [Serializable]
    public class HeroTexturesSet {
        public string? Icon128x128 { get; set; }
        public string? Icon64x64 { get; set; }
        public string? RoundedFace { get; set; }
        public string? LeftFace { get; set; }
        public string? RightFace { get; set; }
    }

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
        [XmlArrayItem("Item")]
        public List<ArtifactID>? artifactIDs { get; set; } = new List<ArtifactID>();
        [XmlArrayItem("Item")]
        public List<int> isUntransferable { get; set; } = new List<int>();
        public Editable Editable { get; set; } = new Editable();
        public int OverrideMask { get; set; }
        public Mastery PrimarySkillMastery { get; set; }
        public Trigger? LossTrigger { get; set; } = new Trigger();
        public bool AllowQuickCombat { get; set; }
        public HeroTexturesSet? Textures { get; set; } = new HeroTexturesSet();
        public int PresetPrice { get; set; }
        [XmlArrayItem("Item")]
        public List<TownType> BannedRaces { get; set; } = new List<TownType>();
    }

    [Serializable]
    public class DuelPreset {
        public FileRef? PresetNameFileRef { get; set; }
        public FileRef? LeftFace { get; set; }
        public FileRef? RightFace { get; set; }
        public FileRef? RoundedFace { get; set; }
        public FileRef? PresetHero { get; set; }
    }

    [Serializable]
    [XmlRoot("DuelPresets")]
    public class DuelPresetsTable {
        [XmlArrayItem("Item")]
        public List<DuelPreset>? presets { get; set; } = new List<DuelPreset>();
    }
}