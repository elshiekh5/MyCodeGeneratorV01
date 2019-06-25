using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace SPGen
{
    class TableResource
    {
        public  TableResource(string key)
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
        //AdminTexts
        //------------------------------------------------
        private Hashtable adminTexts = new Hashtable(); 

        public Hashtable AdminTexts
        {
            get { return adminTexts; }
            set { adminTexts = value; }
        }
        //------------------------------------------------
        //AddAdminText
        public void AddAdminText(string key ,string value)
        {
            if (!adminTexts.Contains(key))
            {
                AdminTexts.Add(key, value);
            }
        }
        //------------------------------------------------

        //------------------------------------------------
        //UserTexts
        //------------------------------------------------
        private Hashtable userTexts = new Hashtable();

        public Hashtable UserTexts
        {
            get { return userTexts; }
            set { userTexts = value; }
        }
        //------------------------------------------------
        //AddUserText
        public void AddUserText(string key, string value)
        {
            if (!userTexts.Contains(key))
            {
                UserTexts.Add(key, value);
            }
        }
        //-------------------------------------
        public static string CreateTextXmlNode(string key, string text)
        {
            text = text.Replace("BoxHeader", "");
            char[] Capitals=new char[]{'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
            int CharIndex=-1;
            for (int i = 0; i < Capitals.Length; i++)
            {
                CharIndex = text.IndexOf(Capitals[i]);
                if (CharIndex > 0)
                    text=text.Replace(Capitals[i].ToString(), " " + Capitals[i].ToString().ToLower());
            }
            text=text.Replace(" i d", "");
            StringBuilder xmlTag = new StringBuilder();
            //xmlTag
            xmlTag.Append("\n\t<data name=\"" + key + "\" xml:space=\"preserve\"><value>" + text + "</value></data>");
            //--------------------------
            return xmlTag.ToString();
        }
        //-------------------------------------
        public string GetAdminTags()
        {
            StringBuilder xmlTags = new StringBuilder();
            foreach (DictionaryEntry key in AdminTexts)
            {
                xmlTags.Append(CreateTextXmlNode(key.Key.ToString(), key.Value.ToString()));
            }
            return xmlTags.ToString();
        }
        //-------------------------------------
        public string GetUserTags()
        {
            StringBuilder xmlTags = new StringBuilder();
            foreach (DictionaryEntry key in UserTexts)
            {
                xmlTags.Append(CreateTextXmlNode(key.Key.ToString(), key.Value.ToString()));
            }
            return xmlTags.ToString();
        }
        //-------------------------------------
    }
}
