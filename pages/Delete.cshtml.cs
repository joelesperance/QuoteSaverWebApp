using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using QuoteSaverApp.Pages.Models;

namespace QuoteSaverApp.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [BindProperty]
        public QuoteModel Quote { get; set; }




        public IActionResult OnGet(int id)
        {
            Quote = GetById(id);

            return Page();
        }

        private QuoteModel GetById(int id)
        {
            var quoteModelRecord = new QuoteModel();

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    $"SELECT * FROM quoteTable WHERE Id = {id}";

                SqliteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    quoteModelRecord.Id = reader.GetInt32(0);
                    quoteModelRecord.Quote = reader.GetString(1);
                    quoteModelRecord.Source = reader.GetString(2);
                }

                return quoteModelRecord;
            }
        }



        public IActionResult OnPost(int id)
        {
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    $"DELETE FROM quoteTable WHERE Id = {id}";

                cmd.ExecuteNonQuery();
                connection.Close();
            }
            return RedirectToPage("/Index");
        }
    }
}
