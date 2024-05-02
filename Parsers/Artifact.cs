using DuelPresetsGenerator.Common.Objects;
using DuelPresetsGenerator.Entities.Artifact;
using DuelPresetsGenerator.Entities.Creature;
using DuelPresetsGenerator.Parsers.Base;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DuelPresetsGenerator.Parsers {

    [Serializable]
    public class ArtifactObject {
        public ArtifactID ID { get; set; }
        public Artifact? obj { get; set; }
    }

    [Serializable]
    [XmlRoot("Table_DBArtifact_ArtifactEffect")]
    public class ArtifactsTable {
        [XmlArrayItem("Item")]
        public List<ArtifactObject>? objects { get; set; }
    }

    internal class ArtifactFilesParser: IParser {
        private readonly string _artifactsXdbKey = "GameMechanics/RefTables/Artifacts.xdb";

        public void Parse(FilesDatabase database) {
            string artifactsXdbString = database.GetFile(_artifactsXdbKey);
            Console.WriteLine(artifactsXdbString);
            XmlSerializer xs = new XmlSerializer(typeof(ArtifactsTable));
            XDocument doc = XDocument.Parse(artifactsXdbString);
            ArtifactsTable artifactsEntities = (ArtifactsTable)xs.Deserialize(doc.CreateReader())!;

            XmlSerializer cs = new XmlSerializer(typeof(Artifact));
            string commandString = "BEGIN TRANSACTION; ";
            using (SqliteConnection connection = new SqliteConnection($"Data Source = {Paths.Database}")) {
                connection.Open();
                foreach (ArtifactObject entity in artifactsEntities.objects!) {
                    commandString += "INSERT OR REPLACE INTO Artifacts (Id, Type, Slot, Cost) " +
                                     $"VALUES (\"{entity.ID}\", \"{entity.obj!.Type}\", \"{entity.obj!.Slot}\", {entity.obj!.CostOfGold}); ";

                }
                commandString += "COMMIT";
                SqliteCommand command = new SqliteCommand(commandString, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
