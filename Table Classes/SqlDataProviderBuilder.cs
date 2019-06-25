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
	public class SqlDataProviderBuilder:Generator 
	{
        public SqlDataProviderBuilder()
		{
            ClassName = global.SqlDataProviderClass;
		}
        private string GeneateUsingBlock()
        {
            string Usingblock = "";
            Usingblock += "using System;\n";
            Usingblock += "using System.Collections;\n"; 
            Usingblock += "using System.Collections.Generic;\n";
            Usingblock += "using System.Data;\n";
            Usingblock += "using System.Data.SqlClient;\n";
            Usingblock += "using System.Data.SqlTypes;\n";
            Usingblock += "using System.Configuration;\n";
            return Usingblock;
        }
        //
        private string GenerateClassBody()
        {
            StringBuilder MethodsBuilder = new StringBuilder();
            MethodsBuilder.Append(CreateInstanceProperty());
            MethodsBuilder.Append("\n" + CreateGetConnectionMethod());
            MethodsBuilder.Append("\n" + CreateCreateMethod());
            MethodsBuilder.Append("\n" + CreateUpdateMethod());
			MethodsBuilder.Append("\n" + CreateDeleteMethod());
            MethodsBuilder.Append("\n" + CreateGetAllWithPagerMethod());
            MethodsBuilder.Append("\n" + CreateGetCountMethod());
            MethodsBuilder.Append(GenerateReletionalGetData());
            if (ID != null)
                MethodsBuilder.Append("\n" + CreateGetOneMethod(StoredProcedureTypes.GetOneByID));
            MethodsBuilder.Append("\n" + CreatePopulateObjectMethod());
            return MethodsBuilder.ToString();
        }
        //
        private string GenerateClass(string classBody)
        {
            string xmlDocumentation ="\n/// <summary>\n";
            xmlDocumentation += "/// " + SqlProvider.obj.TableName + " SQL data provider which represents the data access layer of " + SqlProvider.obj.TableName + ".\n";
            xmlDocumentation += "/// </summary>\n";
            string _class = "";
            _class += GeneateUsingBlock();
            if (ProjectBuilder.AllowXmlDocumentation)
                _class += xmlDocumentation;
            _class += "public class " + ClassName;
            _class += "\n{\n" + classBody + "\n}";
            return _class;
        }
        //
		public static void Create()
		{
			SqlDataProviderBuilder dp=new SqlDataProviderBuilder();
            dp.CreateClassFile();
		}
        //
		private void CreateClassFile()
		{
			try
			{
                string _class = GenerateClass(GenerateClassBody());
				DirectoryInfo dInfo= Directory.CreateDirectory(Globals.ClassesDirectory+global.TableProgramatlyName);
				string path = dInfo.FullName+"\\"+ClassName+".cs";
				// Create a file to write to.
				using (StreamWriter sw = File.CreateText(path)) 
				{
					
					sw.WriteLine(_class);				
				}    
				
			}
			catch(Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:"+ex.Message);
				
			}
		}
        //
        #region ClassMember
        //----------------------------------------------------------
        #region CreateInstanceProperty
        private string CreateInstanceProperty()
        {

			try
            {
                string xmlDocumentation = "\n\t/// <summary>\n";
                xmlDocumentation += "\t/// Gets instance of " + ClassName + " calss.\n";
                xmlDocumentation += "\t/// <example>" + ClassName + " " + global.SqlDataProviderObject + "=" + ClassName + ".Instance.</example>\n";
                xmlDocumentation += "\t/// </summary>";
                
                StringBuilder methodBody = new StringBuilder();
				methodBody.Append("\n\t#region --------------Instance--------------");
				methodBody.Append("\n\tprivate static " + ClassName + "  _Instance;");
                if (ProjectBuilder.AllowXmlDocumentation)
                    methodBody.Append(xmlDocumentation);
                methodBody.Append("\n\tpublic static " + ClassName + "  Instance");
                methodBody.Append("\n\t{");
				methodBody.Append("\n\t\tget");
				methodBody.Append("\n\t\t{");
				methodBody.Append("\n\t\t\tif (_Instance == null)");
				methodBody.Append("\n\t\t\t{");
				methodBody.Append("\n\t\t\t\t_Instance = new " + ClassName + "();");
				methodBody.Append("\n\t\t\t}");
				methodBody.Append("\n\t\t\treturn _Instance;");
                methodBody.Append("\n\t\t} ");
                methodBody.Append("\n\t} ");
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
        #region CreateGetConnectionMethod
        private string CreateGetConnectionMethod()
        {
            try
            {
                //XML Documentaion
                string xmlDocumentation = "\n\t/// <summary>\n";
                xmlDocumentation += "\t/// Creates and returns a new SqlConnection Which its connection string depends on AppSettings[\"Connectionstring\"].\n";
                xmlDocumentation += "\t/// </summary>\n";
                xmlDocumentation += "\t/// <returns></returns>";
                //Method Body 
                StringBuilder methodBody = new StringBuilder();
				methodBody.Append("\n\t#region --------------GetSqlConnection--------------");
                if (ProjectBuilder.AllowXmlDocumentation)
				    methodBody.Append(xmlDocumentation);
                methodBody.Append("\n\tpublic SqlConnection GetSqlConnection()");
                methodBody.Append("\n\t{");
				methodBody.Append("\n\t\treturn new SqlConnection(ConfigurationManager.ConnectionStrings[\"Connectionstring\"].ToString());");
                methodBody.Append("\n\t} ");
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
        #region CreateCreateMethod
        private string CreateCreateMethod()
        {
            StoredProcedureTypes type = StoredProcedureTypes.Create;
            string id = Globals.GetProgramatlyName(ID.Name);
            id = Globals.ConvetStringToCamelCase(id);
            //
            string ProcName = global.TableProgramatlyName + "_" + type.ToString();
            string MethodName = MethodName = type.ToString();
            string MethodReturn = "bool";
            if(ProjectBuilder.ISExcuteScaler)
                MethodReturn = "ExecuteCommandStatus";
            //XML Documentaion
            string xmlInsertDocumentation = "\n\t/// <summary>\n";
            xmlInsertDocumentation += "\t/// Converts the " + SqlProvider.obj.TableName + " object properties to SQL paramters and executes the create " + SqlProvider.obj.TableName + " procedure \n";
            xmlInsertDocumentation += "\t/// and updates the " + SqlProvider.obj.TableName + " object with the SQL data by reference.\n";
            xmlInsertDocumentation += "\t/// <example>[Example]" + MethodReturn + " status=" + ClassName + ".Instance." + MethodName + "(" + global.EntityClassObject + ");.</example>\n";

            xmlInsertDocumentation += "\t/// </summary>\n";
            xmlInsertDocumentation += "\t/// <param name=\"" + global.EntityClassObject + "\">The " + SqlProvider.obj.TableName + " object.</param>\n";
            xmlInsertDocumentation += "\t/// <returns>The status of create query.</returns>";
           
            //method body
            try
            {
                StringBuilder methodBody = new StringBuilder();
                methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
                methodBody.Append(xmlInsertDocumentation);
                 //
                methodBody.Append("\n\tpublic  " + MethodReturn + " " + MethodName + "(" + global.TableEntityClass + " " + global.EntityClassObject + ")");
                methodBody.Append("\n\t{");
                methodBody.Append("\n\t\tusing( SqlConnection myConnection = GetSqlConnection()) ");
                methodBody.Append("\n\t\t{");
                methodBody.Append("\n\t\t\tSqlCommand myCommand = new SqlCommand(\"" + ProcName + "\", myConnection);");
                methodBody.Append("\n\t\t\tmyCommand.CommandType = CommandType.StoredProcedure;");
                methodBody.Append("\n\t\t\t// Set the parameters");

                foreach (SQLDMO.Column colCurrent in Fields)
                {
                    //colCurrent.Name.IndexOf("_") meen not a default value
                    if (ID != null && colCurrent.Name == ID.Name  && ID.IdentityIncrement > 0)
                        methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + "," + colCurrent.Length + ").Direction = ParameterDirection.Output;");
                    else if (colCurrent.Name.ToLower() == ProjectBuilder.LangID.ToLower())
                    {
                         methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + ").Value = (" + Globals.GetAliasDataType(colCurrent.Datatype) + ")" + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + ";");
                    }
                    else if (colCurrent.Datatype.ToLower() == SqlDbType.NText.ToString().ToLower() && colCurrent.Name.IndexOf("_") < 0)
                    {
                        methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + ").Value = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + ";");
                    }
                    else if (colCurrent.Name.IndexOf("_") < 0)
                    {
                        methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + "," + colCurrent.Length + ").Value = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + ";");
                    }
                }
               
                methodBody.Append("\n\t\t\t// Execute the command");
                if (!ProjectBuilder.ISExcuteScaler)
                    methodBody.Append("\n\t\t\tbool status=false;");
                methodBody.Append("\n\t\t\tmyConnection.Open();");
                if (!ProjectBuilder.ISExcuteScaler)
                {
                    methodBody.Append("\n\t\t\tif(myCommand.ExecuteNonQuery()>0)");
                    methodBody.Append("\n\t\t\t{");
                    methodBody.Append("\n\t\t\t\tstatus=true;");
                    if (ID != null && ID.IdentityIncrement > 0)
                    {
                        methodBody.Append("\n\t\t\t\t//Get ID value from database and set it in object");
                        methodBody.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + "= (" + Globals.GetAliasDataType(ID.Datatype) + ") myCommand.Parameters[\"@" + Globals.GetProgramatlyName(ID.Name) + "\"].Value;");
                    }

                }
                else
                {
                    methodBody.Append("\n\t\t\tExecuteCommandStatus status = (ExecuteCommandStatus)myCommand.ExecuteScalar();");
                    methodBody.Append("\n\t\t\tif (status == ExecuteCommandStatus.Done)");
                    methodBody.Append("\n\t\t\t{");
                    if (ID != null && ID.IdentityIncrement > 0)
                    {
                        methodBody.Append("\n\t\t\t\t//Get ID value from database and set it in object");
                        methodBody.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + "= (" + Globals.GetAliasDataType(ID.Datatype) + ") myCommand.Parameters[\"@" + Globals.GetProgramatlyName(ID.Name) + "\"].Value;");
                    }
                }
                methodBody.Append("\n\t\t\t}");
                methodBody.Append("\n\t\t\tmyConnection.Close();");
                methodBody.Append("\n\t\t\treturn status;");
                methodBody.Append("\n\t\t}");
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
        #region CreateUpdateMethod
        private string CreateUpdateMethod()
        {
            StoredProcedureTypes type = StoredProcedureTypes.Update;
            string id = Globals.GetProgramatlyName(ID.Name);
            id = Globals.ConvetStringToCamelCase(id);
            //
            string ProcName = global.TableProgramatlyName + "_" + type.ToString();
            string MethodName = MethodName = type.ToString();
            string MethodReturn = "bool";
            if (ProjectBuilder.ISExcuteScaler)
                MethodReturn = "ExecuteCommandStatus";
            //XML Documentaion
            string xmlUpdateDocumentation = "\n\t/// <summary>\n";
            xmlUpdateDocumentation += "\t/// Converts the " + SqlProvider.obj.TableName + " object properties to SQL paramters and executes the update " + SqlProvider.obj.TableName + " procedure.\n";
            xmlUpdateDocumentation += "\t/// <example>[Example]" + MethodReturn + "  status=" + ClassName + ".Instance." + MethodName + "(" + global.EntityClassObject + ");.</example>\n";
            xmlUpdateDocumentation += "\t/// </summary>\n";
            xmlUpdateDocumentation += "\t/// <param name=\"" + global.EntityClassObject + "\">The " + SqlProvider.obj.TableName + " object.</param>\n";
            xmlUpdateDocumentation += "\t/// <returns>The status of update query.</returns>";
            //method body
            try
            {
                StringBuilder methodBody = new StringBuilder();
                methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
                methodBody.Append(xmlUpdateDocumentation);
                //
                methodBody.Append("\n\tpublic  " + MethodReturn + " " + MethodName + "(" + global.TableEntityClass + " " + global.EntityClassObject + ")");
                methodBody.Append("\n\t{");
                methodBody.Append("\n\t\tusing( SqlConnection myConnection = GetSqlConnection()) ");
                methodBody.Append("\n\t\t{");
                methodBody.Append("\n\t\t\tSqlCommand myCommand = new SqlCommand(\"" + ProcName + "\", myConnection);");
                methodBody.Append("\n\t\t\tmyCommand.CommandType = CommandType.StoredProcedure;");
                methodBody.Append("\n\t\t\t// Set the parameters");

                foreach (SQLDMO.Column colCurrent in Fields)
                {
                    //colCurrent.Name.IndexOf("_") meen not a default value
                    //if (colCurrent.Name.ToLower() == ProjectBuilder.LangID.ToLower())
                    //{
                    //    //Only in create
                    //    if (type == StoredProcedureTypes.Create)
                    //        methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + ").Value = (" + Globals.GetAliasDataType(colCurrent.Datatype) + ")" + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + ";");
                    //}
                    //else
                    if (colCurrent.Datatype.ToLower() == SqlDbType.NText.ToString().ToLower() && colCurrent.Name.IndexOf("_") < 0 && colCurrent.Name.ToLower() != "langid")
                    {
                        methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + ").Value = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + ";");
                    }
                    else if (colCurrent.Name.IndexOf("_") < 0 && colCurrent.Name.ToLower() != "langid")
                    {
                        methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + "," + colCurrent.Length + ").Value = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + ";");
                    }
                }

                methodBody.Append("\n\t\t\t// Execute the command");
                if (!ProjectBuilder.ISExcuteScaler)
                    methodBody.Append("\n\t\t\tbool status=false;");
                methodBody.Append("\n\t\t\tmyConnection.Open();");
                if (!ProjectBuilder.ISExcuteScaler)
                {
                    methodBody.Append("\n\t\t\tif(myCommand.ExecuteNonQuery()>0)");
                    methodBody.Append("\n\t\t\t{");
                    methodBody.Append("\n\t\t\t\tstatus=true;");
                }
                else
                {
                    methodBody.Append("\n\t\t\tExecuteCommandStatus status = (ExecuteCommandStatus)myCommand.ExecuteScalar();");
                    methodBody.Append("\n\t\t\tif (status == ExecuteCommandStatus.Done)");
                    methodBody.Append("\n\t\t\t{");
                }
                methodBody.Append("\n\t\t\t}");
                methodBody.Append("\n\t\t\tmyConnection.Close();");
                methodBody.Append("\n\t\t\treturn status;");
                methodBody.Append("\n\t\t}");
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
        #region CreateDeleteMethod
        private string CreateDeleteMethod()
        {
            StoredProcedureTypes type = StoredProcedureTypes.Delete;
            string id = Globals.GetProgramatlyName(ID.Name);
            id = Globals.ConvetStringToCamelCase(id);
            //
            string ProcName = global.TableProgramatlyName + "_" + type.ToString();
            string MethodName = MethodName = type.ToString();
            string MethodReturn = "bool";
            //XML Documentaion
            string xmlDeleteDocumentation = "\n\t/// <summary>\n";
            xmlDeleteDocumentation += "\t/// Deletes single " + SqlProvider.obj.TableName + " object .\n";
            xmlDeleteDocumentation += "\t/// <example>[Example]bool status=" + ClassName + ".Instance." + MethodName + "(" + id + ");.</example>\n";
            xmlDeleteDocumentation += "\t/// </summary>\n";
            xmlDeleteDocumentation += "\t/// <param name=\"" + id + "\">The " + global.EntityClassObject + " id.</param>\n";
            xmlDeleteDocumentation += "\t/// <returns>The status of delete query.</returns>";
            //method body
            try
            {
                StringBuilder methodBody = new StringBuilder();
                methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
                methodBody.Append(xmlDeleteDocumentation);

                //
                methodBody.Append("\n\tpublic  " + MethodReturn + " " + MethodName + "(" + Globals.GetAliasDataType(ID.Datatype) + " " + id + ")");

                methodBody.Append("\n\t{");
                methodBody.Append("\n\t\tbool status=false;");
                methodBody.Append("\n\t\tusing( SqlConnection myConnection = GetSqlConnection()) ");
                methodBody.Append("\n\t\t{");
                methodBody.Append("\n\t\t\tSqlCommand myCommand = new SqlCommand(\"" + ProcName + "\", myConnection);");
                methodBody.Append("\n\t\t\tmyCommand.CommandType = CommandType.StoredProcedure;");
                methodBody.Append("\n\t\t\t// Set the parameters");
                methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(ID.Name) + "\", SqlDbType." + Globals.GetSqlDataType(ID.Datatype).ToString() + "," + ID.Length + ").Value = " + id + ";");

                methodBody.Append("\n\t\t\t// Execute the command");
                methodBody.Append("\n\t\t\tmyConnection.Open();");
                methodBody.Append("\n\t\t\tif(myCommand.ExecuteNonQuery()>0)");
                methodBody.Append("\n\t\t\t{");
                methodBody.Append("\n\t\t\t\tstatus=true;");
                if (ID != null && type == StoredProcedureTypes.Create && ID.IdentityIncrement > 0)
                {
                    methodBody.Append("\n\t\t\t\t//Get ID value from database and set it in object");
                    methodBody.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + "= (" + Globals.GetAliasDataType(ID.Datatype) + ") myCommand.Parameters[\"@" + Globals.GetProgramatlyName(ID.Name) + "\"].Value;");
                }
                if (ID.Name.ToLower() == "id" && global.EntityClassObject == "Site_RolesObj")
                    MessageBox.Show(SqlProvider.obj.ID.Name);
                methodBody.Append("\n\t\t\t}");
                methodBody.Append("\n\t\t\tmyConnection.Close();");
                methodBody.Append("\n\t\t\treturn status;");
                methodBody.Append("\n\t\t}");
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
        /*Old Code
        #region CreateInsertUpdateDeleteMethod
        private string CreateInsertUpdateDeleteMethod(StoredProcedureTypes type)
        {
			string id = Globals.GetProgramatlyName(ID.Name);
			id = Globals.ConvetStringToCamelCase(id);
			//
            string ProcName = global.TableProgramatlyName + "_" + type.ToString();
			string MethodName = MethodName = type.ToString() ;
			string MethodReturn = "bool";
            //XML Documentaion
            string xmlInsertDocumentation = "\n\t/// <summary>\n";
            xmlInsertDocumentation += "\t/// Converts the " + SqlProvider.obj.TableName + " object properties to SQL paramters and executes the create " + SqlProvider.obj.TableName + " procedure \n";
            xmlInsertDocumentation += "\t/// and updates the " + SqlProvider.obj.TableName + " object with the SQL data by reference.\n";
            //xmlInsertDocumentation += "\t/// <example>[Example]bool status=" + ClassName + ".Instance." + MethodName + "(" + global.EntityClassObject + ");.</example>\n";
            xmlInsertDocumentation += "\t/// </summary>\n";
            xmlInsertDocumentation += "\t/// <param name=\"" + global.EntityClassObject + "\">The " + SqlProvider.obj.TableName + " object.</param>\n";
            xmlInsertDocumentation += "\t/// <returns>The status of create query.</returns>";
            //
            
			//
			string xmlDeleteDocumentation = "\n\t/// <summary>\n";
			xmlDeleteDocumentation += "\t/// Deletes single " + SqlProvider.obj.TableName + " object .\n";
			//xmlDeleteDocumentation += "\t/// <example>[Example]bool status=" + ClassName + ".Instance." + MethodName + "(" + id + ");.</example>\n";
			xmlDeleteDocumentation += "\t/// </summary>\n";
			xmlDeleteDocumentation += "\t/// <param name=\"" + id + "\">The " + global.EntityClassObject + " id.</param>\n";
			xmlDeleteDocumentation += "\t/// <returns>The status of delete query.</returns>";
           	//method body
            try
            {
                StringBuilder methodBody = new StringBuilder();
				methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
                if (type == StoredProcedureTypes.Create)
                    methodBody.Append(xmlInsertDocumentation);
				else if (type == StoredProcedureTypes.Update)
                    methodBody.Append(xmlUpdateDocumentation);
				else if(type == StoredProcedureTypes.Delete)
					methodBody.Append(xmlDeleteDocumentation);

				//
				if(type == StoredProcedureTypes.Delete)
					methodBody.Append("\n\tpublic  " + MethodReturn + " " + MethodName + "(" + Globals.GetAliasDataType(ID.Datatype) + " " + id + ")");
				else
					methodBody.Append("\n\tpublic  " + MethodReturn + " " + MethodName + "(" + global.TableEntityClass + " " + global.EntityClassObject + ")");
                methodBody.Append("\n\t{");
                methodBody.Append("\n\t\tbool status=false;");
                methodBody.Append("\n\t\tusing( SqlConnection myConnection = GetSqlConnection()) ");
                methodBody.Append("\n\t\t{");
                methodBody.Append("\n\t\t\tSqlCommand myCommand = new SqlCommand(\"" + ProcName + "\", myConnection);");
                methodBody.Append("\n\t\t\tmyCommand.CommandType = CommandType.StoredProcedure;");
                methodBody.Append("\n\t\t\t// Set the parameters");
				if (type == StoredProcedureTypes.Delete)
				{
					methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(ID.Name) + "\", SqlDbType." + Globals.GetSqlDataType(ID.Datatype).ToString() + "," + ID.Length + ").Value = " + id + ";");
				}
				else
				{
					foreach (SQLDMO.Column colCurrent in Fields)
					{
						//colCurrent.Name.IndexOf("_") meen not a default value
						if (ID != null && colCurrent.Name == ID.Name && type == StoredProcedureTypes.Create && ID.IdentityIncrement > 0)
							methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + "," + colCurrent.Length + ").Direction = ParameterDirection.Output;");
                        else if (colCurrent.Name.ToLower() == ProjectBuilder.LangID.ToLower())
						{
                            //Only in create
                            if (type == StoredProcedureTypes.Create)
							    methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + ").Value = (" + Globals.GetAliasDataType(colCurrent.Datatype)+")" + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + ";");
						}
						else if (colCurrent.Datatype.ToLower() == SqlDbType.NText.ToString().ToLower() && colCurrent.Name.IndexOf("_") < 0)
						{
							methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + ").Value = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + ";");
						}
						else if (colCurrent.Name.IndexOf("_") < 0)
						{
							methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + "," + colCurrent.Length + ").Value = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + ";");
						}
					}
				}
                methodBody.Append("\n\t\t\t// Execute the command");
                methodBody.Append("\n\t\t\tmyConnection.Open();");
                methodBody.Append("\n\t\t\tif(myCommand.ExecuteNonQuery()>0)");
                methodBody.Append("\n\t\t\t{");
                methodBody.Append("\n\t\t\t\tstatus=true;");
                if (ID != null && type == StoredProcedureTypes.Create && ID.IdentityIncrement > 0)
                {
                    methodBody.Append("\n\t\t\t\t//Get ID value from database and set it in object");
                    methodBody.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + "= (" + Globals.GetAliasDataType(ID.Datatype) + ") myCommand.Parameters[\"@" + Globals.GetProgramatlyName(ID.Name) + "\"].Value;");
                }
                if (ID.Name.ToLower() == "id" && global.EntityClassObject == "Site_RolesObj")
                    MessageBox.Show(SqlProvider.obj.ID.Name);
                methodBody.Append("\n\t\t\t}");
                methodBody.Append("\n\t\t\tmyConnection.Close();");
                methodBody.Append("\n\t\t\treturn status;");
                methodBody.Append("\n\t\t}");
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
        */
        //----------------------------------------------------------
        #region GenerateReletionalGetData
        private string GenerateReletionalGetData()
        {
            StringBuilder relationalMethods = new StringBuilder(); 
            foreach (SQLDMO.Column column in Fields)
            {
                //if ((ID == null || column.Name != ID.Name) && (column.Default == null || column.Default.Length != 0) && column.Name.ToLower() != ProjectBuilder.LangID)
                if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0))
                {
                    TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
                    if (cnstr != null)
                    {
                        relationalMethods.Append("\n" + CreateGetAllWithPagerMethod(column));
                        relationalMethods.Append("\n" + CreateGetCountMethod(column));
                    }
                }
            }
           return relationalMethods.ToString();


       }
        #endregion
       //----------------------------------------------------------
        #region CreateGetCountMethod
       private string CreateGetCountMethod()
       {
           return CreateGetCountMethod(null);
       }
       //----------------------------------------------------------
        private string CreateGetCountMethod(SQLDMO.Column conditionalColumn)
       {
           try
           {
               string ProcName = global.TableProgramatlyName + "_" + StoredProcedureTypes.GetCount.ToString();
               string MethodName = StoredProcedureTypes.GetCount.ToString() + global.TableProgramatlyName;
               string methodParameters = "";
               //XXXXXXXXXXXXXXX
               bool hasPreviousParameter = false;
               //------------------------------
               bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(SqlProvider.obj.TableName));
               if (isMaultiLanguages)
               {
                   //Check Previous Parameter
                   if (hasPreviousParameter)
                       methodParameters += ", ";
                   methodParameters += "Languages langID";
                   hasPreviousParameter = true;
               }
               //------------------------------
               if (conditionalColumn != null)
               {
                   //Check Previous Parameter
                   if (hasPreviousParameter)
                       methodParameters += ", ";
                   MethodName += "By" + Globals.GetProgramatlyName(conditionalColumn.Name);
                   ProcName += "By" + Globals.GetProgramatlyName(conditionalColumn.Name);
                   methodParameters += Globals.GetAliasDataType(conditionalColumn.Datatype) + " " + Globals.ConvetStringToCamelCase(Globals.GetProgramatlyName(conditionalColumn.Name));
                   hasPreviousParameter = true;

               }
               //------------------------------
               bool hasIsAvailable = SqlProvider.CheckIsATableHasIsAvailableColumnName(SqlProvider.obj.TableName);
               if (hasIsAvailable)
               {
                   //Check Previous Parameter
                   if (hasPreviousParameter)
                       methodParameters += ", ";
                   methodParameters += "bool " + Globals.ConvetStringToCamelCase(ProjectBuilder.IsAvailableConditionParam);
                   hasPreviousParameter = true;

               }
               //------------------------------
               //XXXXXXXXXXXXXXX
               //LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL
               string MethodReturn = "int";
               //LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL
               //XML Documentaion
               string xmlDocumentation = "\n\t/// <summary>\n";
               xmlDocumentation += "\t/// Gets All " + SqlProvider.obj.TableName + " Records.\n";
               xmlDocumentation += "\t/// <example>[Example]DataTable dt" + global.TableProgramatlyName + "=" + ClassName + ".Instance." + MethodName + "();.</example>\n";
               xmlDocumentation += "\t/// </summary>\n";
               xmlDocumentation += "\t/// <returns>Entities Counts.</returns>";
               //Method Body

               //
               StringBuilder methodBody = new StringBuilder();
               methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
               if (ProjectBuilder.AllowXmlDocumentation)
                   methodBody.Append(xmlDocumentation);
               methodBody.Append("\n\tpublic  " + MethodReturn + " " + MethodName + "(" + methodParameters + ")");
               methodBody.Append("\n\t{");
               methodBody.Append("\n\t\tusing( SqlConnection myConnection = GetSqlConnection()) ");
               methodBody.Append("\n\t\t{");
               //LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL
              
               //LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL
               methodBody.Append("\n\t\t\tSqlCommand myCommand = new SqlCommand(\"" + ProcName + "\", myConnection);");
               methodBody.Append("\n\t\t\tmyCommand.CommandType = CommandType.StoredProcedure;");
               //------------------------------
               if (conditionalColumn != null)
               {
                   if (conditionalColumn.Datatype.ToLower() == SqlDbType.NText.ToString().ToLower())
                   {
                       methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(conditionalColumn.Name) + "\", SqlDbType." + Globals.GetSqlDataType(conditionalColumn.Datatype).ToString() + ").Value = " + Globals.ConvetStringToCamelCase(Globals.GetProgramatlyName(conditionalColumn.Name)) + ";");
                   }
                   else
                   {
                       methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(conditionalColumn.Name) + "\", SqlDbType." + Globals.GetSqlDataType(conditionalColumn.Datatype).ToString() + "," + conditionalColumn.Length + ").Value = " + Globals.ConvetStringToCamelCase(Globals.GetProgramatlyName(conditionalColumn.Name)) + ";");
                   }
               }
               //------------------------------
               if (isMaultiLanguages)
               {
                   methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@LangID\", SqlDbType.Int,8).Value = (int) langID ;");
               }
               //------------------------------
               if (hasIsAvailable)
               {
                   methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + ProjectBuilder.IsAvailableConditionParam + "\", SqlDbType.Bit,1).Value = " + Globals.ConvetStringToCamelCase(ProjectBuilder.IsAvailableConditionParam) + " ;");
               }
               //------------------------------
               methodBody.Append("\n\t\t\t// Execute the command");
               //LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL
               methodBody.Append("\n\t\t\tmyConnection.Open();");
               methodBody.Append("\n\t\t\tint itemsCount = (int)myCommand.ExecuteScalar();");
              
               
               methodBody.Append("\n\t\t\tmyConnection.Close();");
               methodBody.Append("\n\t\t\treturn itemsCount;");
               //LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL
               methodBody.Append("\n\t\t}");
               methodBody.Append("\n\t}");
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
        
        //----------------------------------------------------------
        #region CreateGetAllWithPagerMethod
        private string CreateGetAllWithPagerMethod()
        {
            return CreateGetAllWithPagerMethod(null);
        }
        //----------------------------------------------------------
        private string CreateGetAllWithPagerMethod(SQLDMO.Column conditionalColumn)
		{
			try
            {
                string ProcName = global.TableProgramatlyName + "_" + StoredProcedureTypes.GetAll.ToString();
                if (conditionalColumn != null)
                    ProcName += "By" + Globals.GetProgramatlyName(conditionalColumn.Name);
                //-----------
                string MethodName = "GetAll";
                string methodParameters = "";
                string sqlMethodParameters = "";
                //------------------------------
                bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(SqlProvider.obj.TableName));
                if (isMaultiLanguages)
                {
                    methodParameters += "Languages langID,";
                    sqlMethodParameters += "langID,";
                }
                //------------------------------
                if (conditionalColumn != null)
                {
                    MethodName += "By" + Globals.GetProgramatlyName(conditionalColumn.Name);
                    methodParameters += Globals.GetAliasDataType(conditionalColumn.Datatype) + " " + Globals.ConvetStringToCamelCase(Globals.GetProgramatlyName(conditionalColumn.Name)) + ",";
                    sqlMethodParameters += Globals.ConvetStringToCamelCase(Globals.GetProgramatlyName(conditionalColumn.Name)) + ",";
                }
                //------------------------------
                bool hasIsAvailable = SqlProvider.CheckIsATableHasIsAvailableColumnName(SqlProvider.obj.TableName);
                if (hasIsAvailable)
                {
                    methodParameters += "bool " + Globals.ConvetStringToCamelCase(ProjectBuilder.IsAvailableConditionParam) + ",";
                    sqlMethodParameters += Globals.ConvetStringToCamelCase(ProjectBuilder.IsAvailableConditionParam) + ",";
                }
                //------------------------------
                methodParameters += "int pageIndex, int pageSize, out int totalRecords";
                sqlMethodParameters += "pageIndex, pageSize, out totalRecords";
                //XXXXXXXXXXXXXXX
                string MethodReturn = "List<" + global.TableEntityClass + ">";
                //XML Documentaion
                string xmlDocumentation = "\n\t/// <summary>\n";
                xmlDocumentation += "\t/// Gets All " + SqlProvider.obj.TableName + " Records.\n";
                xmlDocumentation += "\t/// <example>[Example]List<" + global.TableEntityClass + "> " + global.EntityClassList + " =" + ClassName + ".Instance." + MethodName + "(" + sqlMethodParameters + ");.</example>\n";
                xmlDocumentation += "\t/// </summary>\n";
                xmlDocumentation += "\t/// <returns>Specified Entities.</returns>";
                //Method Body
                //
                StringBuilder methodBody = new StringBuilder();
                methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
                if (ProjectBuilder.AllowXmlDocumentation)
                    methodBody.Append(xmlDocumentation);
                methodBody.Append("\n\tpublic  " + MethodReturn + " " + MethodName + "("+ methodParameters +")");
                methodBody.Append("\n\t{");
                methodBody.Append("\n\t\tusing( SqlConnection myConnection = GetSqlConnection()) ");
                methodBody.Append("\n\t\t{");
                //LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL
                methodBody.Append("\n\t\t\tList<" + global.TableEntityClass + "> " + global.EntityClassList + " = new List<" + global.TableEntityClass + ">();");
                methodBody.Append("\n\t\t\t" + global.TableEntityClass + " " + global.EntityClassObject + ";");
                //LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL
                methodBody.Append("\n\t\t\tSqlCommand myCommand = new SqlCommand(\"" + ProcName + "\", myConnection);");
                methodBody.Append("\n\t\t\tmyCommand.CommandType = CommandType.StoredProcedure;");
                methodBody.Append("\n\t\t\t// Set the parameters");
                //XXXXXXXXXX
                if (conditionalColumn != null)
                {
                    if (conditionalColumn.Datatype.ToLower() == SqlDbType.NText.ToString().ToLower())
                    {
                        methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(conditionalColumn.Name) + "\", SqlDbType." + Globals.GetSqlDataType(conditionalColumn.Datatype).ToString() + ").Value = " + Globals.ConvetStringToCamelCase(Globals.GetProgramatlyName(conditionalColumn.Name)) + ";");
                    }
                    else
                    {
                        methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(conditionalColumn.Name) + "\", SqlDbType." + Globals.GetSqlDataType(conditionalColumn.Datatype).ToString() + "," + conditionalColumn.Length + ").Value = " + Globals.ConvetStringToCamelCase(Globals.GetProgramatlyName(conditionalColumn.Name)) + ";");
                    }
                }
                //------------------------------
                if (isMaultiLanguages)
                {
                    methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@LangID\", SqlDbType.Int,8).Value = (int) langID ;");
                }
                //------------------------------
                if (hasIsAvailable)
                {
                    methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + ProjectBuilder.IsAvailableConditionParam + "\", SqlDbType.Bit,1).Value = " + Globals.ConvetStringToCamelCase(ProjectBuilder.IsAvailableConditionParam) + " ;");
                }
                //------------------------------
                //XXXXXXXXXX
                methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@pageIndex\", SqlDbType.Int, 4).Value = pageIndex;");
                methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@pageSize\", SqlDbType.Int, 4).Value = pageSize;");
                methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@TotalRecords\", SqlDbType.Int, 4).Direction = ParameterDirection.Output;");
                methodBody.Append("\n\t\t\t// Execute the command");
                //LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL
                methodBody.Append("\n\t\t\tSqlDataReader dr;");
                methodBody.Append("\n\t\t\tmyConnection.Open();");
                methodBody.Append("\n\t\t\tdr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);");
                methodBody.Append("\n\t\t\twhile (dr.Read())");
                methodBody.Append("\n\t\t\t{");
                methodBody.Append("\n\t\t\t\t(" + global.EntityClassObject + ") = " + global.PopulateMethodName + "(dr);");
                methodBody.Append("\n\t\t\t\t" + global.EntityClassList + ".Add(" + global.EntityClassObject + ");");
                methodBody.Append("\n\t\t\t}");
                methodBody.Append("\n\t\t\tdr.Close();");
                methodBody.Append("\n\t\t\tmyConnection.Close();");
                methodBody.Append("\n\t\t\t//Gets result rows count");
                methodBody.Append("\n\t\t\ttotalRecords = (int)myCommand.Parameters[\"@TotalRecords\"].Value;");
                methodBody.Append("\n\t\t\treturn " + global.EntityClassList + ";");
                //LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL
                methodBody.Append("\n\t\t}");
                methodBody.Append("\n\t}");
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
        #region CreateGetOneMethod
        private string CreateGetOneMethod(StoredProcedureTypes type)
        {
            if (ID != null)
            {
                try
                {
					string id = Globals.GetProgramatlyName(ID.Name);
					id = Globals.ConvetStringToCamelCase(id);
					//
                    string ProcName = global.TableProgramatlyName + "_" + type.ToString();
                    string MethodName = "GetObject";
                    string MethodReturn = global.TableEntityClass;
                    //XML Documentaion
                    string xmlDocumentation = "\n\t/// <summary>\n";
                    xmlDocumentation += "\t/// Gets single " + SqlProvider.obj.TableName + " object .\n";
                    xmlDocumentation += "\t/// <example>[Example]" + global.TableEntityClass + " " + global.EntityClassObject + "=" + ClassName + ".Instance." + MethodName + "(" + id + ");.</example>\n";
                    xmlDocumentation += "\t/// </summary>\n";
					xmlDocumentation += "\t/// <param name=\"" + id + "\">The " + global.EntityClassObject + " id.</param>\n";
                    xmlDocumentation += "\t/// <returns>" + SqlProvider.obj.TableName + " object.</returns>";
                    //Method Body
                    StringBuilder methodBody = new StringBuilder();
					methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
                    if (ProjectBuilder.AllowXmlDocumentation)
                        methodBody.Append(xmlDocumentation);
					methodBody.Append("\n\tpublic  " + MethodReturn + " " + MethodName + "(" + Globals.GetAliasDataType(ID.Datatype) + " " + id + ")");
                    methodBody.Append("\n\t{");
                    methodBody.Append("\n\t\t" + global.TableEntityClass + " " + global.EntityClassObject + " = null;");
                    methodBody.Append("\n\t\tusing( SqlConnection myConnection = GetSqlConnection()) ");
                    methodBody.Append("\n\t\t{");
                    methodBody.Append("\n\t\t\tSqlCommand myCommand = new SqlCommand(\"" + ProcName + "\", myConnection);");
                    methodBody.Append("\n\t\t\tmyCommand.CommandType = CommandType.StoredProcedure;");
                    methodBody.Append("\n\t\t\t// Set the parameters");

					methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(ID.Name) + "\", SqlDbType." + Globals.GetSqlDataType(ID.Datatype).ToString() + "," + ID.Length + ").Value = " + id + ";");
                    methodBody.Append("\n\t\t\t// Execute the command");
                    methodBody.Append("\n\t\t\tmyConnection.Open();");
                    methodBody.Append("\n\t\t\tusing(SqlDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection))");
                    methodBody.Append("\n\t\t\t{");
                    methodBody.Append("\n\t\t\t\tif(dr.Read())");
                    methodBody.Append("\n\t\t\t\t{");
                    methodBody.Append("\n\t\t\t\t\t" + global.EntityClassObject + " = " + global.PopulateMethodName + "(dr);");
                    methodBody.Append("\n\t\t\t\t}");
                    methodBody.Append("\n\t\t\t\tdr.Close();");
                    methodBody.Append("\n\t\t\t}");
                    methodBody.Append("\n\t\t\tmyConnection.Close();");
                    //-------------------------------------
                    methodBody.Append("\n\t\t\treturn " + global.EntityClassObject + ";");
                    methodBody.Append("\n\t\t}");
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
        #endregion
        //----------------------------------------------------------
        #region CreatePopulateObjectMethod
        private string CreatePopulateObjectMethod()
        {
            try
            {
                // 
                string MethodName = "PopulateEntity";
                string MethodReturn =global.TableEntityClass;
                //XML Documentaion
                string xmlDocumentation = "\n\t/// <summary>\n";
                xmlDocumentation += "\t/// Populates entity .\n";
                xmlDocumentation += "\t/// <example>[Example]" + global.TableEntityClass + global.EntityClassObject + "=" + MethodName + "(reader);.</example>\n";
                xmlDocumentation += "\t/// </summary>\n";
                xmlDocumentation += "\t/// <param name=\"reader\"></param>\n";
                xmlDocumentation += "\t/// <returns>" + SqlProvider.obj.TableName + " object.</returns>";
                //Method Body
                StringBuilder methodBody = new StringBuilder();
				methodBody.Append("\n\t#region --------------" + MethodName + "--------------");
                if (ProjectBuilder.AllowXmlDocumentation)
                    methodBody.Append(xmlDocumentation);
                methodBody.Append("\n\tprivate " + global.TableEntityClass + " " + MethodName + "(IDataReader reader)");
                methodBody.Append("\n\t{");
                methodBody.Append("\n\t\t//Create a new " + SqlProvider.obj.TableName + " object");
                methodBody.Append("\n\t\t" + global.TableEntityClass + " " + global.EntityClassObject + " = new " + global.TableEntityClass + "();");
                methodBody.Append("\n\t\t//Fill the object with data");
                //
               
                foreach (SQLDMO.Column colCurrent in Fields)
                {
                    methodBody.Append("\n\t\t//" + Globals.GetProgramatlyName(colCurrent.Name) );
                    methodBody.Append("\n\t\tif (reader[\"" + Globals.GetProgramatlyName(colCurrent.Name) + "\"] != DBNull.Value)");
					if (colCurrent.Name.ToLower() == "langid")
					{
						methodBody.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + " = (Languages) reader[\"" + Globals.GetProgramatlyName(colCurrent.Name) + "\"];");
					}
					else
					{
						methodBody.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + " = (" + Globals.GetAliasDataType(colCurrent.Datatype) + ") reader[\"" + Globals.GetProgramatlyName(colCurrent.Name) + "\"];");
					}
                }
                //
                methodBody.Append("\n\t\t//Return the populated object");
                methodBody.Append("\n\t\treturn " + global.EntityClassObject + ";");
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
        #endregion
    }
}

