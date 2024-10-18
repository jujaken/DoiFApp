using Microsoft.Data.Sqlite;

namespace DoiFApp.Services
{
    public class SqliteDbCopy : IDbCopier
    {
        public async Task Copy(string source, string target)
        {
            using var sourceConnection = new SqliteConnection("Data Source=" + source);
            await sourceConnection.OpenAsync();

            using var targetConnection = new SqliteConnection("Data Source=" + target);
            await targetConnection.OpenAsync();

            var getTablesQuery = "SELECT name FROM sqlite_master WHERE type='table'";
            using var getTablesCommand = new SqliteCommand(getTablesQuery, sourceConnection);
            using var reader = await getTablesCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var tableName = reader.GetString(0);
                await CopyTableData(sourceConnection, targetConnection, tableName);
            }
        }

        private async Task CopyTableData(SqliteConnection sourceConnection, SqliteConnection targetConnection, string tableName)
        {
            var selectQuery = $"SELECT * FROM {tableName}";
            using var selectCommand = new SqliteCommand(selectQuery, sourceConnection);
            using var reader = await selectCommand.ExecuteReaderAsync();

            var columnNames = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
                columnNames[i] = reader.GetName(i);

            var insertQuery = $"INSERT INTO {tableName} ({string.Join(", ", columnNames)}) VALUES ({string.Join(", ", columnNames.Select(c => "@" + c))})";
            using var insertCommand = new SqliteCommand(insertQuery, targetConnection);

            while (await reader.ReadAsync())
            {
                insertCommand.Parameters.Clear();
                foreach (var columnName in columnNames)
                    insertCommand.Parameters.AddWithValue("@" + columnName, reader[columnName]);

                await insertCommand.ExecuteNonQueryAsync();
            }
        }
    }
}
