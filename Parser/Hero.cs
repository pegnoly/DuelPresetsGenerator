using DuelPresetsGenerator.Common.Objects;
using DuelPresetsGenerator.Entities;
using DuelPresetsGenerator.Parsers.Base;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DuelPresetsGenerator.Parsers {

    public class AdvMapSharedGroup {
        [XmlArrayItem("Item")]
        public List<FileRef> links { get; set; } = new List<FileRef>();
    }

    /// <summary>
    /// Парсит файлы героев в бд
    /// </summary>
    public class HeroesFilesParser: IParser {

        private readonly IReadOnlyList<string> _possibleHeroesGroups = new List<string>() {
            "Academy", "Dungeon_Standart", "Fortress", "Haven_Standart", "Inferno_Standart",
            "Necropolis_Standart", "Preserve_Standart", "Stronghold"
        };

        public string Parse(FilesDatabase database) {
            string commandString = string.Empty;
            XmlSerializer heroSerializer = new XmlSerializer(typeof(AdvMapHeroShared));
            foreach (string group in _possibleHeroesGroups) {
                XDocument heroesDocument = XDocument.Parse(database.GetFile($"MapObjects/_(AdvMapSharedGroup)/Heroes/{group}.xdb")!);
                XmlSerializer heroesGroupSerializer = new XmlSerializer(typeof(AdvMapSharedGroup));
                AdvMapSharedGroup heroesGroup = (AdvMapSharedGroup)heroesGroupSerializer.Deserialize(heroesDocument.CreateReader())!;
                foreach (FileRef link in heroesGroup.links) {
                    string href = link.href!;
                    string key = href.Replace("#xpointer(/AdvMapHeroShared)", string.Empty).Remove(0, 1);
                    string heroXdbString = database.GetFile(key)!;
                    XDocument heroDoc = XDocument.Parse(heroXdbString);
                    AdvMapHeroShared hero = (AdvMapHeroShared)heroSerializer.Deserialize(heroDoc.CreateReader())!;
                    commandString += "INSERT OR REPLACE INTO Heroes (Xdb, Class, Town, Name, Icon) " +
                                        $"VALUES (\"{href}\", \"{hero.Class}\", \"{hero.TownType}\", \"{hero.Editable!.NameFileRef!.href}\", \"{hero.FaceTexture!.href}\"); ";
                }
            }
            return commandString;
        }
    }
}
