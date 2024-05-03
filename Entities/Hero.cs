using DuelPresetsGenerator.Common.Objects;
using DuelPresetsGenerator.Common;
using System.Xml.Serialization;

namespace DuelPresetsGenerator.Entities {

    /// <summary>
    /// Типы классов героев
    /// </summary>
    [Serializable]
    public enum HeroClassID {
        HERO_CLASS_NONE,
        HERO_CLASS_KNIGHT,
        HERO_CLASS_RANGER,
        HERO_CLASS_WIZARD,
        HERO_CLASS_DEMON_LORD,
        HERO_CLASS_NECROMANCER,
        HERO_CLASS_WARLOCK,
        HERO_CLASS_RUNEMAGE,
        HERO_CLASS_BARBARIAN
    }

    /// <summary>
    /// Форма информации для заполнения тега Editable у героя в игре
    /// </summary>
    [Serializable]
    public class Editable {
        public FileRef? NameFileRef { get; set; }
        public FileRef? BiographyFileRef { get; set; }
        public int Offence { get; set; }
        public int Defence { get; set; }
        public int Spellpower { get; set; }
        public int Knowledge { get; set; }
        [XmlArrayItem("Item")]
        public List<HeroSkill> skills { get; set; } = new List<HeroSkill>();
        [XmlArrayItem("Item")]
        public List<HeroSkillID> perksIDs { get; set; } = new List<HeroSkillID>();
        [XmlArrayItem("Item")]
        public List<SpellID> spellIDs { get; set; } = new List<SpellID>();
        public bool Ballista { get; set; }
        public bool FirstAidTent { get; set; }
        public bool AmmoCart { get; set; }
        [XmlArrayItem("Item")]
        public List<CreatureID> FavoriteEnemies { get; set; } = new List<CreatureID>();
        public int TalismanLevel { get; set; } = 0;
    }

    /// <summary>
    /// Форма информации о герое для файлов игры
    /// </summary>
    [Serializable]
    public class AdvMapHeroShared {
        public FileRef? FaceTexture { get; set; }
        public HeroClassID Class { get; set; }
        public TownType TownType { get; set; }
        public Editable? Editable { get; set; }
    }

    /// <summary>
    /// Форма информации о герое для бд
    /// </summary>
    [Serializable]
    public class HeroDataModel {
        public string? Xdb { get; set; }
        public string? Icon { get; set; }
        public HeroClassID Class { get; set; }
        public TownType Town { get; set; }
        public string? Name { get; set; }
    }
}
