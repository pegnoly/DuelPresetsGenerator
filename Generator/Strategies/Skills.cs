using Dapper;
using DuelPresetsGenerator.Entities;
using Microsoft.Data.Sqlite;


namespace DuelPresetsGenerator.Generator.Strategies {
    /// <summary>
    /// Тестовая стратегия генерации скиллов. Выдает двухпроцентные навыки фракции героя на экспертном уровне.
    /// </summary>
    public class TestSkillGenerationStrategy : ISkillsGenerationStrategy {
        public List<HeroSkill> Generate(HeroDataModel hero, SqliteConnection connection) {
            List<HeroSkill> heroSkills = new List<HeroSkill>();
            IEnumerable<HeroSkillDataModel> skillModels = connection.Query<HeroSkillDataModel>($"SELECT * FROM HeroClassSkillProbs " +
                                                                                                $"WHERE Class = \"{hero.Class}\" AND Prob = 2");
            if (skillModels != null) {
                foreach (HeroSkillDataModel skillModel in skillModels) {
                    heroSkills.Add(new HeroSkill() {
                        Mastery = Mastery.MASTERY_EXPERT,
                        SkillID = skillModel.Skill
                    });
                }
            }
            return heroSkills;
        }
    }
}
