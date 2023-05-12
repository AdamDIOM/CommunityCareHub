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
            Text = "#000000";
            Highlight = "#00ffff";
            Background = "#ffffff";
            Header = "#008000";
            HeaderText = "#ffffff";
            HeaderTextalt = "#00A000";
            Footer = "#005300";
            FooterText = "#ffffff";
            FooterTextAlt = "#007000";
            Link = "#0000ff";
            TextSize = 15;
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
                HeaderTextalt = "#003333";
                Footer = "#00FFFF";
                FooterText = "#000000";
                FooterTextAlt = "#333300";
                Link = "#00FFFF";
            }
            if(choice == AccessibilityOptions.Greyscale)
            {
                Text = "#000000";
                Highlight = "#B2B2B2";
                Background = "#FFFFFF";
                Header = "#4B4B4B";
                HeaderText = "#FFFFFF";
                HeaderTextalt = "D3D3D3";
                Footer = "#303030";
                FooterText = "#FFFFFF";
                FooterTextAlt = "#D3D3D3";
				Link = "#1C1C1C";
			}
            if(choice == AccessibilityOptions.Invert)
            {
                Text = "#FFFFFF";
                Highlight = "#FF0000";
                Background = "#000000";
                Header = "#FF7FFF";
                HeaderText = "#000000";
                HeaderTextalt = "#2C2C2C";
                Footer = "#FFACFF";
                FooterText = "#000000";
                FooterTextAlt = "#2C2C2C";
                Link = "#FFFF00";
            }
        }

        public override string ToString()
        {
            return Text + ',' + Highlight + ',' + Background + ',' + Header + ',' + HeaderText +',' + HeaderTextalt + ',' + Footer + ',' + FooterText + ',' + FooterTextAlt + ',' + Link + ',' + TextSize;
        }
    }
}
