using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalRProject.Classes.Google;

namespace SignalRProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            TextToSpeach.CreаteTextToAudiоFile("Mooi man, dit gaat naar spraak.", "test.mp3");
        }
    }
}
