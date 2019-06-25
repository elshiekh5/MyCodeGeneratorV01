using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace SPGen
{
    
    class TableUrls
    {

        public  StringBuilder Propreties = new StringBuilder();
        public  StringBuilder Tags = new StringBuilder();

        
        //FilesUrl
        public static string filesUrl = "App_Files/{0}/Files/";
        //----------------------------------------------------
        //OtherFilesUrl
        public static string otherFilesUrl = "App_Files/{0}/{1}/";
        public TableUrls(string key)
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
        //Urls
        //------------------------------------------------
        private Hashtable urls = new Hashtable();

        public Hashtable Urls
        {
            get { return urls; }
            set { urls = value; }
        }
        //------------------------------------------------
 
        //AddUrl
        /*
        public void AddUrl(string key, UrlType type)
        {
            if (!urls.Contains(key))
            {
                Propreties.Append(CreateUrlPropertyBlock(key,type));
                Tags.Append(CreateXmlNode( key, type));
                urls.Add(key, null);
            }
        }*/
        //------------------------------------------------
        public void AddUrl(string key,string url ,UrlType type)
        {
            if (!urls.Contains(key))
            {
                Propreties.Append(CreateUrlPropertyBlock(key, type));
                Tags.Append(CreateXmlNode(key, url));
                urls.Add(key, null);
            }
        }
        //------------------------------------------------
        private string CreateUrlPropertyBlock(string key, UrlType type)
        {
            StringBuilder block = new StringBuilder();
            string datatype = "string";
            string propertyBody = "get { return Resources." + this.Key + "Urls." + key + "; }";

            if(type==UrlType.Url)
                propertyBody = "get { return " + SiteUrlsBuilder.GetGlobalResolveMethodIdentifire()+ "(Resources." + this.Key + "Urls." + key + "); }";

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
        public  string CreateXmlNode(string key, UrlType type)
        {
            string val = "";
            if (type == UrlType.Parameter)
                val = key;
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
