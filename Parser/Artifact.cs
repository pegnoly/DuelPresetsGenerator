using DuelPresetsGenerator.Entities;
using DuelPresetsGenerator.Parsers.Base;
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

    /// <summary>
    /// Парсит файл с артефактами в бд
    /// </summary>
    public class ArtifactFilesParser: IParser {
        private readonly string _artifactsXdbKey = "GameMechanics/RefTables/Artifacts.xdb";

        public string Parse(FilesDatabase database) {

            XmlSerializer artTableSerializer = new XmlSerializer(typeof(ArtifactsTable));
            XDocument artTableDocument = XDocument.Parse(database.GetFile(_artifactsXdbKey)!);
            ArtifactsTable artifactsEntities = (ArtifactsTable)artTableSerializer.Deserialize(artTableDocument.CreateReader())!;

            string commandString = string.Empty;
            foreach (ArtifactObject entity in artifactsEntities.objects!) {
                commandString += "INSERT OR REPLACE INTO Artifacts (Id, Type, Slot, Cost) " +
                                    $"VALUES (\"{entity.ID}\", \"{entity.obj!.Type}\", \"{entity.obj!.Slot}\", {entity.obj!.CostOfGold}); ";

            }

            return commandString;
        }
    }
}
