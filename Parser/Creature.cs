using DuelPresetsGenerator.Common.Objects;
using DuelPresetsGenerator.Entities;
using DuelPresetsGenerator.Parsers.Base;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DuelPresetsGenerator.Parsers {

    [Serializable]
    public class CreatureObject {
        public CreatureID ID { get;set; }
        public FileRef? Obj { get; set; }
    }

    [Serializable]
    [XmlRoot("Table_Creature_CreatureType")]
    public class CreaturesTable {
        [XmlArrayItem("Item")]
        public List<CreatureObject>? objects { get; set; }
    }

    /// <summary>
    /// Парсит файлы существ в бд
    /// </summary>
    public class CreaturesFilesParser: IParser {

        private readonly string _creaturesXdbKey = "GameMechanics/RefTables/Creatures.xdb";

        public string Parse(FilesDatabase database) {
            XmlSerializer creaturesTableSerializer = new XmlSerializer(typeof(CreaturesTable));
            XDocument creaturesTableDocument = XDocument.Parse(database.GetFile(_creaturesXdbKey)!);
            CreaturesTable creaturesEntities = (CreaturesTable)creaturesTableSerializer.Deserialize(creaturesTableDocument.CreateReader())!;
            XmlSerializer creatureSerializer = new XmlSerializer(typeof(Creature));

            string commandString = string.Empty;
            foreach (CreatureObject entity in creaturesEntities.objects!) {
                string key = entity.Obj!.href!.Replace('\\', '/').Replace("#xpointer(/Creature)", string.Empty).Remove(0, 1);
                XDocument creatureDocument = XDocument.Parse(database.GetFile(key)!);
                Creature creature = (Creature)creatureSerializer.Deserialize(creatureDocument.CreateReader())!;
                commandString += "INSERT OR REPLACE INTO Creatures (Id, Tier, Town, Grow, BaseCreature) " +
                                    $"VALUES (\"{entity.ID}\", {creature.CreatureTier}, \"{creature.CreatureTown}\", {creature.WeeklyGrowth}, \"{creature.BaseCreature}\"); ";

            }
            return commandString;
        }
    }
}
