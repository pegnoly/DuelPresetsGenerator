using Dapper;
using DuelPresetsGenerator.Entities;
using DuelPresetsGenerator.Generator.Objects;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuelPresetsGenerator.Generator.Strategies {

    /// <summary>
    /// Тестовая стратегия генерации спеллов. Выдает все спеллы ранее сгенерированных школ(т.е. спеллы 2-процентных школ)
    /// </summary>
    public class TestSpellGenerationStrategy : ISpellGenerationStrategy {

        public List<SpellID> Generate(HeroDataModel heroModel, SqliteConnection connection, AdvMapHero preset) {
            List<SpellID> spells = new List<SpellID>();
            IEnumerable<MagicSchool> availableSchools = from KeyValuePair<HeroSkillID, MagicSchool> kvp in ISpellGenerationStrategy._skillToSchoolMapping 
                                                        where preset.Editable.skills.Any(skill => kvp.Key == skill.SkillID)
                                                        select kvp.Value;
            if(availableSchools != null && availableSchools.Count() > 0) {
                foreach(MagicSchool school in availableSchools) {
                    IEnumerable<SpellDataModel> spellModels = connection.Query<SpellDataModel>($"SELECT * FROM Spells WHERE School = \"{school}\";");
                    if(spellModels != null && spellModels.Count() > 0) {
                        foreach(SpellDataModel spellModel in spellModels) {
                            spells.Add(spellModel.Id);
                        }
                    }
                }
            }
            return spells;
        }
    }
}
