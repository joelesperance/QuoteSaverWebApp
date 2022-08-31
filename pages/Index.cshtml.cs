using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using QuoteSaverApp.Pages.Models;

namespace QuoteSaverApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<QuoteModel> Records { get; set; }

        public void OnGet()
        {
            Records = GetAllRecords();
        }
        private List<QuoteModel> GetAllRecords()
        {
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    $"SELECT * FROM quoteTable";

                var tableData = new List<QuoteModel>();
                SqliteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tableData.Add(
                        new QuoteModel
                        {
                            Id = reader.GetInt32(0),
                            Quote = reader.GetString(1),
                            Source = reader.GetString(2),
                        });
                }
                return tableData;
            }
        }
    }
}