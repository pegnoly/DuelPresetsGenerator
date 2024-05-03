using DuelPresetsGenerator.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DuelPresetsGenerator.Common.Objects {
    /// <summary>
    /// Описывает тег, содержащий три значения одного типа(например Pos или Color)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Vec3<T> {
        public T? x { get; set; }

        public T? y { get; set; }

        public T? z { get; set; }
    }

    /// <summary>
    /// Описывает тег, представляющий собой ссылку на другой файл.
    /// </summary>
    [Serializable]
    public class FileRef {
        [XmlAttribute]
        public string? href { get; set; }
    }

    [Serializable]
    public class PointLight {
        public Vec3<int>? Pos { get; set; } = new Vec3<int>();

        public Vec3<float>? Color { get; set; } = new Vec3<float>();

        public int Radius { get; set; } = 0;
    }

    [Serializable]
    public class ArmySlot {
        public CreatureID Creature { get; set; }
        public int Count { get; set; }
    }
}
