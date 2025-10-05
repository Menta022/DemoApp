using DemoApp.Api.Interfaces;

namespace DemoApp.Api.Implementation
{
    public class FileDataAccess : IDataAccess
    {
        public async Task<string> CreateRecord(string note)
        {
            var id = Guid.NewGuid();
            var record = $"'{id}', '{note}'";
            using StreamWriter writer = File.AppendText("Db.txt");
            await writer.WriteLineAsync(record);
            return id.ToString();
        }



        public async Task<List<Record>> GetAllRecords()
        {
            var lines = await File.ReadAllLinesAsync("Db.txt");

            var records = new List<Record>();
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                records.Add(new Record
                {
                    Id = Guid.Parse(parts[0].Trim().Trim('\'', ' ')),
                    Note = parts[1].Trim().Trim('\'', ' ')
                });
            }

            return records;
        }

        /*refactoring.*/


        public async Task<Record?> GetRecordsById(string id)
        {
            /*guard clauses*/
            if (!Guid.TryParse(id, out var validId))
                return default;

            var lines = await GetAllRecords();
            return lines.FirstOrDefault(record => record.Id == validId);          
        }
    }
}
