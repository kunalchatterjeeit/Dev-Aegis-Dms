using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AegisDMS
{
    public partial class UserMessage : System.Web.UI.UserControl
    {
        [Description("Test text displayed in the textbox"), Category("Data")]
        public string Css
        {
            get { return CssClass.Attributes["class"]; }
            set
            {
                CssClass.Attributes.Add("class", value);
                switch (Css)
                {
                    case BusinessLayer.MessageCssClass.Success:
                        MessageType.InnerHtml = "Success: ";
                        break;
                    case BusinessLayer.MessageCssClass.Info:
                        MessageType.InnerHtml = "Info: ";
                        break;
                    case BusinessLayer.MessageCssClass.Warning:
                        MessageType.InnerHtml = "Warning: ";
                        break;
                    case BusinessLayer.MessageCssClass.Error:
                        MessageType.InnerHtml = "Error: ";
                        break;
                    default:
                        MessageType.InnerHtml = string.Empty;
                        break;
                }
            }
        }
        [Description("Test text displayed in the textbox"), Category("Data")]
        public string Text
        {
            set { Message.InnerHtml = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}