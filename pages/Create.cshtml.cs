using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using QuoteSaverApp.Pages.Models;

namespace QuoteSaverApp.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public IActionResult OnGet()
        {
            return Page();
        }

    [BindProperty]
    public QuoteModel Quote { get; set; }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }
            string escapedQuote = Quote.Quote.Replace("'", "''");
            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText =
                    @$"INSERT INTO quoteTable(Quote, Source) VALUES('{escapedQuote}', '{Quote.Source}')";

                cmd.ExecuteNonQuery();
                connection.Close();
            }

            return RedirectToPage("/Index");
        }
    }
}
