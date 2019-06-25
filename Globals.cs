using System;
using System.Text;
using System.Data;
using System.IO;
namespace SPGen
{
	/// <summary>
	/// Summary description for Globals.
	/// </summary>
	public class Globals
	{
        public static string MetthodsSeparator = "//------------------------------------------";
        public string SqlDataProviderClass;
        public string SqlDataProviderObject;
		//public string DataProviderClass="CommonDataPrvider";
		public string TableEntityClass;
		private string entityClassObject=null;
        private string entityClassList = null;
        public string EntityClassObject
		{
			get
			{
				if(entityClassObject==null)
					entityClassObject=Globals.ConvetStringToCamelCase(TableProgramatlyName);
				return entityClassObject;
			}
		}
        public string EntityClassList
        {
            get
            {
                if (entityClassList == null)
                    entityClassList = Globals.ConvetStringToCamelCase(TableProgramatlyName) + "List";
                return entityClassList;
            }
        }
		public string TableSqlName="";
		public string TableProgramatlyName="";
		//-------------------------------
		public string TableFactoryClass="";
		public string UpdateCodeBehindClass="";
		public string CreateCodeBehindClass="";
        public string GetAllCodeBehindClass = "";
        public string GetAllWithSortingCodeBehindClass = "";
		//
		public string HeaderTitle_Create;
		public string HeaderTitle_Update;
		public string HeaderTitle_GetAll;
		//--------------------------
		public string ViewAllDataGrid;
		public string ClearControlsMethod="ClearControls()";
		//
		public string PopulateMethodName;
		//statics
		public string SecuritySQLFile = "ProjectFiles/SecurityDB.sql";
		//
		public string ProjectEncoding="windows-1256";
		public Globals()
		{
			TableSqlName=SqlProvider.obj.TableName;
			TableProgramatlyName=GetProgramatlyName(TableSqlName);
			//
            TableEntityClass = TableProgramatlyName + "Entity";
			//
			//DataProviderClass=TableProgramatlyName+"DataPrvider";this module was canceled
			SqlDataProviderClass=TableProgramatlyName+"SqlDataPrvider";
            SqlDataProviderObject = ConvetStringToCamelCase(SqlDataProviderClass);
			TableFactoryClass=TableProgramatlyName+"Factory";
			//
			CreateCodeBehindClass=TableProgramatlyName+"_Create";
			UpdateCodeBehindClass=TableProgramatlyName+"_Update";
			GetAllCodeBehindClass=TableProgramatlyName+"_GetAll";
            GetAllWithSortingCodeBehindClass = TableProgramatlyName + "_GetAll2";
            //
			HeaderTitle_Create="Create" + TableSqlName;
			HeaderTitle_GetAll="All"+TableSqlName;
			HeaderTitle_Update="Update" + TableSqlName;
			//
			ViewAllDataGrid="dg"+TableProgramatlyName;
			PopulateMethodName="PopulateEntity";
			
		}
		public static string ConvetStringToCamelCase(string stringRow)
		{
			string stringInCamelCase = "";
			if (stringRow.ToUpper() == "ID")
				return stringRow.ToLower();
			stringRow = stringRow.Trim();
			char[] stringRowChars = stringRow.ToCharArray();
			stringRowChars[0] = Convert.ToChar(stringRowChars[0].ToString().ToLower());

			for (int i = 0; i < stringRowChars.Length; i++)
			{
				stringInCamelCase += stringRowChars[i];
			}
			return stringInCamelCase;
		}
        private static string _App_DataDirectory = BaseDirectory + "App_Data\\";
        public static string App_DataDirectory
        {
            get
            {
                if (!Directory.Exists(_App_DataDirectory))
                    Directory.CreateDirectory(_App_DataDirectory);
                return _App_DataDirectory;

            }
        }
		#region Generated Directories
		private static string _BaseDirectory=null;
		public static string BaseDirectory
		{
			get
			{
				if(_BaseDirectory==null)
					_BaseDirectory=ProjectBuilder.PhysicalPath+"\\"+ProjectBuilder.ProjectName+"\\";
				//				if(!Directory.Exists("GeneratedCode\\ProjectBuilder\\"))
				//					Directory.CreateDirectory(_BaseDirectory);
				return _BaseDirectory;

			}
		}
		//
		private static string _ClassesDirectory=BaseDirectory+"App_Code\\Modules\\";
		public static string ClassesDirectory
		{
			get
			{
				if(!Directory.Exists(_ClassesDirectory))
					Directory.CreateDirectory(_ClassesDirectory);
				return _ClassesDirectory;

			}
		}
		//
		private static string _ComponentsClassesDirectory = BaseDirectory + "App_Code\\Components\\";
		public static string ComponentsClassesDirectory
		{
			get
			{
				return _ComponentsClassesDirectory;

			}
		}
		//
        private static string _ConfigFilesDirectory = BaseDirectory + "App_GlobalResources\\App_Configs\\";
		public static string ConfigFilesDirectory
		{
			get
			{
				return _ConfigFilesDirectory;

			}
		}
        //
        private static string _SiteOptionsDirectory = BaseDirectory + "App_GlobalResources\\SiteOptions\\";
        public static string SiteOptionsDirectory
        {
            get
            {
                return _SiteOptionsDirectory;

            }
        }
        //
        private static string _SiteSettingsResxDirectory = BaseDirectory + "App_GlobalResources\\SiteSettings\\";
        public static string SiteSettingsResxDirectory
        {
            get
            {
                return _SiteSettingsResxDirectory;

            }
        }
        //
        private static string _SiteSettingsClassesDirectory = BaseDirectory + "App_Code\\SiteSettings\\";
        public static string SiteSettingsClassesDirectory
        {
            get
            {
                return _SiteSettingsClassesDirectory;

            }
        }

        private static string _SiteUrlsResxDirectory = BaseDirectory + "App_GlobalResources\\SiteUrls\\";
        public static string SiteUrlsResxDirectory
        {
            get
            {
                return _SiteUrlsResxDirectory;

            }
        }
        //
        /*private static string _SiteUrlsClassesDirectory = BaseDirectory + "App_Code\\Modules\\";
        public static string SiteUrlsClassesDirectory
        {
            get
            {
                return _SiteUrlsClassesDirectory;

            }
        }*/
        //
        private static string _GlobalResources = BaseDirectory + "App_GlobalResources\\";
        public static string GlobalResources
        {
            get
            {
                return _GlobalResources;

            }
        }
        private static string _ModulesGlobalResources = GlobalResources + "Modules\\";
        public static string ModulesGlobalResources
        {
            get
            {
                return _ModulesGlobalResources;

            }
        }
		private static string _AdminLanguagesDirectory = GlobalResources + "\\Languages\\Admin\\";
		public static string AdminLanguagesDirectory
		{
			get
			{
				return _AdminLanguagesDirectory;

			}
		}
		//
		private static string _UserLanguagesDirectory = BaseDirectory + "App_GlobalResources\\Languages\\User\\";
		public static string UserLanguagesDirectory
		{
			get
			{
				return _UserLanguagesDirectory;

			}
		}
		//
        private static string _AdminNavigationCodeBehind = BaseDirectory + "App_Design\\Admin\\UserControls\\";
		public static string AdminNavigationCodeBehind
		{
			get
			{
				return _AdminNavigationCodeBehind;

			}
		}
		//
		
		//
        private static string _SiteTemplates = BaseDirectory + "App_Design\\User\\App_Templates\\";
		public static string SiteTemplates
		{
			get
			{
				return _SiteTemplates;

			}
		}
		//
		private static string _SiteForms = BaseDirectory + "App_Forms\\";
		public static string SiteForms
		{
			get
			{
				if (!Directory.Exists(_SiteForms))
					Directory.CreateDirectory(_SiteForms);
				return _SiteForms;

			}
		}
		//
		private static string _UserControlsDirectory=BaseDirectory+"UserControls\\";
		public static string UserControlsDirectory
		{
			get
			{
				if(!Directory.Exists(_UserControlsDirectory))
					Directory.CreateDirectory(_UserControlsDirectory);
				return _UserControlsDirectory;

			}
		}
		//
		private static string _AdminFolder=BaseDirectory+"Admin\\";
		public static string AdminFolder
		{
			get
			{
				if(_AdminFolder==null)
					_AdminFolder=BaseDirectory+"Admin\\";
				if(!Directory.Exists(_AdminFolder))
					Directory.CreateDirectory(_AdminFolder);
				return _AdminFolder;

			}
		}
		//
		public static string AdminWebPath
		{
			get
			{
				return ProjectBuilder.NameSpace+"/Admin/";
			}
		}
		#endregion
		
		

		//------------------------------------------------------------
		public static bool CheckIsAddedBySql(SQLDMO.Column  clmn)
		{
			if(clmn.Identity&&clmn.IdentityIncrement>0)
				return true;
			else 
				return false;
		}
		public static string GetProcedureName(StoredProcedureTypes spType)
		{
			return "";
		}
		//------------------------------------------------------------
		public static SqlDbType GetSqlDataType(string type)
		{
			if(type.ToLower()== SqlDbType.BigInt.ToString().ToLower())
				return SqlDbType.BigInt;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Binary.ToString().ToLower())
				return SqlDbType.Binary;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Bit.ToString().ToLower())
				return SqlDbType.Bit;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Char.ToString().ToLower())
				return SqlDbType.Char;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.DateTime.ToString().ToLower())
				return SqlDbType.DateTime;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Decimal.ToString().ToLower())
				return SqlDbType.Decimal;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Float.ToString().ToLower())
				return SqlDbType.Float;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Image.ToString().ToLower())
				return SqlDbType.Image;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Int.ToString().ToLower())
				return SqlDbType.Int;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Money.ToString().ToLower())
				return SqlDbType.Money;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.NChar.ToString().ToLower())
				return SqlDbType.NChar;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.NText.ToString().ToLower())
				return SqlDbType.NText;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.NVarChar.ToString().ToLower())
				return SqlDbType.NVarChar;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Real.ToString().ToLower())
				return SqlDbType.Real;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.SmallDateTime.ToString().ToLower())
				return SqlDbType.SmallDateTime;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.SmallInt.ToString().ToLower())
				return SqlDbType.SmallInt;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.SmallMoney.ToString().ToLower())
				return SqlDbType.SmallMoney;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Text.ToString().ToLower())
				return SqlDbType.Text;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Timestamp.ToString().ToLower())
				return SqlDbType.Timestamp;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.TinyInt.ToString().ToLower())
				return SqlDbType.TinyInt;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.UniqueIdentifier.ToString().ToLower())
				return SqlDbType.UniqueIdentifier;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.VarBinary.ToString().ToLower())
				return SqlDbType.VarBinary;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.VarChar.ToString().ToLower())
				return SqlDbType.VarChar;
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Variant.ToString().ToLower())
				return SqlDbType.Variant;
				//-----------------------------------
			else
				return SqlDbType.NVarChar;//????????
		}
		//------------------------------------------------------------
        public static string GetDataType(string type)
        {
            if (type.ToLower() == SqlDbType.BigInt.ToString().ToLower())
                return "Int64";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.Binary.ToString().ToLower())
                return "Byte[]";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.Bit.ToString().ToLower())
                return "Boolean";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.Char.ToString().ToLower())
                return "String";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.DateTime.ToString().ToLower())
                return "DateTime";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.Decimal.ToString().ToLower())
                return "Decimal";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.Float.ToString().ToLower())
                return "Double";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.Image.ToString().ToLower())
                return "Byte[]";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.Int.ToString().ToLower())
                return "Int32";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.Money.ToString().ToLower())
                return "Decimal";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.NChar.ToString().ToLower())
                return "String";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.NText.ToString().ToLower())
                return "String";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.NVarChar.ToString().ToLower())
                return "String";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.Real.ToString().ToLower())
                return "Single";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.SmallDateTime.ToString().ToLower())
                return "DateTime";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.SmallInt.ToString().ToLower())
                return "Int16";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.SmallMoney.ToString().ToLower())
                return "Decimal";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.Text.ToString().ToLower())
                return "String";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.Timestamp.ToString().ToLower())
                return "Byte[]";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.TinyInt.ToString().ToLower())
                return "Byte";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.UniqueIdentifier.ToString().ToLower())
                return "Guid";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.VarBinary.ToString().ToLower())
                return "Byte[]";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.VarChar.ToString().ToLower())
                return "String";
            //-----------------------------------
            else if (type.ToLower() == SqlDbType.Variant.ToString().ToLower())
                return "Object";
            //-----------------------------------
            else
                return "";
        }
        //-------------------------------------------------------------------
        public static string GetAliasDataType(string type)
		{
			if(type.ToLower()== SqlDbType.BigInt.ToString().ToLower())
				return "long";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Binary.ToString().ToLower())
				return "byte[]";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Bit.ToString().ToLower())
				return "bool";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Char.ToString().ToLower())
				return "string";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.DateTime.ToString().ToLower())
				return "DateTime";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Decimal.ToString().ToLower())
				return "decimal";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Float.ToString().ToLower())
				return "double";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Image.ToString().ToLower())
				return "byte[]";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Int.ToString().ToLower())
				return "int";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Money.ToString().ToLower())
				return "decimal";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.NChar.ToString().ToLower())
				return "string";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.NText.ToString().ToLower())
				return "string";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.NVarChar.ToString().ToLower())
				return "string";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Real.ToString().ToLower())
				return "Single";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.SmallDateTime.ToString().ToLower())
				return "DateTime";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.SmallInt.ToString().ToLower())
				return "short";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.SmallMoney.ToString().ToLower())
				return "decimal";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Text.ToString().ToLower())
				return "string";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Timestamp.ToString().ToLower())
				return "byte[]";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.TinyInt.ToString().ToLower())
				return "byte";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.UniqueIdentifier.ToString().ToLower())
				return "Guid";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.VarBinary.ToString().ToLower())
				return "byte[]";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.VarChar.ToString().ToLower())
				return "string";
				//-----------------------------------
			else if(type.ToLower()== SqlDbType.Variant.ToString().ToLower())
				return "Object";
				//-----------------------------------
			else
				return "";
		}
		//------------------------------------------------------------
		public static string GetProgramatlyName(string name)
		{
			return name.Replace(" ","_");
		}

		//------------------------------------------------------------
		public static int GetTextBoxMaxLength(SQLDMO.Column column)
		{
			if(column.Datatype.ToLower()== SqlDbType.BigInt.ToString().ToLower())
				return 20;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.Binary.ToString().ToLower())
				return 0;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.Bit.ToString().ToLower())
				return 1;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.Char.ToString().ToLower())
				return column.Length ;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.DateTime.ToString().ToLower())
				return 0;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.Decimal.ToString().ToLower())
				return 40;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.Float.ToString().ToLower())
				return 0;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.Image.ToString().ToLower())
				return 0;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.Int.ToString().ToLower())
				return 9;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.Money.ToString().ToLower())
				return 20;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.NChar.ToString().ToLower())
				return column.Length ;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.NText.ToString().ToLower())
				return 0;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.NVarChar.ToString().ToLower())
				return column.Length ;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.Real.ToString().ToLower())
				return 0;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.SmallDateTime.ToString().ToLower())
				return 0;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.SmallInt.ToString().ToLower())
				return 6;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.SmallMoney.ToString().ToLower())
				return 0;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.Text.ToString().ToLower())
				return 0;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.Timestamp.ToString().ToLower())
				return 0;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.TinyInt.ToString().ToLower())
				return 3;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.UniqueIdentifier.ToString().ToLower())
				return 0;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.VarBinary.ToString().ToLower())
				return 0;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.VarChar.ToString().ToLower())
				return  column.Length ;
				//-----------------------------------
			else if(column.Datatype.ToLower()== SqlDbType.Variant.ToString().ToLower())
				return 0;
			else
				return 0;
		}
		//--------------------------
		public static string CreatePageName
		{
			get
			{
				Globals global =new Globals ();
				return global.TableProgramatlyName+"_Create.aspx";
			} 
		}
		//--------------------------
		public static string UpdatePageName
		{
			get
			{
				Globals global =new Globals ();
				return global.TableProgramatlyName+"_Update.aspx";
			} 
		}
		//--------------------------
		public static string GetAllPageName
		{
			get
			{
				Globals global =new Globals ();
				return global.TableProgramatlyName+"_GetAll.aspx";
			} 
		}
	}
	
}
	

		

