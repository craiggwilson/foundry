using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundry.Website.Models
{
    public enum ViewMessageType
    {
        Info,
        Warning,
        Error
    }

    public class ViewMessageModel
    {
        public ViewMessageType MessageType { get; set; }

        public string Text { get; set; }

        public ViewMessageModel()
        { }

        public ViewMessageModel(string text, ViewMessageType type)
        {
            MessageType = type;
            Text = text;
        }
    }
}