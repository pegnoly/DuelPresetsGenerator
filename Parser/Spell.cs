using DuelPresetsGenerator.Common.Objects;
using DuelPresetsGenerator.Entities;
using DuelPresetsGenerator.Parsers.Base;
using DuelPresetsGenerator.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DuelPresetsGenerator.Parser {
    [Serializable]
    public class SpellObject {
        public SpellID ID { get; set; }
        public FileRef? Obj { get; set; }
    }

    [Serializable]
    [XmlRoot("Table_Spell_SpellID")]
    public class SpellsTable {
        [XmlArrayItem("Item")]
        public List<SpellObject>? objects { get; set; }
    }

    /// <summary>
    /// Парсит инфу о спеллах в бд
    /// </summary>
    public class SpellsFilesParser : IParser {

        private readonly string _spellsXdbKey = "GameMechanics/RefTables/UndividedSpells.xdb";

        public string Parse(FilesDatabase database) {
            XmlSerializer spellsTableSerializer = new XmlSerializer(typeof(SpellsTable));
            XDocument spellsTableDocument = XDocument.Parse(database.GetFile(_spellsXdbKey)!);
            SpellsTable spellsEntities = (SpellsTable)spellsTableSerializer.Deserialize(spellsTableDocument.CreateReader())!;
            XmlSerializer spellSerializer = new XmlSerializer(typeof(Spell));

            string commandString = string.Empty;
            foreach (SpellObject entity in spellsEntities.objects!) {
                if(entity.Obj!.href != null) {
                    string key = entity.Obj!.href!.Replace('\\', '/').Replace("#xpointer(/Spell)", string.Empty).Remove(0, 1);
                    string? spellFile = database.GetFile(key);
                    if (spellFile != null) {
                        XDocument spellDocument = XDocument.Parse(spellFile);
                        Spell spell = (Spell)spellSerializer.Deserialize(spellDocument.CreateReader())!;
                        commandString += "INSERT OR REPLACE INTO Spells (Id, Level, School) " +
                                            $"VALUES (\"{entity.ID}\", {spell.Level}, \"{spell.MagicSchool}\"); ";
                    }
                }
            }
            return commandString;
        }
    }
}
