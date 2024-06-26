﻿namespace DuelPresetsGenerator.Entities {

    /// <summary>
    /// Типы слотов артефактов
    /// </summary>
    [Serializable]
    public enum ArtifactSlot {
        INVENTORY,
        PRIMARY,
        SECONDARY,
        HEAD,
        CHEST,
        NECK,
        FINGER,
        FEET,
        SHOULDERS,
        MISCSLOT1
    }

    /// <summary>
    /// Типы силы артефактов
    /// </summary>
    [Serializable]
    public enum ArtifactType {
        ARTF_CLASS_MINOR,
        ARTF_CLASS_MAJOR,
        ARTF_CLASS_RELIC,
        ARTF_CLASS_GRAIL
    }

    /// <summary>
    /// Айдишники артефактов
    /// </summary>
    [Serializable]
    public enum ArtifactID {
        ARTIFACT_NONE,
        SWORD_OF_RUINS,
        GREAT_AXE_OF_GIANT_SLAYING,
        WAND_OF_X,
        UNICORN_HORN_BOW,
        TITANS_TRIDENT,
        STAFF_OF_VEXINGS,
        SHACKLES_OF_WAR,
        FOUR_LEAF_CLOVER,
        ICEBERG_SHIELD,
        GOLDEN_SEXTANT,
        CROWN_OF_COURAGE,
        CROWN_OF_MANY_EYES,
        PLATE_MAIL_OF_STABILITY,
        BREASTPLATE_OF_PETRIFIED_WOOD,
        PEDANT_OF_MASTERY,
        NECKLACE_OF_BRAVERY,
        WEREWOLF_CLAW_NECKLACE,
        EVERCOLD_ICICLE,
        NECKLACE_OF_POWER,
        RING_OF_LIGHTING_PROTECTION,
        RING_OF_LIFE,
        RING_OF_HASTE,
        NIGHTMARISH_RING,
        BOOTS_OF_SPEED,
        GOLDEN_HORSESHOE,
        WAYFARER_BOOTS,
        BOOTS_OF_INTERFERENCE,
        ENDLESS_SACK_OF_GOLD,
        ENDLESS_BAG_OF_GOLD,
        ANGEL_WINGS,
        LION_HIDE_CAPE,
        PHOENIX_FEATHER_CAPE,
        CLOAK_OF_MOURNING,
        HELM_OF_ENLIGHTMENT,
        CHAIN_MAIL_OF_ENLIGHTMENT,
        DRAGON_SCALE_ARMOR,
        DRAGON_SCALE_SHIELD,
        DRAGON_BONE_GRAVES,
        DRAGON_WING_MANTLE,
        DRAGON_TEETH_NECKLACE,
        DRAGON_TALON_CROWN,
        DRAGON_EYE_RING,
        DRAGON_FLAME_TONGUE,
        ROBE_OF_MAGI,
        STAFF_OF_MAGI,
        CROWN_OF_MAGI,
        RING_OF_MAGI,
        DWARVEN_MITHRAL_CUIRASS,
        DWARVEN_MITHRAL_GREAVES,
        DWARVEN_MITHRAL_HELMET,
        DWARVEN_MITHRAL_SHIELD,
        SCROLL_OF_SPELL_X,
        GRAAL,
        BOOTS_OF_LEVITATION,
        ARTIFACT_SKULL_HELMET,
        ARTIFACT_VALORIOUS_ARMOR,
        ARTIFACT_BOOTS_OF_SWIFTNESS,
        ARTIFACT_MOONBLADE,
        ARTIFACT_RING_OF_CELERITY,
        ARTIFACT_BAND_OF_CONJURER,
        ARTIFACT_EARTHSLIDERS,
        ARTIFACT_RIGID_MANTLE,
        ARTIFACT_JINXING_BAND,
        ARTIFACT_BONESTUDDED_LEATHER,
        ARTIFACT_WISPERING_RING,
        ARTIFACT_HELM_OF_CHAOS,
        ARTIFACT_TWISTING_NEITHER,
        ARTIFACT_SANDALS_OF_THE_SAINT,
        ARTIFACT_SHAWL_OF_GREAT_LICH,
        ARTIFACT_RING_OF_DEATH,
        ARTIFACT_NECROMANCER_PENDANT,
        ARTIFACT_FREIDA,
        ARTIFACT_RING_OF_THE_SHADOWBRAND,
        ARTIFACT_OGRE_CLUB,
        ARTIFACT_OGRE_SHIELD,
        ARTIFACT_TOME_OF_DESTRUCTION,
        ARTIFACT_TOME_OF_LIGHT_MAGIC,
        ARTIFACT_TOME_OF_DARK_MAGIC,
        ARTIFACT_TOME_OF_SUMMONING_MAGIC,
        ARTIFACT_BEGINNER_MAGIC_STICK,
        ARTIFACT_RUNIC_WAR_AXE,
        ARTIFACT_RUNIC_WAR_HARNESS,
        ARTIFACT_SKULL_OF_MARKAL,
        ARTIFACT_BEARHIDE_WRAPS,
        ARTIFACT_DWARVEN_SMITHY_HUMMER,
        ARTIFACT_RUNE_OF_FLAME,
        ARTIFACT_TAROT_DECK,
        ARTIFACT_CROWN_OF_LEADER,
        ARTIFACT_MASK_OF_DOPPELGANGER,
        ARTIFACT_EDGE_OF_BALANCE,
        ARTIFACT_RING_OF_MACHINE_AFFINITY,
        ARTIFACT_HORN_OF_PLENTY,
        ARTIFACT_RING_OF_UNSUMMONING,
        ARTIFACT_BOOK_OF_POWER,
        ARTIFACT_TREEBORN_QUIVER,
        ARTIFACT_PRINCESS,
        ARTIFACT_UNIVERSE_RING_OF_SILENCE
    }

    /// <summary>
    /// Форма информации об артефакте для файлов игры.
    /// </summary>
    [Serializable]
    public class Artifact {
        public ArtifactType Type { get; set; }
        public ArtifactSlot Slot { get; set; }
        public int CostOfGold { get; set; }
    }

    /// <summary>
    /// Форма информации об артефакте для бд.
    /// </summary>
    [Serializable]
    public class ArtifactDataModel {
        public ArtifactID Id { get; set; } 
        public ArtifactType Type { get; set; }
        public ArtifactSlot Slot { get; set; }
        public int Cost { get; set; }
    }
}
