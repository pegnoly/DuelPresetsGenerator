using DuelPresetsGenerator.Common.Objects;
using DuelPresetsGenerator.Entities.Creature;
using DuelPresetsGenerator.Parsers.Base;
using Microsoft.Data.Sqlite;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DuelPresetsGenerator.Parsers {

    public class CreatureObject {
        public CreatureType ID { get;set; }
        public FileRef? Obj { get; set; }
    }

    [Serializable]
    [XmlRoot("Table_Creature_CreatureType")]
    public class CreaturesTable {
        [XmlArrayItem("Item")]
        public List<CreatureObject>? objects { get; set; }
    }

    public class CreaturesFilesParser: IParser {

        private readonly string _creaturesXdbKey = "GameMechanics/RefTables/Creatures.xdb";

        public void Parse(FilesDatabase database) {
            string creaturesXdb = database.GetFile(_creaturesXdbKey);
            XmlSerializer xs = new XmlSerializer(typeof(CreaturesTable));
            XDocument doc = XDocument.Parse(creaturesXdb);
            CreaturesTable creaturesEntities = (CreaturesTable)xs.Deserialize(doc.CreateReader())!;

            XmlSerializer cs = new XmlSerializer(typeof(Creature));
            string commandString = "BEGIN TRANSACTION; ";
            using (SqliteConnection connection = new SqliteConnection($"Data Source = {Paths.Database}")) {
                connection.Open();
                foreach (CreatureObject entity in creaturesEntities.objects!) {
                    string key = entity.Obj!.href!.Replace('\\', '/').Replace("#xpointer(/Creature)", string.Empty).Remove(0, 1);
                    string creatureXdbString = database.GetFile(key);
                    XDocument creatureDoc = XDocument.Parse(creatureXdbString);
                    Creature creature = (Creature)cs.Deserialize(creatureDoc.CreateReader())!;
                    commandString += "INSERT OR REPLACE INTO Creatures (Id, Tier, Town, Grow, BaseCreature) " +
                                     $"VALUES (\"{entity.ID}\", {creature.CreatureTier}, \"{creature.CreatureTown}\", {creature.WeeklyGrowth}, \"{creature.BaseCreature}\"); ";

                }
                commandString += "COMMIT";
                SqliteCommand command = new SqliteCommand(commandString, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
