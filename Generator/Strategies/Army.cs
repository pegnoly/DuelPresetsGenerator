using Dapper;
using DuelPresetsGenerator.Common.Objects;
using DuelPresetsGenerator.Entities;
using Microsoft.Data.Sqlite;

namespace DuelPresetsGenerator.Generator.Strategies {
    /// <summary>
    /// Тестовая стратегия генерации армии. Генерирует 5 приростов случайных грейженых существ каждого уровня фракции, соответствующей герою.
    /// </summary>
    public class TestArmyGenerationStrategy : IArmyGenerationStrategy {

        private readonly int _growWeeksMultiplier = 5;

        public List<ArmySlot> Generate(HeroDataModel hero, SqliteConnection connection) {
            List<ArmySlot> armySlots = new List<ArmySlot>();
            IEnumerable<CreatureDataModel> creatures = connection.Query<CreatureDataModel>($"SELECT * FROM Creatures WHERE Town = \"{hero.Town}\" " +
                                                                                       $"AND BaseCreature IS NOT \"CREATURE_UNKNOWN\";");
            Random rnd = new Random();
            for (int creatureLevel = 1; creatureLevel <= 7; creatureLevel++) {
                IEnumerable<CreatureDataModel> selectedCreatures = from CreatureDataModel creature in creatures
                                                                 where creature.Tier == creatureLevel
                                                                 select creature;
                int randomIndex = rnd.Next(0, selectedCreatures.Count());
                CreatureDataModel selectedCreature = selectedCreatures.ElementAt(randomIndex);
                armySlots.Add(new ArmySlot() {
                    Creature = selectedCreature.Id,
                    Count = selectedCreature.Grow * _growWeeksMultiplier
                });
            }
            return armySlots;
        }
    }
}
