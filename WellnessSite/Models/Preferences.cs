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
        public int TextSize { get; set; }
        [Required]
        public string Hex1 { get; set; }
        [Required]
        public string Hex2 { get; set; }
        [Required]
        public string HexColour { get; set; }
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
            TextSize = textSize;
            Hex1 = hex1;
            Hex2 = hex2;
            HexColour = hexColour;
            HexHover = hexHover;
        }

        public Preferences(string uid)
        {
            UserID = uid;
            Text = "#000000";
            Highlight = "#00ffff";
            Background = "#ffffff";
            Header = "#008000";
            HeaderText = "#ffffff";
            Footer = "#005300";
            FooterText = "#ffffff";
            Link = "#0000ff";
            TextSize = 15;
            Hex1 = "#22AA22";
            Hex2 = "#44CC44";
            HexColour = "#000000";
            HexHover = "#FFFF99";
        }

        public Preferences() : this("usr-x") { }


        public Preferences(string uid, int text, AccessibilityOptions choice)
        {
            UserID = uid;
            TextSize = text;
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
                Hex1 = "#FFFF00";
                Hex2 = "#DDDD00";
                HexColour = "#000000";
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
                Hex1 = "#DDDDDD";
                Hex2 = "#BBBBBB";
                HexColour = "#000000";
                HexHover = "#F1F1F1";
            }
            if(choice == AccessibilityOptions.Invert)
            {
                Text = "#FFFFFF";
                Highlight = "#FF0000";
                Background = "#000000";
                Header = "#FF7FFF";
                HeaderText = "#000000";
                Footer = "#FFACFF";
                FooterText = "#000000";
                Link = "#FFFF00";
                Hex1 = "#DD55DD";
                Hex2 = "#BB33BB";
                HexColour = "#FFFFFF";
                HexHover = "#000077";
            }
        }

        public override string ToString()
        {
            return Text + ',' + Highlight + ',' + Background + ',' + Header + ',' + HeaderText + ',' + Footer + ',' + FooterText + ',' + Link + ',' + Hex1 + ',' + Hex2 + ',' + HexColour + ',' + HexHover + ',' + TextSize;
        }
    }
}
