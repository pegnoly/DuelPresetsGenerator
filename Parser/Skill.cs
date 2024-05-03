using DuelPresetsGenerator.Entities;
using DuelPresetsGenerator.Parsers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DuelPresetsGenerator.Parser {

    [Serializable]
    public class HeroClassObject {
        public HeroClassID ID { get; set; }
        public HeroClass? obj { get; set; }
    }

    public class HeroClass {
        [XmlArrayItem("Item")]
        public List<HeroSkillProb>? SkillsProbs { get; set; }
    }

    [Serializable]
    [XmlRoot("Table_HeroClassDesc_HeroClass")]
    public class HeroClassTable {
        [XmlArrayItem("Item")]
        public List<HeroClassObject>? objects { get; set; }
    }

    public class HeroSkillProb {
        public HeroSkillID SkillID { get; set; }
        public int Prob { get; set; }
    }

    /// <summary>
    /// Парсит инфу о шансах выпадения баз для героев в бд
    /// </summary>
    public class HeroSkillParser : IParser {

        private readonly string _heroClassXdbKey = "GameMechanics/RefTables/HeroClass.xdb";

        public string Parse(FilesDatabase database) {
            XmlSerializer heroClassTableSerializer = new XmlSerializer(typeof(HeroClassTable));
            XDocument heroClassTableDocument = XDocument.Parse(database.GetFile(_heroClassXdbKey)!);
            HeroClassTable heroClassEntities = (HeroClassTable)heroClassTableSerializer.Deserialize(heroClassTableDocument.CreateReader())!;

            string commandString = string.Empty;
            foreach (HeroClassObject entity in heroClassEntities.objects!) {
                foreach(HeroSkillProb skillProb in entity.obj!.SkillsProbs!) {
                    commandString += "INSERT OR REPLACE INTO HeroClassSkillProbs (Class, Skill, Prob) " +
                    $"VALUES (\"{entity.ID}\", \"{skillProb.SkillID}\", {skillProb.Prob}); ";
                }
            }
            return commandString;
        }
    }
}
