using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;
namespace SPGen
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class ClassFactoryBuilder:Generator 
	{
		public ClassFactoryBuilder()
		{
			ClassName=global.TableFactoryClass;			
    	}
        //----------------------------------------------------------
        private string GeneateUsingBlock()
        {
            string Usingblock = "";
            Usingblock += "using System;\n";
            Usingblock += "using System.Collections;\n";
            Usingblock += "using System.Collections.Generic;\n";
            Usingblock += "using System.Data;\n";
			Usingblock += "using System.Web;\n";
			Usingblock += "using System.Web.Caching;\n";
            return Usingblock;
        }
        //----------------------------------------------------------
        private string GenerateClassBody()
        {
            StringBuilder MethodsBuilder = new StringBuilder();
			MethodsBuilder.Append("\n" + CreateGetChacheKeyMethod());
            MethodsBuilder.Append("\n" + CreateCreateMethod());
            MethodsBuilder.Append("\n" + CreateUpdateMethod());
			MethodsBuilder.Append("\n" + CreateDeleteMethod());
            bool hasIsAvailable = SqlProvider.CheckIsATableHasIsAvailableColumnName(SqlProvider.obj.TableName);
            bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(SqlProvider.obj.TableName));
            //CreateGetAllMethod
           
            //----------------------
            //CreateGetAllMethod
			MethodsBuilder.Append("\n" + CreateGetAllMethod());
            //----------------------
            //CreateGetCountMethod
            MethodsBuilder.Append("\n" + CreateGetCountMethod());
            //----------------------
            //GenerateReletionalGetData
            MethodsBuilder.Append(GenerateReletionalGetData());
            
            if (ID != null)
                MethodsBuilder.Append("\n" + CreateGetOneMethod());
			//MethodsBuilder.Append("\n" + CreatePopulateObjectMethod());
            return MethodsBuilder.ToString();
        }
        //----------------------------------------------------------
        private string GenerateClass(string classBody)
        {
            string xmlDocumentation = "\n/// <summary>\n";
            xmlDocumentation += "/// The class factory of " + SqlProvider.obj.TableName +".\n";
            xmlDocumentation += "/// </summary>\n";
            string _class = "";
            _class += GeneateUsingBlock();
            if (ProjectBuilder.AllowXmlDocumentation)
                _class += xmlDocumentation;
            _class += "public class " + ClassName;
            _class += "\n{\n" + classBody + "\n}";
            return _class;
        }
        //----------------------------------------------------------
        public static void Create()
        {
            ClassFactoryBuilder dp = new ClassFactoryBuilder();
            dp.CreateClassFile();
        }
        //----------------------------------------------------------
        public void CreateClassFile()
        {
            try
            {
                string _class = GenerateClass(GenerateClassBody());
                DirectoryInfo dInfo = Directory.CreateDirectory(Globals.ClassesDirectory + global.TableProgramatlyName);
                string path = dInfo.FullName + "\\" + ClassName + ".cs";
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {

                    sw.WriteLine(_class);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);
            }
        }
        //----------------------------------------------------------
        #region ClassMember
        //----------------------------------------------------------
        #region CreateCreateMethod
        public string CreateCreateMethod()
		{
			try
			{
				string id = Globals.GetProgramatlyName(ID.Name);
				id = Globals.ConvetStringToCamelCase(id);
				string MethodName = StoredProcedureTypes.Create.ToString();
				string sqlDataProviderMethodName = StoredProcedureTypes.Create.ToString();
				string MethodParameters = "(" + global.TableEntityClass + " " + global.EntityClassObject + ")";
                string MethodReturn = "bool";
                if (ProjectBuilder.ISExcuteScaler)
                    MethodReturn = "ExecuteCommandStatus";
                //
				MethodParameters = "(" + global.TableEntityClass + " " + global.EntityClassObject + ")";
				//XML Documentaion
				string xmlInsertDocumentation = "\n\t/// <summary>\n";
				xmlInsertDocumentation += "\t/// Creates " + SqlProvider.obj.TableName + " object by calling " + SqlProvider.obj.TableName + " data provider create method.\n";
                xmlInsertDocumentation += "\t/// <example>[Example]" + MethodReturn + " status=" + ClassName + "." + MethodName + "(" + global.EntityClassObject + ");.</example>\n";
				xmlInsertDocumentation += "\t/// </summary>\n";
				xmlInsertDocumentation += "\t/// <param name=\"" + global.EntityClassObject + "\">The " + SqlProvider.obj.TableName + " object.</param>\n";
				xmlInsertDocumentation += "\t/// <returns>Status of create operation.</returns>";
				//
				//method body
				StringBuilder methodBody = new StringBuilder();
				methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
				methodBody.Append(xmlInsertDocumentation);
                methodBody.Append("\n\tpublic static " + MethodReturn + " " + MethodName + MethodParameters);
				methodBody.Append("\n\t{");
				//
				methodBody.Append("\n\t\treturn " + global.SqlDataProviderClass + ".Instance." + sqlDataProviderMethodName + "(" + global.EntityClassObject + ");");
				methodBody.Append("\n\t}");
				methodBody.Append("\n\t" + Globals.MetthodsSeparator);
				methodBody.Append("\n\t#endregion");
				return methodBody.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);
				return "";
			}



        }
        #endregion
        //----------------------------------------------------------
        #region CreateUpdateMethod
        public string CreateUpdateMethod()
		{
			try
			{
				string id = Globals.GetProgramatlyName(ID.Name);
				id = Globals.ConvetStringToCamelCase(id);
				string MethodName = StoredProcedureTypes.Update.ToString();
				string sqlDataProviderMethodName = StoredProcedureTypes.Update.ToString();
				string MethodParameters = "(" + global.TableEntityClass + " " + global.EntityClassObject + ")";
                string MethodReturn = "bool";
                if (ProjectBuilder.ISExcuteScaler)
                    MethodReturn = "ExecuteCommandStatus";
				//XML Documentaion
				string xmlUpdateDocumentation = "\n\t/// <summary>\n";
				xmlUpdateDocumentation += "\t/// Updates " + SqlProvider.obj.TableName + " object by calling " + SqlProvider.obj.TableName + " data provider update method.\n";
                xmlUpdateDocumentation += "\t/// <example>[Example]" + MethodReturn + " status=" + ClassName + "." + MethodName + "(" + global.EntityClassObject + ");.</example>\n";
				xmlUpdateDocumentation += "\t/// </summary>\n";
				xmlUpdateDocumentation += "\t/// <param name=\"" + global.EntityClassObject + "\">The " + SqlProvider.obj.TableName + " object.</param>\n";
				xmlUpdateDocumentation += "\t/// <returns>Status of update operation.</returns>";
				//
				//method body
				StringBuilder methodBody = new StringBuilder();
				methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
				methodBody.Append(xmlUpdateDocumentation);
                methodBody.Append("\n\tpublic static " + MethodReturn + " " + MethodName + MethodParameters);
				methodBody.Append("\n\t{");
                if (!ProjectBuilder.ISExcuteScaler)
                {
                    methodBody.Append("\n\t\tbool status =" + global.SqlDataProviderClass + ".Instance." + sqlDataProviderMethodName + "(" + global.EntityClassObject + ");");
                    methodBody.Append("\n\t\tif (status)");
                }
                else
                {
                    methodBody.Append("\n\t\tExecuteCommandStatus status =" + global.SqlDataProviderClass + ".Instance." + sqlDataProviderMethodName + "(" + global.EntityClassObject + ");");
                    methodBody.Append("\n\t\tif (status == ExecuteCommandStatus.Done)");
                }
				methodBody.Append("\n\t\t{");
				methodBody.Append("\n\t\t\tstring cacheKey = GetChacheKey("+global.EntityClassObject+"."+Globals.GetProgramatlyName(ID.Name)+");");
				
				
				methodBody.Append("\n\t\t\tOurCache.Remove(cacheKey);");
				
				methodBody.Append("\n\t\t}");
				methodBody.Append("\n\t\treturn status;");
				methodBody.Append("\n\t}");
				methodBody.Append("\n\t" + Globals.MetthodsSeparator);
				methodBody.Append("\n\t#endregion");
				return methodBody.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);
				return "";
			}



		}
		//-------------------------------------
        #endregion
        //----------------------------------------------------------
        #region CreateDeleteMethod
        public string CreateDeleteMethod()
		{
			try
			{
				string id = Globals.GetProgramatlyName(ID.Name);
				id = Globals.ConvetStringToCamelCase(id);
				string MethodName = StoredProcedureTypes.Delete.ToString();
				string sqlDataProviderMethodName = StoredProcedureTypes.Delete.ToString();
				string MethodParameters = "(" + Globals.GetAliasDataType(ID.Datatype) + " " + id + ")";
				//XML Documentaion
				string xmlDeleteDocumentation = "\n\t/// <summary>\n";
				xmlDeleteDocumentation += "\t/// Deletes single " + SqlProvider.obj.TableName + " object .\n";
                xmlDeleteDocumentation += "\t/// <example>[Example]bool status=" + ClassName + "." + MethodName + "(" + id + ");.</example>\n";
				xmlDeleteDocumentation += "\t/// </summary>\n";
				xmlDeleteDocumentation += "\t/// <param name=\"" + id + "\">The " + global.EntityClassObject + " id.</param>\n";
				xmlDeleteDocumentation += "\t/// <returns>Status of delete operation.</returns>";
				//method body
				StringBuilder methodBody = new StringBuilder();
				methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
				methodBody.Append(xmlDeleteDocumentation);
				methodBody.Append("\n\tpublic static bool " + MethodName + MethodParameters);
				methodBody.Append("\n\t{");
				//
				methodBody.Append("\n\t\tbool status =" + global.SqlDataProviderClass + ".Instance." + sqlDataProviderMethodName + "(" + id + ");");
				methodBody.Append("\n\t\tif (status)");
				methodBody.Append("\n\t\t{");
				methodBody.Append("\n\t\t\tstring cacheKey = GetChacheKey(" + id + ");");
                methodBody.Append("\n\t\t\tOurCache.Remove(cacheKey);");
				methodBody.Append("\n\t\t}");
				methodBody.Append("\n\t\treturn status;");
				//
				methodBody.Append("\n\t}");
				methodBody.Append("\n\t" + Globals.MetthodsSeparator);
				methodBody.Append("\n\t#endregion");
				return methodBody.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);
				return "";
			}



		}
        //----------------------------------------------------------
        #endregion
        //----------------------------------------------------------
        #region GenerateReletionalGetData
        private string GenerateReletionalGetData()
        {
            StringBuilder relationalMethods = new StringBuilder();
            bool hasIsAvailable = SqlProvider.CheckIsATableHasIsAvailableColumnName(SqlProvider.obj.TableName);
            foreach (SQLDMO.Column column in Fields)
            {
                //if ((ID == null || column.Name != ID.Name) && (column.Default == null || column.Default.Length != 0) && column.Name.ToLower() != ProjectBuilder.LangID)
                if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0))
                {
                    TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
                    if (cnstr != null)
                    {
                       
                        relationalMethods.Append("\n" + CreateGetAllMethod(column, false));
                       
                       
                        relationalMethods.Append("\n" + CreateGetCountMethod(column, false));
                       
                    }
                }
            }
            return relationalMethods.ToString();


        }
        //----------------------------------------------------------
        #endregion
        //----------------------------------------------------------
        #region CreateGetCountMethod
        public string CreateGetCountMethod()
        {
            return CreateGetCountMethod(null, false);
        }
        //----------------------------------------------------------
        public string CreateGetCountMethod(SQLDMO.Column conditionalColumn, bool isAvailableMethod)
        {
            try
            {
                string MethodName = StoredProcedureTypes.GetCount.ToString();
                string sqlDataProviderMethodName2 = StoredProcedureTypes.GetCount.ToString() + global.TableProgramatlyName;
                string sqlDataProviderMethodCaller = global.SqlDataProviderClass + ".Instance." + sqlDataProviderMethodName2;
                string methodParameters = "";
                string sqlMethodParameters = "";
                string additionalPreviousBodyLines = "";
                //------------------------------
                bool hasPreviousMethodParameter = false;
                bool hasPreviousSqlMethodParameter = false;
                //------------------------------
                bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(SqlProvider.obj.TableName));
                if (isMaultiLanguages)
                {
                    /*if (userType == UserType.Anonymous)
                    {
                        methodParameters += "Languages langID";
                        hasPreviousMethodParameter = true;                    
                    }*/
                    sqlMethodParameters += "langID";
                    hasPreviousSqlMethodParameter = true;

                }
                //------------------------------
                if (conditionalColumn != null)
                {
                    //Check Previous Parameters
                    if (hasPreviousMethodParameter)
                        methodParameters += ", ";
                    if(hasPreviousSqlMethodParameter)
                        sqlMethodParameters += ", ";
                    //-----------------------
                    MethodName += "By" + Globals.GetProgramatlyName(conditionalColumn.Name);
                    sqlDataProviderMethodCaller += "By" + Globals.GetProgramatlyName(conditionalColumn.Name);
                    methodParameters += Globals.GetAliasDataType(conditionalColumn.Datatype) + " " + Globals.ConvetStringToCamelCase(Globals.GetProgramatlyName(conditionalColumn.Name));
                    sqlMethodParameters += Globals.ConvetStringToCamelCase(Globals.GetProgramatlyName(conditionalColumn.Name));
                    hasPreviousMethodParameter = true;
                    hasPreviousSqlMethodParameter = true;
                }
                //------------------------------
                bool hasIsAvailable = SqlProvider.CheckIsATableHasIsAvailableColumnName(SqlProvider.obj.TableName);
                if (hasIsAvailable)
                {
                    //-------------------------
                    if (!isAvailableMethod)
                    {
                        //Check Previous Parameters
                        if (hasPreviousMethodParameter)
                            methodParameters += ", ";
                        if (hasPreviousSqlMethodParameter)
                            sqlMethodParameters += ", ";
                        //-------------------------
                        methodParameters += "bool " + Globals.ConvetStringToCamelCase(ProjectBuilder.IsAvailableConditionParam);
                        sqlMethodParameters += Globals.ConvetStringToCamelCase(ProjectBuilder.IsAvailableConditionParam);
                    }
                    else
                    {
                        //Check Previous Parameter
                        if (hasPreviousSqlMethodParameter)
                        {
                            sqlMethodParameters += ", ";
                        }
                        //--------------------------
                        sqlDataProviderMethodCaller = MethodName;
                        /*if (userType == UserType.Admin)
                        {
                            sqlMethodParameters += "false";
                        }
                        else
                        {
                            sqlMethodParameters += "true";
                        }*/
                    }
                    hasPreviousMethodParameter = true;

                }
                
                //------------------------------
                //XXXXXXXXXXXXXXX
                string MethodReturn = "int";
                //XML Documentaion
                string xmlDocumentation = "\n\t/// <summary>\n";
                xmlDocumentation += "\t/// Gets All " + SqlProvider.obj.TableName + ".\n";
                xmlDocumentation += "\t/// <example>[Example]int itemsCount" + global.TableProgramatlyName + "=" + ClassName + "." + MethodName + "();.</example>\n";
                xmlDocumentation += "\t/// </summary>\n";
                xmlDocumentation += "\t/// <returns>All " + SqlProvider.obj.TableName + ".</returns>";
                //Method Body
                StringBuilder methodBody = new StringBuilder();
                methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
                if (ProjectBuilder.AllowXmlDocumentation)
                    methodBody.Append(xmlDocumentation); methodBody.Append("\n\tpublic static " + MethodReturn + " " + MethodName + "(" + methodParameters + ")");
                methodBody.Append("\n\t{");
                methodBody.Append("\n\t\t" + additionalPreviousBodyLines );
                methodBody.Append("\n\t\treturn " + sqlDataProviderMethodCaller + "(" + sqlMethodParameters + ");");
                methodBody.Append("\n\t}");
                methodBody.Append("\n\t" + Globals.MetthodsSeparator);
                methodBody.Append("\n\t#endregion");
                return methodBody.ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show("My Generated Code Exception:" + ex.Message);
                return "";

            }

        }
        //-------------------------------------
        #endregion
        //----------------------------------------------------------
        
        //----------------------------------------------------------
        #region CreateGetAllMethod
        public string CreateGetAllMethod()
        {
            return CreateGetAllMethod(null, false);
        }
        //-------------------------------------
        public string CreateGetAllMethod(SQLDMO.Column conditionalColumn,bool isAvailableMethod)
		{
			try
            {
				string MethodName = StoredProcedureTypes.GetAll.ToString();
                string sqlDataProviderMethodName2 = "GetAll";
                string sqlDataProviderMethodCaller=global.SqlDataProviderClass + ".Instance." + sqlDataProviderMethodName2;
                string methodParameters = "";
                string sqlMethodParameters = "";
                string additionalPreviousBodyLines = "";
                //------------------------------
                bool hasPreviousMethodParameter = false;
                bool hasPreviousSqlMethodParameter = false;
                //------------------------------
                bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(SqlProvider.obj.TableName));
                if (isMaultiLanguages)
                {
                    if (!isAvailableMethod)
                    {
                        methodParameters += "Languages langID";
                        hasPreviousMethodParameter = true;
                    }
                    sqlMethodParameters += "langID";
                    hasPreviousSqlMethodParameter = true;
                }
                //------------------------------
                if (conditionalColumn != null)
                {
                    //Check Previous Parameters
                    if (hasPreviousMethodParameter)
                        methodParameters += ", ";
                    if (hasPreviousSqlMethodParameter)
                        sqlMethodParameters += ", ";
                    //-----------------------
                    MethodName += "By" + Globals.GetProgramatlyName(conditionalColumn.Name);
                    sqlDataProviderMethodCaller += "By" + Globals.GetProgramatlyName(conditionalColumn.Name);
                    methodParameters += Globals.GetAliasDataType(conditionalColumn.Datatype) + " " + Globals.ConvetStringToCamelCase(Globals.GetProgramatlyName(conditionalColumn.Name)) ;
                    sqlMethodParameters += Globals.ConvetStringToCamelCase(Globals.GetProgramatlyName(conditionalColumn.Name)) ;
                    //-----------------------
                    hasPreviousMethodParameter = true;
                    hasPreviousSqlMethodParameter = true;
                }
                //------------------------------
                bool hasIsAvailable = SqlProvider.CheckIsATableHasIsAvailableColumnName(SqlProvider.obj.TableName);
                if (hasIsAvailable)
                {
                    if (!isAvailableMethod)
                    {
                        //Check Previous Parameters
                        if (hasPreviousMethodParameter)
                            methodParameters += ", ";
                        if (hasPreviousSqlMethodParameter)
                            sqlMethodParameters += ", ";
                        //-------------------------
                        methodParameters += "bool " + Globals.ConvetStringToCamelCase(ProjectBuilder.IsAvailableConditionParam) ;
                        sqlMethodParameters += Globals.ConvetStringToCamelCase(ProjectBuilder.IsAvailableConditionParam) ;
                        //-----------------------
                        hasPreviousMethodParameter = true;
                        hasPreviousSqlMethodParameter = true;
                    }
                    else
                    {
                        //Check Previous Parameter
                        if (hasPreviousSqlMethodParameter)
                        {
                            sqlMethodParameters += ", ";
                        }
                        sqlDataProviderMethodCaller = MethodName;
                        //if (userType == UserType.Admin)
                        //{
                        //    MethodName += "ForAdmin";
                        //    sqlMethodParameters += "false";
                        //    additionalPreviousBodyLines = "Languages langID = (Languages) ResourceManager.GetCurrentLanguage();";
                        //}
                        //else
                        //{
                        //    MethodName += "ForUser";
                        //    sqlMethodParameters += "true";
                        //    additionalPreviousBodyLines = "Languages langID = (Languages) ResourceManager.GetCurrentLanguage();";
                        //}
                        ////-----------------------
                        hasPreviousSqlMethodParameter = true;
                    }
                }
                //------------------------------
                //Check Previous Parameters
                if (hasPreviousMethodParameter)
                    methodParameters += ", ";
                if (hasPreviousSqlMethodParameter)
                    sqlMethodParameters += ", ";
                //-------------------------
                methodParameters    += "int pageIndex, int pageSize, out int totalRecords";
                sqlMethodParameters += "pageIndex, pageSize, out totalRecords";
                //XXXXXXXXXXXXXXX
                //XXXXXXXXXXXXXXX
                string MethodReturn = "List<" + global.TableEntityClass + ">";
				//XML Documentaion
				string xmlDocumentation = "\n\t/// <summary>\n";
				xmlDocumentation += "\t/// Gets All " + SqlProvider.obj.TableName + ".\n";
                xmlDocumentation += "\t/// <example>[Example]" + MethodReturn + " " + global.EntityClassList + "=" + ClassName + "." + MethodName + "(" + sqlMethodParameters + ");.</example>\n";

				xmlDocumentation += "\t/// </summary>\n";
				xmlDocumentation += "\t/// <returns>All " + SqlProvider.obj.TableName + ".</returns>";
				//Method Body
				StringBuilder methodBody = new StringBuilder();
				methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
                if (ProjectBuilder.AllowXmlDocumentation)
                    methodBody.Append(xmlDocumentation);
                methodBody.Append("\n\tpublic static " + MethodReturn + " " + MethodName + "(" + methodParameters + ")");
                methodBody.Append("\n\t{");
                methodBody.Append("\n\t\t" + additionalPreviousBodyLines);
                methodBody.Append("\n\t\treturn " + sqlDataProviderMethodCaller + "(" + sqlMethodParameters + ");");
                methodBody.Append("\n\t}");
				methodBody.Append("\n\t" + Globals.MetthodsSeparator);
				methodBody.Append("\n\t#endregion");
				return methodBody.ToString();
			}
			catch (Exception ex)
			{

				MessageBox.Show("My Generated Code Exception:" + ex.Message);
				return "";

			}

		}
		//-------------------------------------
        #endregion
        //----------------------------------------------------------
        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        //----------------------------------------------------------
        
        //----------------------------------------------------------
        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        #region CreateGetOneMethod
        public string CreateGetOneMethod()
		{
			if (ID != null)
			{
				try
				{
					string id = Globals.GetProgramatlyName(ID.Name);
					id = Globals.ConvetStringToCamelCase(id);
					string factoryClass = global.TableFactoryClass;
					string entityClass = global.TableEntityClass;
					string entityObject = global.EntityClassObject;
					//------------------------------------------------
					string MethodName = "GetObject";
					string MethodReturn = global.TableEntityClass;
                    string methodParameters = id;
                    string sqlMethodParameters = id;
					//XML Documentaion
					string xmlDocumentation = "\n\t/// <summary>\n";
					xmlDocumentation += "\t/// Gets single " + SqlProvider.obj.TableName + " object .\n";
                    xmlDocumentation += "\t/// <example>[Example]" + MethodReturn + " " + global.EntityClassList + "=" + ClassName + "." + MethodName + "(" + sqlMethodParameters + ");.</example>\n";
                    xmlDocumentation += "\t/// </summary>\n";
					xmlDocumentation += "\t/// <param name=\"" + id + "\">The " + global.EntityClassObject + " id.</param>\n";
					xmlDocumentation += "\t/// <returns>" + SqlProvider.obj.TableName + " object.</returns>";
					//Method Body
					StringBuilder methodBody = new StringBuilder();
					methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
                    if (ProjectBuilder.AllowXmlDocumentation)
                        methodBody.Append(xmlDocumentation);
                    methodBody.Append("\n\tpublic static  " + MethodReturn + " " + MethodName + "(" + Globals.GetAliasDataType(ID.Datatype) + " " + methodParameters + ")");
					methodBody.Append("\n\t{");
					//------------
					methodBody.Append("\n\t\tstring cacheKey = GetChacheKey(" + id + ");");
					methodBody.Append("\n\t\t" + entityClass + " " + entityObject + ";");
					//methodBody.Append("\n\t\tCache contextCache = HttpContext.Current.Cache;");
					//methodBody.Append("\n\t\tCache applicationCache = HttpRuntime.Cache;");
                    methodBody.Append("\n\t\tobject cachedObject = OurCache.Get(cacheKey);");

					methodBody.Append("\n\t\t//Check is object cached into our cache");
					methodBody.Append("\n\t\tif (cachedObject == null)");
					methodBody.Append("\n\t\t{");
					//methodBody.Append("\n\t\t\t//Check is object cached into application cache");
                   // methodBody.Append("\n\t\t\tif (cachedObject == null)");
					//methodBody.Append("\n\t\t\t{");
					methodBody.Append("\n\t\t\t" + entityObject + " = " + global.SqlDataProviderClass + ".Instance." + MethodName + "(" + id + ");");
					methodBody.Append("\n\t\t\tif (" + entityObject + " == null)");
					methodBody.Append("\n\t\t\t\treturn null;");
					methodBody.Append("\n\t\t\t//Save object into application cache");
                    methodBody.Append("\n\t\t\tOurCache.Insert(cacheKey, " + entityObject + ", 3 * OurCache.MinuteFactor);");
                    /*
					methodBody.Append("\n\t\t\t\tapplicationCache[cacheKey] = " + entityObject + ";");
					methodBody.Append("\n\t\t\t\t//Save object into httpContext cache");
					methodBody.Append("\n\t\t\t\tcontextCache[cacheKey] = " + entityObject + ";");
					methodBody.Append("\n\t\t\t}");
					methodBody.Append("\n\t\t\telse");
					methodBody.Append("\n\t\t\t{");
					methodBody.Append("\n\t\t\t\t//get object from application cache");
					methodBody.Append("\n\t\t\t\t" + entityObject + " = (" + entityClass + ")applicationCache[cacheKey];");
					methodBody.Append("\n\t\t\t\t//Save object into httpContext cache");
					methodBody.Append("\n\t\t\t\tcontextCache[cacheKey] = " + entityObject + ";");
					methodBody.Append("\n\t\t\t}");*/
					methodBody.Append("\n\t\t}");
					methodBody.Append("\n\t\telse");
					methodBody.Append("\n\t\t{");
					methodBody.Append("\n\t\t\t//get object from httpContext cache");
                    methodBody.Append("\n\t\t\t" + entityObject + " = (" + entityClass + ")cachedObject;");
					methodBody.Append("\n\t\t}");
					methodBody.Append("\n\t\t//return the object");
					methodBody.Append("\n\t\treturn " + entityObject + ";");

					//------------

					methodBody.Append("\n\t}");
					methodBody.Append("\n\t" + Globals.MetthodsSeparator);
					methodBody.Append("\n\t#endregion");
					return methodBody.ToString();
				}
				catch (Exception ex)
				{
					MessageBox.Show("My Generated Code Exception:" + ex.Message);
					return "";
				}
			}
			else
			{
				return "";
			}
		}
		//-------------------------------------
        #endregion
        //----------------------------------------------------------
        #region CreateGetChacheKeyMethod
        public string CreateGetChacheKeyMethod()
		{
			if (ID != null)
			{
				try
				{
					string id = Globals.GetProgramatlyName(ID.Name);
					id = Globals.ConvetStringToCamelCase(id);
					//
					string MethodName = "GetChacheKey";
					string MethodReturn = "string";
					//Method Body
					StringBuilder methodBody = new StringBuilder();
					methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
					methodBody.Append("\n\tpublic static  " + MethodReturn + " " + MethodName + "(" + Globals.GetAliasDataType(ID.Datatype) + " " + id + ")");
					methodBody.Append("\n\t{");
					methodBody.Append("\n\t\treturn \" " + global.TableProgramatlyName + "-\" + " + id +";");
					methodBody.Append("\n\t}");
					methodBody.Append("\n\t" + Globals.MetthodsSeparator);
					methodBody.Append("\n\t#endregion");
					return methodBody.ToString();
				}
				catch (Exception ex)
				{
					MessageBox.Show("My Generated Code Exception:" + ex.Message);
					return "";
				}
			}
			else
				return "";
		}
		//
        #endregion
        //----------------------------------------------------------
        
        //----------------------------------------------------------
        
        //----------------------------------------------------------
        
        //----------------------------------------------------------
        
        //----------------------------------------------------------
        #endregion
    }
}

