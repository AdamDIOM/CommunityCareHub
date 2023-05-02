using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace WellnessSite.Models
{
    public class Preferences
    {
        [Key]
        public string UserID { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Highlight { get; set; }
        [Required]
        public string Background { get; set; }
        [Required]
        public string Header { get; set; }
        [Required]
        public string HeaderText { get; set; }
        [Required]
        public string HeaderTextalt { get; set; }
        [Required]
        public string Footer { get; set; }
        [Required]
        public string FooterText { get; set; }
        [Required]
        public string FooterTextAlt { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public int TextSize { get; set; }

        public Preferences(string userID, string text, string highlight, string background, string header, string headerText, string headerTextalt, string footer, string footerText, string footerTextAlt, string link, int textSize) : this(userID)
        {
            Text = text;
            Highlight = highlight;
            Background = background;
            Header = header;
            HeaderText = headerText;
            HeaderTextalt = headerTextalt;
            Footer = footer;
            FooterText = footerText;
            FooterTextAlt = footerTextAlt;
            Link = link;
            TextSize = textSize;
        }

        public Preferences(string uid)
        {
            UserID = uid;
            Text = "black";
            Highlight = "cyan";
            Background = "white";
            Header = "green";
            HeaderText = "white";
            HeaderTextalt = "lightgrey";
            Footer = "darkgreen";
            FooterText = "white";
            FooterTextAlt = "lightgrey";
            Link = "blue";
            TextSize = 15;
        }

    }
}
