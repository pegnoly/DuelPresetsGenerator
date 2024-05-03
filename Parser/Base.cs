using Ionic.Zip;

namespace DuelPresetsGenerator.Parsers.Base {

    public interface IParser {
        string Parse(FilesDatabase database);
    }

    /// <summary>
    /// База данных файлов игры для парсинга.
    /// </summary>
    public class FilesDatabase {

        private Dictionary<string, string> files = new Dictionary<string, string>();
        private Dictionary<string, DateTime> modifiedTimes = new Dictionary<string, DateTime>();

        /// <summary>
        /// Чекает паки в указанной папке и производит словарь актуальных файлов игры и их содержимого.
        /// </summary>
        /// <param name="directory"></param>
        public void Create(DirectoryInfo directory) {
            foreach (FileInfo file in directory.GetFiles()) {
                if (file.Extension == ".pak") {
                    using (ZipFile zip = ZipFile.Read(file.FullName)) {
                        foreach (ZipEntry entry in zip.Entries) {
                            if (IsUsableFile(entry.FileName)) {
                                if (!files.ContainsKey(entry.FileName) || (modifiedTimes[entry.FileName] < entry.LastModified)) {
                                    StreamReader reader = new StreamReader(entry.OpenReader());
                                    files[entry.FileName] = reader.ReadToEnd();
                                    modifiedTimes[entry.FileName] = entry.LastModified;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Определяет подходящие для парсинга файлы(находятся по путям, которые начинаются с путей MapObjects и GameMechanics и имеют расширения xdb или xml)
        /// </summary>
        /// <param name="fileName">Имя проверяемого файла</param>
        /// <returns></returns>
        private bool IsUsableFile(string fileName) {
            return (fileName.StartsWith("MapObjects") || fileName.StartsWith("GameMechanics")) &&
                    (fileName.EndsWith(".xdb") || fileName.EndsWith(".xml"));
        }

        /// <summary>
        /// Возвращает содержимое файла по его ключу.
        /// </summary>
        /// <param name="key">Имя файла в файлах игры.</param>
        /// <returns>Содержимое файла в текстовом формате</returns>
        public string? GetFile(string key) {
            if (!files.ContainsKey(key)) {
                return null;
            }
            return files[key];
        }
    }
}
