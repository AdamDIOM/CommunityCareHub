using Microsoft.AspNetCore.Mvc;
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
        public string Footer { get; set; }
        [Required]
        public string FooterText { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public int FontSize { get; set; }
        [Required]
        public string HexColour { get; set; }
        [Required]
        public string HexColour2 { get; set; }
        [Required]
        public string HexTextColour { get; set; }
        [Required]
        public string HexHover { get; set; }

        public Preferences(string userID, string text, string highlight, string background, string header, string headerText, string footer, string footerText, string link, int textSize, string hex1, string hex2, string hexColour, string hexHover) : this(userID)
        {
            Text = text;
            Highlight = highlight;
            Background = background;
            Header = header;
            HeaderText = headerText;
            Footer = footer;
            FooterText = footerText;
            Link = link;
            FontSize = textSize;
            HexColour = hex1;
            HexColour2 = hex2;
            HexTextColour = hexColour;
            HexHover = hexHover;
        }

        public Preferences(string uid)
        {
            UserID = uid;
            Text = "#000000";
            Highlight = "#00ffff";
            Background = "#ffffff";
            Header = "#f8ae51";
            HeaderText = "#ffffff";
            Footer = "#164d61";
            FooterText = "#ffffff";
            Link = "#0000ff";
            FontSize = 15;
            HexColour = "#cdd24e";
            HexColour2 = "#c1c8b0";
            HexTextColour = "#000000";
            HexHover = "#FFFF99";
        }

        public Preferences() : this("usr-x") { }


        public Preferences(string uid, int text, AccessibilityOptions choice)
        {
            UserID = uid;
            FontSize = text;
            if(choice == AccessibilityOptions.Contrast)
            {
                Text = "#000000";
                Highlight = "#00FFFF";
                Background = "#FFFFFF";
                Header = "#FFFF00";
                HeaderText = "#000000";
                Footer = "#00FFFF";
                FooterText = "#000000";
                Link = "#00FFFF";
                HexColour = "#FFFF00";
                HexColour2 = "#DDDD00";
                HexTextColour = "#000000";
                HexHover = "#FFFFFF";
            }
            if(choice == AccessibilityOptions.Greyscale)
            {
                Text = "#000000";
                Highlight = "#B2B2B2";
                Background = "#FFFFFF";
                Header = "#4B4B4B";
                HeaderText = "#FFFFFF";
                Footer = "#303030";
                FooterText = "#FFFFFF";
				Link = "#1C1C1C";
                HexColour = "#DDDDDD";
                HexColour2 = "#BBBBBB";
                HexTextColour = "#000000";
                HexHover = "#F1F1F1";
            }
            if(choice == AccessibilityOptions.Invert)
            {
                Text = "#FFFFFF";
                Highlight = "#FF0000";
                Background = "#000000";
                Header = "#0751ae";
                HeaderText = "#000000";
                Footer = "#e9b29e";
                FooterText = "#000000";
                Link = "#FFFF00";
                HexColour = "#322db1";
                HexColour2 = "#3e374f";
                HexTextColour = "#FFFFFF";
                HexHover = "#000077";
            }
        }

        public override string ToString()
        {
            return Text + ',' + Highlight + ',' + Background + ',' + Header + ',' + HeaderText + ',' + Footer + ',' + FooterText + ',' + Link + ',' + HexColour + ',' + HexColour2 + ',' + HexTextColour + ',' + HexHover + ',' + FontSize;
        }
    }
}
