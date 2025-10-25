using DemoApp.Api.Interfaces;

namespace DemoApp.Api.Implementation
{
    public class FileDataAccess : IDataAccess
    {
        public async Task<string> CreateRecord(string note)
        {
            var id = Guid.NewGuid();
            string record = parsearregistro(note, id);
            using StreamWriter writer = File.AppendText("Db.txt");
            await escribirRegistro(record, writer);
            return id.ToString();
        }

        private static string parsearregistro(string note, Guid id)
        {
            return $"'{id}', '{note}'";
        }

        private static async Task escribirRegistro(string record, StreamWriter writer)
        {
            await writer.WriteLineAsync(record);
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

        public async Task<Record?> ModificarRecord(Guid id, string note)
        {
            var lines = await GetAllRecords();
            var record = lines.FirstOrDefault(record => record.Id == id);


            if (record == null)
            {
                return null;
            }

            record.Note = note;
            using StreamWriter writer = await actualizarRegistros(lines);

            return record;
        }

        private static async Task<StreamWriter> actualizarRegistros(List<Record> lines)
        {
            // eliminar archivo db original
            File.Delete("Db.txt");
            StreamWriter writer = File.AppendText("Db.txt");

            foreach (var line in lines)
            {
                await escribirRegistro(parsearregistro(line.Note, line.Id), writer);
            }

            return writer;
        }

        public async Task DeleteRecord(Guid id) 
        { 
            var lines = await GetAllRecords();
            var record = lines.FirstOrDefault(record => record.Id == id);
            if (record == null)
            {
                return;
            }
            lines.Remove(record);
            using StreamWriter writer = await actualizarRegistros(lines);
        }

        /*refactoring.*/
        public async Task<Record?> GetRecordsById(Guid id)
        {
            var lines = await GetAllRecords();
            return lines.FirstOrDefault(record => record.Id == id);
        }
    }
}
