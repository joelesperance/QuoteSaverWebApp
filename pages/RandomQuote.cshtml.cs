using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using QuoteSaverApp.Pages.Models;

namespace QuoteSaverApp.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public PrivacyModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<QuoteModel> RandomRecord { get; set; }

        public void OnGet()
        {
            RandomRecord = GetRandomRecord();
        }
        private List<QuoteModel> GetRandomRecord()
        {
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @$"SELECT Quote FROM quoteTable";

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