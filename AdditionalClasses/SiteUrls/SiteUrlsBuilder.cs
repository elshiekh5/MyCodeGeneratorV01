using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
namespace SPGen
{
    public enum UrlType
    {
        Url,
        Parameter
    }
	/// <summary>
	/// Summary description for SiteUrlsBuilder.
	/// </summary>
	public class SiteUrlsBuilder 
	{
        //******************************UrlsPath***************************************//
        //PhotosUrl
        public static string photoOriginalUrl = "App_Files/{0}/Photos/Thumbnail/Original/";
        public static string photoMicroUrl = "App_Files/{0}/Photos/Thumbnail/Micro/";
        public static string photoMiniUrl = "App_Files/{0}/Photos/Thumbnail/Mini/";
        public static string photoNormalUrl = "App_Files/{0}/Photos/Thumbnail/Normal/";
        public static string photoBigUrl = "App_Files/{0}/Photos/Thumbnail/Big/";
        //----------------------------------------------------
        //LogosUrl
        public static string logoOriginalUrl = "App_Files/{0}/Logos/Thumbnail/Original/";
        public static string logoMicroUrl = "App_Files/{0}/Logos/Thumbnail/Micro/";
        public static string logoMiniUrl = "App_Files/{0}/Logos/Thumbnail/Mini/";
        public static string logoNormalUrl = "App_Files/{0}/Logos/Thumbnail/Normal/";
        public static string logoBigUrl = "App_Files/{0}/Logos/Thumbnail/Big/";
        //----------------------------------------------------
        //FilesUrl
        public static string filesUrl = "App_Files/{0}/Files/";
        //----------------------------------------------------
        //OtherFilesUrl
        public static string otherFilesUrl = "App_Files/{0}/{1}/";
        //----------------------------------------------------
        /**************************************************************/
        private static Hashtable TablesUrls = new Hashtable();
        private static Hashtable UrlsChecker = new Hashtable();
        /**************************************************************/
        #region Create
        public static void Create()
        {
            CreateTablesUrlFile();
            CreateTablesUrlClass();
        }
        #endregion
        /**************************************************************/
        #region CreateTablesUrlFile
        public static void CreateTablesUrlFile()
        {
            //-----------------------------------
            string configFile = "ProjectFiles/TableUrls.resx";
            StreamReader _reader = null;
            string lineOfText;
            StringBuilder sb = new StringBuilder();
            if (false == System.IO.File.Exists(configFile))
            {
                throw new Exception("File " + configFile + " does not exists");
            }
            string xmlData = "";
            string urlsFile = "";
            string directoryPath = "";
            string path = "";
            using (Stream stream = System.IO.File.OpenRead(configFile))
            {
                _reader = new StreamReader(stream);
                while (true)
                {
                    lineOfText = _reader.ReadLine();
                    if (lineOfText == null)
                    {
                        xmlData = sb.ToString();
                        TableUrls url;
                        string xmlTags;
                        foreach (DictionaryEntry key in TablesUrls)
                        {
                            url = (TableUrls)key.Value;
                            xmlTags = url.Tags.ToString();
                            urlsFile = xmlData.Replace("{R}", xmlTags);
                            //Directory Check
                            directoryPath = Globals.ModulesGlobalResources + url.Key + "\\";
                            if (!Directory.Exists(directoryPath))
                                Directory.CreateDirectory(directoryPath);
                            //------------------------
                            // Create a file to write to.
                            path = directoryPath + url.Key + "Urls.resx";
                            //------------------------
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine(urlsFile);
                            }
                        }
                        return;
                        //-----------------------------------
                    }
                    else
                        sb.Append(lineOfText + Environment.NewLine);
                }

            }
        }
        #endregion
        /**************************************************************/
        #region CreateTablesUrlClass
        public static void CreateTablesUrlClass()
        {
            //-----------------------------------
            string urlClass = "ProjectFiles/TableUrls.cs";
            StreamReader _reader = null;
            string lineOfText;
            StringBuilder sb = new StringBuilder();
            if (false == System.IO.File.Exists(urlClass))
            {
                throw new Exception("File " + urlClass + " does not exists");
            }
            string classData = "";
            string urlsFile = "";
            string path = "";
            using (Stream stream = System.IO.File.OpenRead(urlClass))
            {
                _reader = new StreamReader(stream);
                while (true)
                {
                    lineOfText = _reader.ReadLine();
                    if (lineOfText == null)
                    {
                        classData = sb.ToString();
                        TableUrls url;
                        string classProperties;
                        foreach (DictionaryEntry key in TablesUrls)
                        {
                            url = (TableUrls)key.Value;
                            classProperties = url.Propreties.ToString();
                            urlsFile = classData.Replace("{R}", classProperties);
                            urlsFile = urlsFile.Replace("{T}", url.Key);
                            //Ar File
                            path = Globals.ClassesDirectory + "\\" + url.Key + "\\" + url.Key + "Urls.cs";
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine(urlsFile);
                            }
                        }
                        return;
                        //-----------------------------------
                    }
                    else
                        sb.Append(lineOfText + Environment.NewLine);
                }

            }
        }
        #endregion
        /**************************************************************/
        #region AddTableUrl
        //-------------------------------------
        public static string AddTableUrl(string key,string url,UrlType type)
        {
            return AddTableUrl(SqlProvider.obj.TableName, key, url, type);
        }
        //-------------------------------------
        public static string AddTableUrl(string tableId, string key, string url, UrlType type)
        {
            key = Globals.GetProgramatlyName(key);
            TableUrls tableUrls;
            if (!TablesUrls.Contains(tableId))
            {
                tableUrls = new TableUrls(tableId);
                tableUrls.Key = tableId;
                TablesUrls.Add(tableId, tableUrls);
            }
            else
            {
                tableUrls = (TableUrls)TablesUrls[tableId];
            }
            tableUrls.AddUrl(key, url, type);
            
            //-------------------------
            return tableId+"Urls.Instance." + key; ;
        }
        #endregion
        //-------------------------------------
        //-------------------------------------
        public static void AddParameter(string id)
        {
            AddTableUrl(id, id, UrlType.Parameter);
        }
        //-------------------------------------
        public static void AddParameter(string id,string url)
        {
            AddTableUrl(id, url, UrlType.Parameter);
        }
        //-------------------------------------
        public static void AddDirectoryUrl(string id, string url, string baseFolder, string fileFolder)
        {
            if (!UrlsChecker.Contains(id))
            {
                url = string.Format(url, new string[] { baseFolder,fileFolder });
                string phisicalDirectory = Globals.BaseDirectory + url.Replace("/", "\\");
                if (!Directory.Exists(phisicalDirectory))
                    Directory.CreateDirectory(phisicalDirectory);
                AddTableUrl(id, url, UrlType.Url);
            }
        }
        //-------------------------------------
        public static void AddUrl(string id, string url)
        {
            AddTableUrl(id,url, UrlType.Url);
        }
        //-------------------------------------
        public static string GetIdentifire()
        {
            return SqlProvider.obj.TableName + "Urls.Instance.";
        }
        //-------------------------------------
        public static string GetGlobalIdentifire()
        {
            return "SiteUrls.Instance.";
        }
        //-------------------------------------
        public static string GetGlobalResolveMethodIdentifire()
        {
            return "SiteUrls.Instance.PopulateUrl";
        }
        //-------------------------------------
        public static string GetFullDeclaration()
        {
            string classIdentifire=SqlProvider.obj.TableName +"Urls";
            return classIdentifire + " urls = " + classIdentifire + ".Instance;";
        }
        //-------------------------------------
        //-------------------------------------
        public static string GetTheGetUrlMethodIdentifire()
        {
            return SqlProvider.obj.TableName + "Urls.Instance.GetUrl";
        }
        //-------------------------------------
	}
}

