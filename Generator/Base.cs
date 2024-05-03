using Dapper;
using DuelPresetsGenerator.Common;
using DuelPresetsGenerator.Common.Objects;
using DuelPresetsGenerator.Entities;
using DuelPresetsGenerator.Generator.Objects;
using DuelPresetsGenerator.Generator.Strategies;
using DuelPresetsGenerator.Parser;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DuelPresetsGenerator.Generator {

    public interface IArmyGenerationStrategy {
        List<ArmySlot> Generate(HeroDataModel hero, SqliteConnection connection);
    }

    public interface IArtifactGenerationStrategy {
        /// <summary>
        /// Запрещенные для генерации артефакты.
        /// </summary>
        static readonly IReadOnlyList<ArtifactID> _bannedArtifacts = new List<ArtifactID>() {
            ArtifactID.ARTIFACT_NONE, ArtifactID.ARTIFACT_PRINCESS, ArtifactID.ARTIFACT_FREIDA, ArtifactID.ANGEL_WINGS, ArtifactID.ARTIFACT_RING_OF_THE_SHADOWBRAND
        };

        List<ArtifactID> Generate(HeroDataModel hero, SqliteConnection connection);
    }

    public interface ISkillsGenerationStrategy {
        List<HeroSkill> Generate(HeroDataModel hero, SqliteConnection connection);
    }

    public interface ISpellGenerationStrategy {

        static readonly IReadOnlyDictionary<HeroSkillID, MagicSchool> _skillToSchoolMapping = new Dictionary<HeroSkillID, MagicSchool>() {
            {HeroSkillID.HERO_SKILL_DARK_MAGIC, MagicSchool.MAGIC_SCHOOL_DARK},
            {HeroSkillID.HERO_SKILL_LIGHT_MAGIC, MagicSchool.MAGIC_SCHOOL_LIGHT},
            {HeroSkillID.HERO_SKILL_SUMMONING_MAGIC, MagicSchool.MAGIC_SCHOOL_SUMMONING},
            {HeroSkillID.HERO_SKILL_DESTRUCTIVE_MAGIC, MagicSchool.MAGIC_SCHOOL_DESTRUCTIVE},
        };

        List<SpellID> Generate(HeroDataModel heroModel, SqliteConnection connection, AdvMapHero currentHero);
    }

    public class Generator {

        private readonly int _defaultExperienceAmount = 15000;

        private IArmyGenerationStrategy armyGenerationStrategy = new TestArmyGenerationStrategy();
        private IArtifactGenerationStrategy artifactGenerationStrategy = new TestArtifactGenerationStrategy();
        private ISkillsGenerationStrategy skillsGenerationStrategy = new TestSkillGenerationStrategy();
        private ISpellGenerationStrategy spellGenerationStrategy = new TestSpellGenerationStrategy();

        private int _presetsCount = 1;

        private DuelPresetsTable _presetsTable = new DuelPresetsTable();

        public void Run() {
            using(SqliteConnection connection = new SqliteConnection($"Data Source = {Paths.Database}")) {
                connection.Open();

                IEnumerable<HeroDataModel> heroModels = connection.Query<HeroDataModel>("SELECT * FROM Heroes;");
                using (FileStream fs = new FileStream($"{Paths.HommData}test_presets.pak", FileMode.Create)) {
                    using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create)) {
                        foreach (HeroDataModel heroModel in heroModels) {
                            AdvMapHero preset = new AdvMapHero();
                            preset.Shared!.href = heroModel.Xdb;
                            preset.Experience = _defaultExperienceAmount;
                            preset.armySlots = armyGenerationStrategy.Generate(heroModel, connection);
                            preset.artifactIDs = artifactGenerationStrategy.Generate(heroModel, connection);
                            preset.Editable.skills = skillsGenerationStrategy.Generate(heroModel, connection);
                            preset.Editable.spellIDs = spellGenerationStrategy.Generate(heroModel, connection, preset);

                            string presetXdbPath = $"Maps/DuelMode/Heroes/preset_{_presetsCount}.xdb";
                            _presetsTable.presets!.Add(new DuelPreset() {
                                PresetNameFileRef = new FileRef() { href = heroModel.Name },
                                LeftFace = new FileRef() { href = heroModel.Icon},
                                RightFace = new FileRef() { href = heroModel.Icon },
                                RoundedFace = new FileRef() { href = heroModel.Icon },
                                PresetHero = new FileRef() { href = $"/{presetXdbPath}#xpointer(/AdvMapHero)" },
                            });
                            _presetsCount++;

                            ZipArchiveEntry presetEntry = archive.CreateEntry(presetXdbPath);
                            using (StreamWriter sw = new StreamWriter(presetEntry.Open())) {
                                var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty, });
                                XmlSerializer xs = new XmlSerializer(typeof(AdvMapHero));
                                xs.Serialize(sw, preset, ns);
                            }
                        }

                        ZipArchiveEntry presetsTableEntry = archive.CreateEntry("UI/MPDMLobby/presets.(DuelPresets).xdb");
                        using(StreamWriter sw = new StreamWriter(presetsTableEntry.Open())) {
                            var ns = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty, });
                            XmlSerializer xs = new XmlSerializer(typeof(DuelPresetsTable));
                            xs.Serialize(sw, _presetsTable, ns);
                        }
                    }
                }
            }
        }
    }
}
