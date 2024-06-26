﻿using DuelPresetsGenerator;
using DuelPresetsGenerator.Parsers.Base;
using DuelPresetsGenerator.Parsers;
using DuelPresetsGenerator.Parser;
using DuelPresetsGenerator.Generator;

string appPath = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", string.Empty);
Paths.Database = $"{appPath}\\database.db";

Console.WriteLine("Укажите путь к папке героев: ");
string? hommPath = Console.ReadLine();
Paths.HommData = hommPath + "data\\";

Console.WriteLine("Выберите режим работы: 1(парсить файлы игры) / 2(генерировать пресеты)");
string? mode = Console.ReadLine();

if (mode == "1") {
    FilesDatabase filesDatabase = new FilesDatabase();
    filesDatabase.Create(new DirectoryInfo(Paths.HommData));
    List<IParser> parsers = new List<IParser>() {
        new HeroesFilesParser(), new CreaturesFilesParser(), new ArtifactFilesParser(), new SpellsFilesParser(), new HeroSkillParser()
    };
    Parser parser = new Parser(filesDatabase, parsers);
    parser.Run();
}
else if (mode == "2") {
    Generator generator = new Generator();
    generator.Run();
}