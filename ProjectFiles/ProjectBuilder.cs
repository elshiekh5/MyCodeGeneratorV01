using System;
using System.IO;
using System.Text;
namespace SPGen
{
    public enum ProjectType
    {
        Simple,
        All
    }
    public enum ProjectVersions
    {
        V2005,
        V2008
    }
	/// <summary>
	/// Summary description for ProjectBuilder.
	/// </summary>
	public class ProjectBuilder 
	{
		public static string ProjectName="";
		public static string PhysicalPath="";
		public static string ProjectPort = "";
		public static string ServerName="";
        public static bool CreateSecurityModel = false;
        //public static bool HasMasterBox = false;
        public static bool HasMultiLanguages = false;
        public static bool AllowAdminSorting = true;
        public static bool IsLabelText = true;
        public static bool HasProprety = true;
        public static bool ISExcuteScaler = true;
        public static string IdentityText = "";
        public static bool AllowXmlDocumentation = false;
        public static bool IsFreeTextBoxEditor = true;

        public static bool IsGlobalResource = true;
		public static ProjectType ProjectType ;
		public static string PhotoExtensionColumnName = "PhotoExtension";
		public static string PhotoExtensionColumnNameVariable = "photoExtension";
		public static string LogoExtensionColumnName = "LogoExtension";
		public static string LogoExtensionColumnNameVariable = "logoExtension";
		public static string FileExtensionColumnName = "FileExtension";
		public static string FileExtensionColumnNameVariable = "fileExtension";
        public static string ExtensionInColumnName = "Extension";
        public static string LangID = "langid";
        public static string IsAvailable = "IsAvailable";
        public static string IsAvailableConditionParam = "IsAvailableCondition";
        public static string PriorityColumnName = "Priority";
        public static string PriorityDropDownList = "ddlPriority";

		private static bool  _HasOptionalConfiguration= true;

		public static bool  HasConfiguration
		{
			get 
			{
				if (_HasOptionalConfiguration && IsEntityTable)
					return true;
				else
					return false;
			}
			set { _HasOptionalConfiguration = value; }
		}
	
		public static bool IsEntityTable = false;
        public static string NameSpace
        {
            get { return ProjectName; }
        }
		public static void CreateProject()
		{
			CreateThetemplateFiles();
			SqlProvider.obj.Connect();
			
			foreach(SQLDMO.Table table in SqlProvider.obj.Tables )
			{
                if (!table.SystemObject && table.Name.ToLower() != "sysdiagrams")
				{
					
                    SqlProvider.obj.Refresh();
					SqlProvider.obj.Table=table;
					SqlProvider.obj.TableName=table.Name;
					//---------------------------------
					if (SqlProvider.obj.ISTableForRelations)
					{
						IsEntityTable = false;
					}
					else
					{
						IsEntityTable = true;
					}
					//---------------------------------
                    StoredProcedure.Create();
					SqlDataProviderBuilder.Create();
					//DataProviderBuilder.Create();This Step was Canceled
                    ClassEntityBuilder.Create();
                    ClassFactoryBuilder.Create();
                    
					if(!SqlProvider.obj.ISTableForRelations)
					{
						//Create User Control
						Create_InterfaceBuilder.Create(InterfaceType.WEbUserControl);
						Create_CodeBehindBuilder.Create(InterfaceType.WEbUserControl);
						//Admin Add Page
						Create_InterfaceBuilder.Create(InterfaceType.WebForm);
						Create_CodeBehindBuilder.Create(InterfaceType.WebForm);
						//-----------------------------------------------------------
						//Edit User Control
						Update_CodeBehindBuilder.Create(InterfaceType.WEbUserControl);
						Update_InterfaceBuilder.Create(InterfaceType.WEbUserControl);
						//Admin Edit Page
						Update_CodeBehindBuilder.Create(InterfaceType.WebForm);
						Update_InterfaceBuilder.Create(InterfaceType.WebForm);
						//-----------------------------------------------------------
                        if (AllowAdminSorting)
                        {
                            //Get All WithSorting User Control
                            GetAllWithSorting_InterfaceBuilder.Create(InterfaceType.WEbUserControl); 
                            GetAllWithSorting_CodeBehindBuilder.Create(InterfaceType.WEbUserControl);
                            //Admin Default With Sorting Page
                            GetAllWithSorting_InterfaceBuilder.Create(InterfaceType.WebForm);
                            GetAllWithSorting_CodeBehindBuilder.Create(InterfaceType.WebForm);
                        }
                        else
                        {
                            //Get All User Control
                            GetAll_InterfaceBuilder.Create(InterfaceType.WEbUserControl);
                            GetAll_CodeBehindBuilder.Create(InterfaceType.WEbUserControl);
                            //Admin Default Page
                            GetAll_InterfaceBuilder.Create(InterfaceType.WebForm);
                            GetAll_CodeBehindBuilder.Create(InterfaceType.WebForm);
                        }
						//-----------------------------------------------------------
                       
                        //-----------------------------------------------------------
						//Get All For User User Control
						GetAllForUser_InterfaceBuilder.Create();
						GetAllForUser_CodeBehindBuilder.Create();
						//-----------------------------------------------------------
						//Details User Control
						Details_InterfaceBuilder.Create();
						Details_CodeBehindBuilder.Create();
						//-----------------------------------------------------------
						//Default page
						UserDefaultPage_InterfaceBuilder.Create();
						UserPage_CodeBehindBuilder.Create();
						//-----------------------------------------------------------
						//Details
						DetailsPage_InterfaceBuilder.Create();
						DetailsPage_CodeBehindBuilder.Create();
						//-----------------------------------------------------------
						//
						AppTemplateBuilder.AddTemplate();
						AdminNavigationBuilder.AddItems();
                        //-----------------------------------------------------------
					}
                    
				}
			}
			if (ProjectBuilder.ProjectType == ProjectType.All)
			{
				SecurityBuilder.Create();
			}
			
			SqlProvider.obj.DisConnect();
			//SiteUrlsBuilder
            SiteUrlsBuilder.Create();
			//SiteUrlsBuilder.CreateSiteUrlsConfig();
			//-------------------------------------
			//SiteSettingsBuilder
			SiteSettingsBuilder.Create();
			//SiteSettingsBuilder.CreateSiteSettingsConfig();
			//-------------------------------------
			//
            ResourcesTesxtsBuilder.Create();
            SiteOptionsBuilder.Create();
/*
			//SiteOptionsBuilder
			if (HasConfiguration)
			{
				SiteOptionsBuilder.CreateSiteOptionsClass();
				SiteOptionsBuilder.CreateSiteOptionsConfig();
			}*/
			//-------------------------------------
			//
            /*ResourcesTesxtsBuilder.CreateAdminMenueArXmlConfig();*/
            //ResourcesTesxtsBuilder.CreateAdminMenueEnXmlConfig();
			AdminNavigationBuilder.CreatePartialClass();
			//-------------------------------------
			//SiteTemplateXmlBuilder.CreateTemplateFile();
			AppTemplateBuilder.CreateAppTemlatesFile();
			//-------------------------------------
            CreateSLN(ProjectVersions.V2005);
            CreateSLN(ProjectVersions.V2008);
            WebConfigBuilder.Create();
			//VirtualDirectoryBuilder.Create();
		}
		//-------------------------------------------
		public static void CreateThetemplateFiles()
		{
			if (!Directory.Exists(Globals.BaseDirectory))
			{
				DirectoriesManager.Copy("ProjectFiles/SimpleProjectBuilderSrc", Globals.BaseDirectory, true);
			}
		}
		//-------------------------------------------
        private static void CreateSLN(ProjectVersions version)
		{
			//-----------------------------------
            string slnFile = "ProjectFiles/ProjectBuilder" + version + ".sln";
            
			StreamReader _reader			= null;
			
			string lineOfText;
			StringBuilder sb = new StringBuilder();
			if( false == System.IO.File.Exists( slnFile )) 
			{
				throw new Exception("File " + slnFile + " does not exists");
			}
			using( Stream stream = System.IO.File.OpenRead( slnFile ) ) 
			{
				_reader = new StreamReader( stream );
				while(true) 
				{
					lineOfText = _reader.ReadLine();
					if( lineOfText == null ) 
					{
						string _class=sb.ToString();
						_class = _class.Replace("{0}", ProjectPort);
                        _class = _class.Replace("{1}", PhysicalPath);
//						_class=_class.Replace("{1}",Guid.NewGuid().ToString());
//						_class=_class.Replace("{2}",Guid.NewGuid().ToString());
						//-----------------------------------
                        string path = Globals.BaseDirectory + ProjectName +  version + ".sln";
						// Create a file to write to.
						using (StreamWriter sw = File.CreateText(path)) 
						{
							sw.WriteLine(_class);				
						}    
						return;
						//-----------------------------------
					}
					else
						sb.Append(lineOfText + Environment.NewLine);
				}
				
				
			}
			//-----------------------------------
		}
		//----------------------------------
		private static void CreateWebinfo()
		{
			//-----------------------------------
			string webinfo = "ProjectFiles/ProjectBuilder.csproj.webinfo";
			StreamReader _reader			= null;
			
			string lineOfText;
			StringBuilder sb = new StringBuilder();
			if( false == System.IO.File.Exists( webinfo )) 
			{
				throw new Exception("File " + webinfo + " does not exists");
			}
			using( Stream stream = System.IO.File.OpenRead( webinfo ) ) 
			{
				_reader = new StreamReader( stream );
				while(true) 
				{
					lineOfText = _reader.ReadLine();
					if( lineOfText == null ) 
					{
						string _class=sb.ToString();
						_class=_class.Replace("{0}",ProjectName);
						//						_class=_class.Replace("{1}",Guid.NewGuid().ToString());
						//						_class=_class.Replace("{2}",Guid.NewGuid().ToString());
						//-----------------------------------
						string path = Globals.BaseDirectory+ProjectName+".csproj.webinfo";
						// Create a file to write to.
						using (StreamWriter sw = File.CreateText(path)) 
						{
							
							sw.WriteLine(_class);				
						}    
						return;
						//-----------------------------------
					}
					else
						sb.Append(lineOfText + Environment.NewLine);
				}
				
				
			}
			//-----------------------------------
		}
	}
}
