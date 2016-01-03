using System.Web;

namespace Vincente.WebApp.Helpers
{
    public class TrelloHelper
    {
        public static HtmlString CardLink(string cardId, string text)
        {
            return new HtmlString(string.Format(
                "<a target=\"_blank\" href=\"https://trello.com/c/{0}\">{1}</a>",
                cardId, text));
        }
    }
}