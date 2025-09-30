namespace DemoApp.Api.Interfaces
{
    public interface IDataAccess
    {
        Task<string> CreateRecord(string note);
        Task<List<Record>> GetAllRecords();
        Task<List<Record>> GetAllRecordsId(string id);
    }

    public class Record
    {
        public Guid Id { get; set; }
        public string Note { get; set; }
    }
}
