using DuelPresetsGenerator.Common;
using DuelPresetsGenerator.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuelPresetsGenerator.Entities.Hero {
    [Serializable]
    public class Editable {
        public FileRef? NameFileRef { get; set; }
    }

    [Serializable]
    public class AdvMapHeroShared {
        public FileRef? FaceTexture { get; set; }
        public HeroClass Class { get; set; }
        public TownType TownType { get; set; }
        public Editable? Editable { get; set; }
    }
}
