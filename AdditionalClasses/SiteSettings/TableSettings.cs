using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace SPGen
{
    
    class TableSettings
    {
        public  StringBuilder Propreties = new StringBuilder();
        public  StringBuilder Tags = new StringBuilder();
        public TableSettings(string key)
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
        //Settings
        //------------------------------------------------
        private Hashtable settings = new Hashtable();

        public Hashtable Settings
        {
            get { return settings; }
            set { settings = value; }
        }
        //------------------------------------------------
 
        //AddSetting
        public void AddSetting(string key, SettingType type)
        {
            if (!settings.Contains(key))
            {
                Propreties.Append(CreateSettingPropertyBlock(key,type));
                Tags.Append(CreateXmlNode( key, type));
                settings.Add(key, null);
            }
        }
        //------------------------------------------------
        private string CreateSettingPropertyBlock(string key, SettingType type)
        {
            StringBuilder block = new StringBuilder();
            string datatype = "string";

            string propertyBody = "get { return Resources." + this.Key + "Settings." + key + "; }";
            if (type == SettingType.MaxLength)
            {
                datatype = "int";
                propertyBody = "get { return Convert.ToInt32(Resources." + this.Key + "Settings." + key + "); }";
            }
            //Property Blook
            block.Append("\n\t#region --------------" + key + "--------------");
            block.Append("\n\t//---------------------------------");
            block.Append("\n\t//" + key);
            block.Append("\n\t//---------------------------------");
            block.Append("\n\tpublic " + datatype + " " + key);
            block.Append("\n\t{");
            block.Append("\n\t\t" + propertyBody);
            block.Append("\n\t}");
            block.Append("\n\t//----------------------");
            block.Append("\n\t#endregion");
            //--------------------------
            return block.ToString();
        }
        //------------------------------------------------
        public  string CreateXmlNode(string key, SettingType type)
        {
            string val = "";
            if (type == SettingType.MaxLength)
                val = "-1";
            return CreateXmlNode(key, val);
        }
        //-------------------------------------
        public  string CreateXmlNode(string key, string val)
        {
            StringBuilder xmlTag = new StringBuilder();
            //xmlTag
            xmlTag.Append("\n\t<data name=\"" + key + "\" xml:space=\"preserve\"><value>" + val + "</value></data>");
            //--------------------------
            return xmlTag.ToString();
        }
        //-------------------------------------
        /**************************************************************************************************/
    }
}
