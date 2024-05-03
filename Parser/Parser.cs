using DuelPresetsGenerator.Parsers.Base;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuelPresetsGenerator.Parser {
    public class Parser {

        private List<IParser> _parsers = new List<IParser>();
        private FilesDatabase? _database;

        public Parser(FilesDatabase database, List<IParser> parsers) {
            _database = database;
            _parsers = parsers;
        }

        public void Run() {
            using(SqliteConnection connection = new SqliteConnection($"Data Source = {Paths.Database}")) {
                connection.Open();
                string commandString = "BEGIN TRANSACTION; ";
                foreach(IParser parser in _parsers) {
                    commandString += parser.Parse(_database!);
                }
                commandString += "COMMIT";
                SqliteCommand command = new SqliteCommand(commandString, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
