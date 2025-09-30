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

            //return lines.Select(line =>
            //{
            //    var parts = line.Split(',');
            //    return new Record
            //    {
            //        Id = Guid.Parse(parts[0].Trim().Trim('\'', ' ')),
            //        Note = parts[1].Trim().Trim('\'', ' ')
            //    };
            //}).ToList();
        }

        public async Task<List<Record>> GetAllRecordsId(string id)
        {
            var lines = await File.ReadAllLinesAsync("Db.txt");
            var idlines = lines.Where(line => line.Contains(id)).ToArray();

            var records = new List<Record>();
            foreach (var line in idlines)
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
    }
}
