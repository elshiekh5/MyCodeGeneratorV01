using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace SPGen
{
    class TableOptions
    {
        public TableOptions(string key)
        {
            this.key = key;
        }
        private string key;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        //------------------------------------------------
        //Options
        //------------------------------------------------
        private Hashtable options = new Hashtable(); 

        public Hashtable Options
        {
            get { return options; }
            set { options = value; }
        }
        //------------------------------------------------
        //AddTableOption
        public void AddTableOption(string key)
        {
            if (!options.Contains(key))
            {
                Options.Add(key, "");
            }
        }
        public static string CreateXmlNode(string key)
        {
            return CreateXmlNode(key, "1");
        }
        //-------------------------------------
        public static string CreateXmlNode(string key, string val)
        {
            StringBuilder xmlTag = new StringBuilder();
            //xmlTag
            xmlTag.Append("\n\t<data name=\"" + key + "\" xml:space=\"preserve\"><value>"+val+"</value></data>");
            //--------------------------
            return xmlTag.ToString();
        }
        //-------------------------------------
        public string GetTags()
        {
            StringBuilder xmlTags = new StringBuilder();
            foreach (DictionaryEntry key in Options)
            {
                xmlTags.Append(CreateXmlNode(key.Key.ToString()));
            }
            return xmlTags.ToString();
        }
        //-------------------------------------

    }
}
