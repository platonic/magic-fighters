// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//Enemy.cs
//Author        : Lisandro Martinez
//Comments      : 
//Date          : 9/27/2012
//Last Modified : 9/27/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Input;

namespace MagicFighters.Screens
{
    class MF_TodoList
    {
        public static List<string> info = new List<string>();
        static string[] Attr =
        {
            "AssignedName",
            "AssistantName",
            "Completed",
            //"CompletedBy",
            //"Date",
            "Comments",
            //"ImportanceLevel",
            "NeedAssistance",
            //"ClassName",

        };
        static string[] Header =
        {
            "Name",
            "Asstnt",
            "Completed",
            //"CompletedBy",
            //"Date",
            "Comments",
            //"ImportanceLevel",
            "NeedAsstnt",
            //"ClassName",

        };
        public MF_TodoList()
        {
            Load();
        }

        public static void Show()
        {
            string message = "";
            foreach (var at in Header)
                message += at + "\t";
            message += "\n\n";
            foreach (var m in info)
                message += m + "\n";
            MessageBox.Show(message,"Sellect OK with mouse.");
           
        }
        public static void Load()
        {

            //load Todo information from the XML file
            XDocument doc = XDocument.Load("Content/TodoItems.xml");
            XName TODO = XName.Get("TODO");
            var TODOItems = doc.Document.Descendants(TODO);

            foreach (var item in TODOItems)
            {
                string a = "";
                foreach (var at in Attr.Take(Attr.Length-1))
                    a += String.IsNullOrEmpty(item.Attribute(at).Value) ? "n/a\t\t" : item.Attribute(at).Value + "\t";
                a += String.IsNullOrEmpty(item.Attribute(Attr[Attr.Length - 1]).Value) ? "n/a" : item.Attribute(Attr[Attr.Length - 1]).Value;

                info.Add(a);
               
            }
        }
    }
}
