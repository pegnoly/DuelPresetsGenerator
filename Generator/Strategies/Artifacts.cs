using Dapper;
using DuelPresetsGenerator.Entities;
using Microsoft.Data.Sqlite;

namespace DuelPresetsGenerator.Generator.Strategies {
    /// <summary>
    /// Тестовая стратегия генерации артефактов. Генерирует случайный артефакт в каждый слот, общей ценой не более 100000.
    /// </summary>
    public class TestArtifactGenerationStrategy : IArtifactGenerationStrategy {

        private int _availableGold = 100000;

        public List<ArtifactID> Generate(HeroDataModel hero, SqliteConnection connection) {
            _availableGold = 1000000;
            List<ArtifactID> artifacts = new List<ArtifactID>();
            Random rnd = new Random();
            foreach (ArtifactSlot slot in Enum.GetValues(typeof(ArtifactSlot))) {
                if (slot == ArtifactSlot.INVENTORY) {
                    continue;
                }
                IEnumerable<ArtifactDataModel> artifactModels = connection.Query<ArtifactDataModel>($"SELECT * FROM Artifacts WHERE Slot = \"{slot}\"");
                if (artifactModels != null) {
                    IEnumerable<ArtifactDataModel> suitableArtifacts = artifactModels.
                        Where(m => ((IArtifactGenerationStrategy._bannedArtifacts.Contains(m.Id) == false) && (m.Cost < _availableGold)));
                    if (suitableArtifacts != null && suitableArtifacts.Count() > 0) {
                        int index = 0;
                        if (suitableArtifacts.Count() > 1) {
                            index = rnd.Next(0, suitableArtifacts.Count());
                        }
                        ArtifactDataModel selectedArtifact = suitableArtifacts.ElementAt(index);
                        artifacts.Add(selectedArtifact.Id);
                        _availableGold -= selectedArtifact.Cost;
                    }
                }
            }
            return artifacts;
        }
    }
}
