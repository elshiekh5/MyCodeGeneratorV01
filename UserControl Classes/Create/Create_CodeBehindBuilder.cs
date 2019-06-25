using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Collections;
namespace SPGen
{
	/// <summary>
	/// Summary description for Ceate_CodeBehindBuilder.
	/// </summary>
	public class Create_CodeBehindBuilder : CodeBehindBuilder
	{
		private string photoUploadreID = "fuPhoto";
		private string logoUploadreID = "fuLogo";
		private string fileUploadreID = "fuFile";
		private bool hasPhoto = false;
		private bool hasLogo = false;
		private bool hasFile = false;
        StringBuilder DropDownListsLoader = new StringBuilder();
        StringBuilder LoadPrioritesLoader = new StringBuilder();

		#region ---------------CreateHandleOptionalControls---------------
		//-----------------------------------------------
		//CreateHandleOptionalControls
		//-----------------------------------------------

		private string CreateHandleOptionalControls()
		{
			StringBuilder method = new StringBuilder();
			string propertyName;
			string trPropertyName;
			string optionalPropertyName;
			string optionalFullPropertyName;
			string id = Globals.GetProgramatlyName(ID.Name);
			id = Globals.ConvetStringToCamelCase(id);
			method.Append("\n\t#region ---------------HandleOptionalControls---------------");
			method.Append("\n\t//-----------------------------------------------");
			method.Append("\n\t//HandleOptionalControls");
			method.Append("\n\t//-----------------------------------------------");
			method.Append("\n\tprotected void HandleOptionalControls()");
			method.Append("\n\t{");
			//method.Append("\n\t\tSiteOptions config = SiteOptions.Instance;");
			foreach (SQLDMO.Column column in Fields)
			{
				propertyName = Globals.GetProgramatlyName(column.Name);
				trPropertyName = "tr" + propertyName;
				optionalPropertyName = SiteOptionsBuilder.GetHasPropertyString(propertyName);
				optionalFullPropertyName = SiteOptionsBuilder.GetFullHasPropertyString(propertyName);
				//if ((ID == null || column.Name != ID.Name) && (column.Default == null || column.Default.Length != 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				{
					method.Append("\n\t\t//" + optionalPropertyName);
                    method.Append("\n\t\tif (!SiteOptions.CheckOption(" + SiteOptionsBuilder.GetFullPropertyPath(optionalFullPropertyName) + "))");
					method.Append("\n\t\t{");
					method.Append("\n\t\t\t" + trPropertyName + ".Visible = false;");
					method.Append("\n\t\t}");
					method.Append("\n\t\t//-----------------------------------");
				}
			}
			method.Append("\n\t}");
			method.Append("\n\t//-----------------------------------------------");
			method.Append("\n\t#endregion");

			return method.ToString();
		}
		#endregion

		public void BuildDropDownListsLoaderMethod(string parentTableName, string text, string value, out string ddl, out string methodName)
		{

            string programatlyTableName = Globals.GetProgramatlyName(parentTableName);
			ddl = "ddl" + programatlyTableName;
            string listID = programatlyTableName + "List";
            string entityClassID = programatlyTableName + "Entity";
			methodName = "Load_" + ddl + "()";
			DropDownListsLoader.Append("\n\t#region --------------" + methodName + "--------------");
			DropDownListsLoader.Append("\n\t//---------------------------------------------------------");
			DropDownListsLoader.Append("\n\t//Load_" + ddl);
			DropDownListsLoader.Append("\n\t//---------------------------------------------------------");
			DropDownListsLoader.Append("\n\tprotected void " + methodName);
			DropDownListsLoader.Append("\n\t{");
            //---------------------------------------------------------
            bool hasIsAvailable = SqlProvider.CheckIsATableHasIsAvailableColumnName(parentTableName);
            if (hasIsAvailable)
            {
                DropDownListsLoader.Append("\n\t\tList<" + entityClassID + "> " + listID + " = " + programatlyTableName + "Factory.GetAllForAdmin();");
            }
            else
            {
                bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(parentTableName));

                if (isMaultiLanguages)
                {
                    DropDownListsLoader.Append("\n\t\tLanguages langID = (Languages)ResourceManager.GetCurrentLanguage();");
                    DropDownListsLoader.Append("\n\t\tList<" + entityClassID + "> " + listID + " = " + programatlyTableName + "Factory.GetAll(langID);");
                }
                else
                {
                    DropDownListsLoader.Append("\n\t\tList<" + entityClassID + "> " + listID + " = " + programatlyTableName + "Factory.GetAll();");
                }
            }
            //---------------------------------------------------------
            DropDownListsLoader.Append("\n\t\tif (" + listID + " != null && " + listID + ".Count > 0)");
			DropDownListsLoader.Append("\n\t\t{");
            DropDownListsLoader.Append("\n\t\t\t" + ddl + ".DataSource = " + listID + ";");
			DropDownListsLoader.Append("\n\t\t\t" + ddl + ".DataTextField = \"" + text + "\";");

			DropDownListsLoader.Append("\n\t\t\t" + ddl + ".DataValueField = \"" + value + "\";");
			DropDownListsLoader.Append("\n\t\t\t" + ddl + ".DataBind();");
			DropDownListsLoader.Append("\n\t\t\t" + ddl + ".Items.Insert(0, new ListItem(" + ResourcesTesxtsBuilder.AddAdminGlobalText("Choose", TextType.Text) + ", \"-1\"));");
			DropDownListsLoader.Append("\n\t\t\t" + ddl + ".Enabled = true;");
			DropDownListsLoader.Append("\n\t\t}");
			DropDownListsLoader.Append("\n\t\telse");
			DropDownListsLoader.Append("\n\t\t{");
            DropDownListsLoader.Append("\n\t\t\t" + ddl + ".Items.Clear();");
            DropDownListsLoader.Append("\n\t\t\t" + ddl + ".Items.Insert(0, new ListItem(" + ResourcesTesxtsBuilder.AddAdminGlobalText("ThereIsNoData", TextType.Text) + ", \"-1\"));");
			DropDownListsLoader.Append("\n\t\t\t" + ddl + ".Enabled = false;");
			DropDownListsLoader.Append("\n\t\t}");

			DropDownListsLoader.Append("\n\t}");
			DropDownListsLoader.Append("\n\t//--------------------------------------------------------");
			DropDownListsLoader.Append("\n\t#endregion");
			//DropDownListsLoaderMethods.Add(methodName);

		}
		//
		public Create_CodeBehindBuilder(InterfaceType type)
		{
			Type = type;
			ClassName = global.CreateCodeBehindClass;
		}

		private string GeneratePageLoad()
		{
			string pageLoadBody = "\n\t\tlblResult.Text=\"\";";
			pageLoadBody += "\n\t\tif(!IsPostBack)";
			pageLoadBody += "\n\t\t{";
            if (ProjectBuilder.HasConfiguration)
            {
                pageLoadBody+="\n\t\t\tSiteOptions.CheckModuleWithHandling(Resources.SiteOptions." + SiteOptionsBuilder.GetHasPropertyString(Table) + ", ViewerType.Admin);";
            }
			pageLoadBody += "\n\t\t\tLoadData();";
			pageLoadBody += "\n\t\t}";
			return GeneratePageLoadHandler(pageLoadBody);

		}
        private void CreateLoadPriorities()
        {

            LoadPrioritesLoader.Append("\n\t#region ---------------LoadPriorities---------------");
            LoadPrioritesLoader.Append("\n\t//-----------------------------------------------");
            LoadPrioritesLoader.Append("\n\t//LoadPriorities");
            LoadPrioritesLoader.Append("\n\t//-----------------------------------------------");
            LoadPrioritesLoader.Append("\n\tprotected void LoadPriorities()");
            LoadPrioritesLoader.Append("\n\t{");
            string getCountMethod = "GetCount()";
            bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(SqlProvider.obj.TableName));
            if (isMaultiLanguages)
            {
                getCountMethod = "GetCountForAdmin()";
            }
            LoadPrioritesLoader.Append("\n\t\tint itemsCount = " + global.TableFactoryClass + "." + getCountMethod + "();");

            LoadPrioritesLoader.Append("\n\t\tOurDropDownList.LoadPriorities(ddlPriority, itemsCount, true);");
            LoadPrioritesLoader.Append("\n\t}");
            LoadPrioritesLoader.Append("\n\t//-----------------------------------------------");
            LoadPrioritesLoader.Append("\n\t#endregion");
            
        }
		private string CreateLoadData()
		{
			StringBuilder loadDataStart = new StringBuilder();
			StringBuilder loadDataChecks = new StringBuilder();
			StringBuilder loadDataBody = new StringBuilder();
			StringBuilder loadDataEnd = new StringBuilder();
			string id = Globals.GetProgramatlyName(ID.Name);
			id = Globals.ConvetStringToCamelCase(id);
			loadDataStart.Append("\n\t#region ---------------LoadData---------------");
			loadDataStart.Append("\n\t//-----------------------------------------------");
			loadDataStart.Append("\n\t//LoadData");
			loadDataStart.Append("\n\t//-----------------------------------------------");
			loadDataStart.Append("\n\tprotected void LoadData()");
			loadDataStart.Append("\n\t{");
			if (ProjectBuilder.HasConfiguration)
			{
				loadDataStart.Append("\n\t\tHandleOptionalControls();");
			}
			string datatype;
			foreach (SQLDMO.Column column in Fields)
			{


				//if ((ID == null || column.Name != ID.Name) && (column.Default == null || column.Default.Length != 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				{
					TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
					datatype = Globals.GetAliasDataType(column.Datatype);
                    //Check Priority
                    if (column.Name.ToLower() == ProjectBuilder.PriorityColumnName.ToLower())
                    {
                        CreateLoadPriorities();
                        loadDataBody.Append("\n\t\tLoadPriorities();");
                    }
					else if (datatype == "string")
					{
						if (cnstr != null)
						{
							string text = SqlProvider.obj.GetExpectedNameForParent(cnstr.ParentTable);
							string _value = Globals.GetProgramatlyName(cnstr.ParentColID);
							string ddl;
							string methodName;
							BuildDropDownListsLoaderMethod(cnstr.ParentTable, text, _value, out ddl, out methodName);
							loadDataBody.Append("\n\t\t" + methodName + ";");
						}

					}
					else if (datatype != "byte[]" && datatype != "Object" && datatype != "Guid")
					{
						if (cnstr != null)
						{
							string text = SqlProvider.obj.GetExpectedNameForParent(cnstr.ParentTable);
							string _value = Globals.GetProgramatlyName(cnstr.ParentColID);
							string ddl;
							string methodName;
							BuildDropDownListsLoaderMethod(cnstr.ParentTable, text, _value, out ddl, out methodName);
							loadDataBody.Append("\n\t\t" + methodName + ";");
						}
					}

				}
			}
			loadDataEnd.Append("\n\t}");
			loadDataEnd.Append("\n\t//-----------------------------------------------");
			loadDataEnd.Append("\n\t#endregion");

			return loadDataStart.ToString() + loadDataChecks.ToString() + loadDataBody.ToString() + loadDataEnd.ToString();
		}
		//
		private string Create_CreateButtonHandler()
		{
			StringBuilder _CreateButtonHandler = new StringBuilder();
            _CreateButtonHandler.Append("\n\t#region ---------------Create---------------");
            //ibtnCreateThenNew
            _CreateButtonHandler.Append("\n\t//-----------------------------------------------");
            _CreateButtonHandler.Append("\n\t//ibtnCreateThenNew");
            _CreateButtonHandler.Append("\n\t//-----------------------------------------------");
            _CreateButtonHandler.Append("\n\tprotected void ibtnCreateThenNew_Click(object sender, ImageClickEventArgs e)");
            _CreateButtonHandler.Append("\n\t{");
            _CreateButtonHandler.Append("\n\t\tCreate(UICreateOperation.NewAfterCreate);");
            _CreateButtonHandler.Append("\n\t}");
            _CreateButtonHandler.Append("\n\t//-----------------------------------------------");
            //ibtnCreateThenFinish event handler
            _CreateButtonHandler.Append("\n");
            //_CreateButtonHandler.Append("\n\t//-----------------------------------------------");
            _CreateButtonHandler.Append("\n\t//ibtnCreateThenFinish");
            _CreateButtonHandler.Append("\n\t//-----------------------------------------------");
            _CreateButtonHandler.Append("\n\tprotected void ibtnCreateThenFinish_Click(object sender, ImageClickEventArgs e)");
            _CreateButtonHandler.Append("\n\t{");
            _CreateButtonHandler.Append("\n\t\tCreate(UICreateOperation.FinishAfterCreate);");
            _CreateButtonHandler.Append("\n\t}");
            _CreateButtonHandler.Append("\n\t//-----------------------------------------------");
            _CreateButtonHandler.Append("\n");
            //Create method
            //_CreateButtonHandler.Append("\n\t//-----------------------------------------------");
            _CreateButtonHandler.Append("\n\t//Create");
			_CreateButtonHandler.Append("\n\t//-----------------------------------------------");
            _CreateButtonHandler.Append("\n\tprotected void Create(UICreateOperation operation)");
			_CreateButtonHandler.Append("\n\t{");
			_CreateButtonHandler.Append("\n\t\tif (!Page.IsValid)");
			_CreateButtonHandler.Append("\n\t\t{");
			_CreateButtonHandler.Append("\n\t\t\treturn;");
			_CreateButtonHandler.Append("\n\t\t}");
			_CreateButtonHandler.Append("\n\t\t" + global.TableEntityClass + " " + global.EntityClassObject + " = new " + global.TableEntityClass + "();");
			string datatype;
			foreach (SQLDMO.Column column in Fields)
			{
				//if ((ID == null || column.Name != ID.Name) && (column.Default == null || column.Default.Length != 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0))
				{
					TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
					datatype = Globals.GetAliasDataType(column.Datatype);
                    if (column.Name.ToLower() == ProjectBuilder.PriorityColumnName.ToLower())
                    {
                        if (ProjectBuilder.HasConfiguration)
                        {
                            _CreateButtonHandler.Append("\n\t\tif (tr" + Globals.GetProgramatlyName(column.Name) + ".Visible)");
                            _CreateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " =Convert.To" + Globals.GetDataType(column.Datatype) + "(ddlPriority.SelectedValue);");

                        }
                        else
                        {
                            _CreateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " =Convert.To" + Globals.GetDataType(column.Datatype) + "(ddlPriority.SelectedValue);");

                        }
                    }
                    else if (datatype == "string")
                    {
                        if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.NText && column.Name.ToLower().IndexOf("details") > -1)
                        {
                            if (ProjectBuilder.IsFreeTextBoxEditor)
                            _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = txt" + Globals.GetProgramatlyName(column.Name) + ".Text;");
                            else
                                _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = txt" + Globals.GetProgramatlyName(column.Name) + ".Value;");

                        }
                        else
                        {
                            if (cnstr == null)
                            {
                                if (column.Name == ProjectBuilder.PhotoExtensionColumnName)
                                {
                                    hasPhoto = true;
                                    _CreateButtonHandler.Append("\n\t\t//-------------");
                                    //_CreateButtonHandler.Append("\n\t\tstring " + ProjectBuilder.PhotoExtensionColumnNameVariable + " = (string)ViewState[\"" + ProjectBuilder.PhotoExtensionColumnName + "\"];");
                                    _CreateButtonHandler.Append("\n\t\tif (" + photoUploadreID + ".HasFile)");
                                    _CreateButtonHandler.Append("\n\t\t{");
                                    _CreateButtonHandler.Append("\n\t\t\tif (!MoversFW.Photos.CheckIsImage(" + photoUploadreID + ".PostedFile))");
                                    _CreateButtonHandler.Append("\n\t\t\t{");
                                    _CreateButtonHandler.Append("\n\t\t\t\tlblResult.ForeColor = Color.Red;");
                                    _CreateButtonHandler.Append("\n\t\t\t\tlblResult.Text = " + ResourcesTesxtsBuilder.AddAdminGlobalText("InvalidPhotoFile", TextType.Text) + ";");
                                    _CreateButtonHandler.Append("\n\t\t\t\treturn;");
                                    _CreateButtonHandler.Append("\n\t\t\t}");
                                    _CreateButtonHandler.Append("\n\t\t}");
                                    _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Path.GetExtension(" + photoUploadreID + ".FileName);");
                                    //_CreateButtonHandler.Append("\n\t\telse");
                                    //_CreateButtonHandler.Append("\n\t\t{");
                                    //_CreateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = " + ProjectBuilder.PhotoExtensionColumnNameVariable + ";");
                                    //_CreateButtonHandler.Append("\n\t\t}");
                                    _CreateButtonHandler.Append("\n\t\t//-----------------------------------------------------------------");

                                }
                                else if (column.Name == "LogoExtension")
                                {
                                    hasLogo = true;

                                    _CreateButtonHandler.Append("\n\t\t//-------------");
                                    //_CreateButtonHandler.Append("\n\t\tstring " + ProjectBuilder.LogoExtensionColumnNameVariable + " = (string)ViewState[\"" + ProjectBuilder.LogoExtensionColumnName + "\"];");
                                    _CreateButtonHandler.Append("\n\t\tif (" + logoUploadreID + ".HasFile)");
                                    _CreateButtonHandler.Append("\n\t\t{");
                                    _CreateButtonHandler.Append("\n\t\t\tif (!MoversFW.Photos.CheckIsImage(" + logoUploadreID + ".PostedFile))");
                                    _CreateButtonHandler.Append("\n\t\t\t{");
                                    _CreateButtonHandler.Append("\n\t\t\t\tlblResult.ForeColor = Color.Red;");
                                    _CreateButtonHandler.Append("\n\t\t\t\tlblResult.Text = " + ResourcesTesxtsBuilder.AddAdminGlobalText("InvalidLogoFile", TextType.Text) + ";");
                                    _CreateButtonHandler.Append("\n\t\t\t\treturn;");
                                    _CreateButtonHandler.Append("\n\t\t\t}");
                                    _CreateButtonHandler.Append("\n\t\t}");
                                    _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Path.GetExtension(" + logoUploadreID + ".FileName);");
                                    //_CreateButtonHandler.Append("\n\t\telse");
                                    //_CreateButtonHandler.Append("\n\t\t{");
                                    //_CreateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = " + ProjectBuilder.LogoExtensionColumnNameVariable + ";");
                                    //_CreateButtonHandler.Append("\n\t\t}");
                                    _CreateButtonHandler.Append("\n\t\t//-----------------------------------------------------------------");

                                }
                                else if (column.Name == "FileExtension")
                                {
                                    hasFile = true;

                                    _CreateButtonHandler.Append("\n\t\t//-------------");
                                    //_CreateButtonHandler.Append("\n\t\tstring " + ProjectBuilder.FileExtensionColumnNameVariable + " = (string)ViewState[\"" + ProjectBuilder.FileExtensionColumnName + "\"];");
                                    _CreateButtonHandler.Append("\n\t\tstring uploadedFilePath = Path.GetExtension(" + fileUploadreID + ".FileName);");
                                    _CreateButtonHandler.Append("\n\t\tif (" + fileUploadreID + ".HasFile)");
                                    _CreateButtonHandler.Append("\n\t\t{");
                                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                                    _CreateButtonHandler.Append("\n\t\t\t//Check suported extention");
                                    _CreateButtonHandler.Append("\n\t\t\tif (!SiteSettings.CheckUploadedFileExtension(uploadedFilePath, " + SiteSettingsBuilder.AddFileExtensions(Table + "File") + "))");
                                    _CreateButtonHandler.Append("\n\t\t\t{");
                                    _CreateButtonHandler.Append("\n\t\t\t\tlblResult.ForeColor = Color.Red;");
                                    _CreateButtonHandler.Append("\n\t\t\t\tlblResult.Text = " + ResourcesTesxtsBuilder.AddAdminGlobalText("NotSuportedFileExtention", TextType.Text) + ";");
                                    _CreateButtonHandler.Append("\n\t\t\t\treturn;");
                                    _CreateButtonHandler.Append("\n\t\t\t}");
                                    _CreateButtonHandler.Append("\n\t\t\t//Check max length");
                                    _CreateButtonHandler.Append("\n\t\t\tif (!SiteSettings.CheckUploadedFileLength(" + fileUploadreID + ".PostedFile.ContentLength," + SiteSettingsBuilder.AddFileMaxLength(Table + "File") + "))");
                                    _CreateButtonHandler.Append("\n\t\t\t{");
                                    _CreateButtonHandler.Append("\n\t\t\t\tlblResult.ForeColor = Color.Red;");
                                    _CreateButtonHandler.Append("\n\t\t\t\tlblResult.Text = " + ResourcesTesxtsBuilder.AddAdminGlobalText("UploadedFileGreaterThanMaxLength", TextType.Text) + ";");
                                    _CreateButtonHandler.Append("\n\t\t\t\treturn;");
                                    _CreateButtonHandler.Append("\n\t\t\t}");
                                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                                    //_CreateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Path.GetExtension(" + fileUploadreID + ".FileName);");
                                    _CreateButtonHandler.Append("\n\t\t}");
                                    _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = uploadedFilePath;");
                                    //_CreateButtonHandler.Append("\n\t\telse");
                                    //_CreateButtonHandler.Append("\n\t\t{");
                                    //_CreateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = " + ProjectBuilder.FileExtensionColumnNameVariable + ";");
                                    //_CreateButtonHandler.Append("\n\t\t}");
                                    _CreateButtonHandler.Append("\n\t\t//-----------------------------------------------------------------");
                                }
                                else if (column.Name.IndexOf("Extension") > -1)
                                {
                                    hasFile = true;
                                    string[] stringSeparators = new string[] { "Extension" };
                                    string[] separatingResult = column.Name.Split(stringSeparators, StringSplitOptions.None);
                                    string propertyName = separatingResult[0];
                                    string uploaderID = "fu" + propertyName;
                                    string columnNamevariable = Globals.ConvetStringToCamelCase(Globals.GetProgramatlyName(column.Name));

                                    _CreateButtonHandler.Append("\n\t\t//-------------");
                                    //_CreateButtonHandler.Append("\n\t\tstring " + columnNamevariable + " = (string)ViewState[\"" + Globals.GetProgramatlyName(column.Name) + "\"];");
                                    _CreateButtonHandler.Append("\n\t\tstring uploaded" + propertyName + "Extension = Path.GetExtension(" + uploaderID + ".FileName);");
                                    _CreateButtonHandler.Append("\n\t\tif (" + uploaderID + ".HasFile)");
                                    _CreateButtonHandler.Append("\n\t\t{");
                                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                                    _CreateButtonHandler.Append("\n\t\t\t//Check suported extention");
                                    _CreateButtonHandler.Append("\n\t\t\tif (!SiteSettings.CheckUploadedFileExtension(uploaded" + propertyName + "Extension, " + SiteSettingsBuilder.AddFileExtensions(Table + propertyName) + "))");
                                    _CreateButtonHandler.Append("\n\t\t\t{");
                                    _CreateButtonHandler.Append("\n\t\t\t\tlblResult.ForeColor = Color.Red;");
                                    _CreateButtonHandler.Append("\n\t\t\t\tlblResult.Text = " + ResourcesTesxtsBuilder.AddUserText("NotSuported" + propertyName + "Extention", TextType.Text) + ";");
                                    _CreateButtonHandler.Append("\n\t\t\t\treturn;");
                                    _CreateButtonHandler.Append("\n\t\t\t}");
                                    _CreateButtonHandler.Append("\n\t\t\t//Check max length");
                                    _CreateButtonHandler.Append("\n\t\t\tif (!SiteSettings.CheckUploadedFileLength(" + uploaderID + ".PostedFile.ContentLength," + SiteSettingsBuilder.AddFileMaxLength(Table + propertyName) + "))");
                                    _CreateButtonHandler.Append("\n\t\t\t{");
                                    _CreateButtonHandler.Append("\n\t\t\t\tlblResult.ForeColor = Color.Red;");
                                    _CreateButtonHandler.Append("\n\t\t\t\tlblResult.Text = " + ResourcesTesxtsBuilder.AddUserText("Uploaded" + propertyName + "GreaterThanMaxLength", TextType.Text) + ";");
                                    _CreateButtonHandler.Append("\n\t\t\t\treturn;");
                                    _CreateButtonHandler.Append("\n\t\t\t}");
                                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                                    //_CreateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Path.GetExtension(" + uploaderID + ".FileName);");
                                    _CreateButtonHandler.Append("\n\t\t}");
                                    _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = uploaded" + propertyName + "Extension;");
                                    //_CreateButtonHandler.Append("\n\t\telse");
                                    //_CreateButtonHandler.Append("\n\t\t{");
                                    //_CreateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = " + columnNamevariable + ";");
                                    //_CreateButtonHandler.Append("\n\t\t}");
                                    _CreateButtonHandler.Append("\n\t\t//-----------------------------------------------------------------");
                                }
                                else if (column.Name.IndexOf("Date") > -1)
                                {
                                    string x = column.Default;
                                }
                                else if (column.Name.ToLower() == ProjectBuilder.LangID)
                                {
                                    _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = (Languages)ResourceManager.GetCurrentLanguage();");
                                }
                                else
                                {
                                    _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = txt" + Globals.GetProgramatlyName(column.Name) + ".Text;");
                                }

                            }
                            else
                                _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = ddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + ".SelectedValue;");
                        }
                    }
                    else if (datatype == "bool")
                        _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = cb" + Globals.GetProgramatlyName(column.Name) + ".Checked;");
                    else if (datatype == "Guid")
                        _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Guid.NewGuid();");
                    else if (datatype != "byte[]" && datatype != "Object")
                    {
                        if (cnstr == null)
                        {
                            if (column.Name.ToLower() == ProjectBuilder.LangID)
                            {
                                _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = (Languages)ResourceManager.GetCurrentLanguage();");
                            }
                            else
                            {
                                if (ProjectBuilder.HasConfiguration)
                                {
                                    _CreateButtonHandler.Append("\n\t\tif (tr" + Globals.GetProgramatlyName(column.Name) + ".Visible)");
                                    _CreateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Convert.To" + Globals.GetDataType(column.Datatype) + "(txt" + Globals.GetProgramatlyName(column.Name) + ".Text);");
                                }
                                else
                                {
                                    _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Convert.To" + Globals.GetDataType(column.Datatype) + "(txt" + Globals.GetProgramatlyName(column.Name) + ".Text);");
                                } 
                            }
                        }
                        else
                        {
                            if (ProjectBuilder.HasConfiguration)
                            {
                                _CreateButtonHandler.Append("\n\t\tif (tr" + Globals.GetProgramatlyName(column.Name) + ".Visible)");
                                _CreateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Convert.To" + Globals.GetDataType(column.Datatype) + "(ddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + ".SelectedValue);");
                            }
                            else
                            {
                                _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Convert.To" + Globals.GetDataType(column.Datatype) + "(ddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + ".SelectedValue);");
                            }
                           
                        }
                    }
				}
			}
            if (!ProjectBuilder.ISExcuteScaler)
            {
                _CreateButtonHandler.Append("\n\t\tbool status = " + global.TableFactoryClass + "." + MethodType.Create.ToString() + "(" + global.EntityClassObject + ");");
                _CreateButtonHandler.Append("\n\t\tif(status)");
            }
            else
            {
                _CreateButtonHandler.Append("\n\t\tExecuteCommandStatus status = " + global.TableFactoryClass + "." + MethodType.Create.ToString() + "(" + global.EntityClassObject + ");");
                _CreateButtonHandler.Append("\n\t\tif(status == ExecuteCommandStatus.Done)");
            }
			_CreateButtonHandler.Append("\n\t\t{");
			if (hasPhoto || hasLogo || hasFile)
			{
				_CreateButtonHandler.Append("\n\t\t\t//--------------------------------");
				_CreateButtonHandler.Append("\n\t\t\tSiteSettings settings = SiteSettings.Instance; ;");
				_CreateButtonHandler.Append("\n\t\t\t"+SiteUrlsBuilder.GetFullDeclaration());
				_CreateButtonHandler.Append("\n\t\t\t//");
				_CreateButtonHandler.Append("\n\t\t\t//--------------------------------");
			}
			//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
			#region XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
			foreach (SQLDMO.Column column in Fields)
			{
				//if ((ID == null || column.Name != ID.Name) && (column.Default == null || column.Default.Length != 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				{
					TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
					datatype = Globals.GetAliasDataType(column.Datatype);

					if (datatype == "string")
					{

						if (cnstr == null)
						{
							if (column.Name == ProjectBuilder.PhotoExtensionColumnName)
							{
								_CreateButtonHandler.Append("\n\t\t\t//Photo-----------------------------");
								_CreateButtonHandler.Append("\n\t\t\tif (" + photoUploadreID + ".HasFile)");
								_CreateButtonHandler.Append("\n\t\t\t{");
								//Add Urls Property
								SiteUrlsBuilder.AddDirectoryUrl( "OriginalPhotos", SiteUrlsBuilder.photoOriginalUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "MicroPhotoThumbs", SiteUrlsBuilder.photoMicroUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "MiniPhotoThumbs", SiteUrlsBuilder.photoMiniUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "NormalPhotoThumbs", SiteUrlsBuilder.photoNormalUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "BigPhotoThumbs", SiteUrlsBuilder.photoBigUrl, Globals.GetProgramatlyName(Table), "");
								_CreateButtonHandler.Append("\n\t\t\t\t//------------------------------------------------");
								_CreateButtonHandler.Append("\n\t\t\t\t//Save new original photo");
								_CreateButtonHandler.Append("\n\t\t\t\t" + photoUploadreID + ".PostedFile.SaveAs(Server.MapPath(urls.OriginalPhotos) + " + global.EntityClassObject + ".Photo);");
								_CreateButtonHandler.Append("\n\t\t\t\t//Create new thumbnails");
                                _CreateButtonHandler.Append("\n\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.MicroPhotoThumbs, " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + photoUploadreID + ".PostedFile, settings.MicroThumnailWidth, settings.MicroThumnailHeight);");
								_CreateButtonHandler.Append("\n\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.MiniPhotoThumbs, " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + photoUploadreID + ".PostedFile, settings.MiniThumnailWidth, settings.MiniThumnailHeight);");
								_CreateButtonHandler.Append("\n\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.NormalPhotoThumbs, " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + photoUploadreID + ".PostedFile, settings.NormalThumnailWidth, settings.NormalThumnailHeight);");
								_CreateButtonHandler.Append("\n\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.BigPhotoThumbs, " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + photoUploadreID + ".PostedFile, settings.BigThumnailWidth, settings.BigThumnailHeight);");
								_CreateButtonHandler.Append("\n\t\t\t}");

							}
							else if (column.Name == "LogoExtension")
							{
								_CreateButtonHandler.Append("\n\t\t\t//Logo-----------------------------");
								_CreateButtonHandler.Append("\n\t\t\tif (" + logoUploadreID + ".HasFile)");
								_CreateButtonHandler.Append("\n\t\t\t{");
								//Add Urls Property
								SiteUrlsBuilder.AddDirectoryUrl( "OriginalLogos", SiteUrlsBuilder.logoOriginalUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "MicroLogoThumbs", SiteUrlsBuilder.logoMicroUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "MiniLogoThumbs", SiteUrlsBuilder.logoMiniUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "NormalLogoThumbs", SiteUrlsBuilder.logoNormalUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "BigLogoThumbs", SiteUrlsBuilder.logoBigUrl, Globals.GetProgramatlyName(Table), "");
								_CreateButtonHandler.Append("\n\t\t\t\t//Save new original Logo");
                                _CreateButtonHandler.Append("\n\t\t\t\t" + logoUploadreID + ".PostedFile.SaveAs(Server.MapPath(urls.OriginalLogos) + " + global.EntityClassObject + ".Logo);");
								_CreateButtonHandler.Append("\n\t\t\t\t//Create new thumbnails");
								_CreateButtonHandler.Append("\n\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.MicroLogoThumbs, " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + logoUploadreID + ".PostedFile, settings.MicroThumnailWidth, settings.MicroThumnailHeight);");
								_CreateButtonHandler.Append("\n\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.MiniLogoThumbs, " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + logoUploadreID + ".PostedFile, settings.MiniThumnailWidth, settings.MiniThumnailHeight);");
								_CreateButtonHandler.Append("\n\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.NormalLogoThumbs, " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + logoUploadreID + ".PostedFile, settings.NormalThumnailWidth, settings.NormalThumnailHeight);");
								_CreateButtonHandler.Append("\n\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.BigLogoThumbs, " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + logoUploadreID + ".PostedFile, settings.BigThumnailWidth, settings.BigThumnailHeight);");
								_CreateButtonHandler.Append("\n\t\t\t}");
							}
							else if (column.Name == "FileExtension")
							{
								_CreateButtonHandler.Append("\n\t\t\t//File-----------------------------");
								_CreateButtonHandler.Append("\n\t\t\tif (" + fileUploadreID + ".HasFile)");
								_CreateButtonHandler.Append("\n\t\t\t{");
								//Add Urls Property
								SiteUrlsBuilder.AddDirectoryUrl( "OriginalFiles", SiteUrlsBuilder.filesUrl, Globals.GetProgramatlyName(Table), "");
								_CreateButtonHandler.Append("\n\t\t\t\t//Save new original file");
								_CreateButtonHandler.Append("\n\t\t\t\t" + fileUploadreID + ".PostedFile.SaveAs(Server.MapPath(urls.OriginalFiles) + " + global.EntityClassObject + ".File);");

								_CreateButtonHandler.Append("\n\t\t\t}");
							}
							else if (column.Name.IndexOf("Extension") > 0)
							{

								string[] stringSeparators = new string[] { "Extension" };
								string[] separatingResult = column.Name.Split(stringSeparators, StringSplitOptions.None);
								string propertyName = separatingResult[0];
								string uploaderID = "fu" + propertyName;
								_CreateButtonHandler.Append("\n\t\t\t//File-----------------------------");
								_CreateButtonHandler.Append("\n\t\t\tif (" + uploaderID + ".HasFile)");
								_CreateButtonHandler.Append("\n\t\t\t{");
								//Add Urls Property
								SiteUrlsBuilder.AddDirectoryUrl( "Original" + propertyName, SiteUrlsBuilder.otherFilesUrl, Globals.GetProgramatlyName(Table), propertyName + "s");
								_CreateButtonHandler.Append("\n\t\t\t\t//Save new original " + propertyName);
								_CreateButtonHandler.Append("\n\t\t\t\t" + uploaderID + ".PostedFile.SaveAs(Server.MapPath(urls.Original" + propertyName + ") + " + global.EntityClassObject + "." + propertyName + ");");

								_CreateButtonHandler.Append("\n\t\t\t}");
							}

						}
					}

				}
			}
			#endregion
			//------------------------------------------------------------------------
			_CreateButtonHandler.Append("\n\t\t\tlblResult.ForeColor=Color.Blue ;");
			_CreateButtonHandler.Append("\n\t\t\tlblResult.Text=" + ResourcesTesxtsBuilder.AddAdminGlobalText("AddingOperationDone", TextType.Text) + ";");
            _CreateButtonHandler.Append("\n\t\t\tif (operation == UICreateOperation.NewAfterCreate)");
                _CreateButtonHandler.Append("\n\t\t\t\tClearControls();");
            _CreateButtonHandler.Append("\n\t\t\telse");
                _CreateButtonHandler.Append("\n\t\t\t\tResponse.Redirect(\"default.aspx\");");
			_CreateButtonHandler.Append("\n\t\t\tClearControls();");
			_CreateButtonHandler.Append("\n\t\t}");
            if (ProjectBuilder.ISExcuteScaler)
            {
                _CreateButtonHandler.Append("\n\t\telse if (status == ExecuteCommandStatus.AllreadyExists)");
                _CreateButtonHandler.Append("\n\t\t{");
                _CreateButtonHandler.Append("\n\t\t\tlblResult.ForeColor=Color.Red ;");
                _CreateButtonHandler.Append("\n\t\t\tlblResult.Text = Resources.Admin.DuplicateItem;");
                _CreateButtonHandler.Append("\n\t\t}");
            }
			_CreateButtonHandler.Append("\n\t\telse");
			_CreateButtonHandler.Append("\n\t\t{");
			_CreateButtonHandler.Append("\n\t\t\tlblResult.ForeColor=Color.Red ;");
			_CreateButtonHandler.Append("\n\t\t\tlblResult.Text= " + ResourcesTesxtsBuilder.AddAdminGlobalText("AddingOperationFaild", TextType.Text) + ";");
			_CreateButtonHandler.Append("\n\t\t}");
			_CreateButtonHandler.Append("\n\t}");
			_CreateButtonHandler.Append("\n\t//-----------------------------------------------");
			_CreateButtonHandler.Append("\n\t#endregion");
			return _CreateButtonHandler.ToString();
		}
		//----------------------------------------------
		private string CreateClearControlsMethod()
		{
			//
			StringBuilder method = new StringBuilder();
			method.Append("\n\t#region --------------" + global.ClearControlsMethod + "--------------");
			method.Append("\n\t//---------------------------------------------------------");
			method.Append("\n\t//" + global.ClearControlsMethod);
			method.Append("\n\t//---------------------------------------------------------");


			method.Append("\n\tprivate void " + global.ClearControlsMethod);
			method.Append("\n\t{");
			string datatype;
			foreach (SQLDMO.Column column in Fields)
			{
				if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0) && column.Name.ToLower() != ProjectBuilder.LangID && column.Name.IndexOf("Extension") < 0)
				{
					datatype = Globals.GetAliasDataType(column.Datatype);
                    if (column.Name.ToLower() == ProjectBuilder.PriorityColumnName.ToLower())
                    {
                        method.Append("\n\t\tLoadPriorities();");
                    }
                    else
                    {
                        if (datatype == "bool")
                        {
                            if (column.Name.ToLower().IndexOf("isavailable") > -1)
                                method.Append("\n\t\tcb" + Globals.GetProgramatlyName(column.Name) + ".Checked=true;");
                            else
                                method.Append("\n\t\tcb" + Globals.GetProgramatlyName(column.Name) + ".Checked=false;");
                        }
                        else if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.NText && column.Name.ToLower().IndexOf("details") > -1)
                        {
                            if (ProjectBuilder.IsFreeTextBoxEditor)
                                method.Append("\n\t\ttxt" + Globals.GetProgramatlyName(column.Name) + ".Text=\"\";");
                            else
                                method.Append("\n\t\ttxt" + Globals.GetProgramatlyName(column.Name) + ".Value=\"\";");

                        }
                        else if (datatype != "byte[]" && datatype != "Object" && datatype != "Guid")
                        {
                            TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
                            if (cnstr == null)
                            {
                                method.Append("\n\t\ttxt" + Globals.GetProgramatlyName(column.Name) + ".Text=\"\";");
                            }
                            else
                            {
                                method.Append("\n\t\tddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + ".SelectedIndex=-1;");
                            }
                        }
                    }
				}
			}
			method.Append("\n\t}");
			method.Append("\n\t//--------------------------------------------------------");
			method.Append("\n\t#endregion");
			return method.ToString();
		}
		//----------------------------------------------
		public static void Create(InterfaceType type)
		{
			Create_CodeBehindBuilder cr = new Create_CodeBehindBuilder(type);
			cr.CreateTheFile();
		}


		//-------------------------------------
		private string GenerateClassBody()
		{
			string loadData = CreateLoadData();
			StringBuilder classBody = new StringBuilder();
            classBody.Append("\n" + GeneratePageLoad());
            classBody.Append("\n" + DropDownListsLoader.ToString());
            classBody.Append("\n" + LoadPrioritesLoader.ToString());
            if (ProjectBuilder.HasConfiguration)
			{
				classBody.Append("\n" + CreateHandleOptionalControls());
			}
			classBody.Append("\n" + loadData);
			classBody.Append("\n" + Create_CreateButtonHandler());
			classBody.Append("\n" + CreateClearControlsMethod());
			return classBody.ToString();
		}
		//
		private void CreateTheFile()
		{
			DirectoryInfo dInfo;
			string path;
			try
			{
				if (Type == InterfaceType.WEbUserControl)
				{
					dInfo = Directory.CreateDirectory(Globals.UserControlsDirectory + global.TableProgramatlyName);
					path = dInfo.FullName + "\\" + ClassName + ".ascx.cs";
				}
				else
				{
					//
					string directoryPath = Globals.AdminFolder + global.TableProgramatlyName + "\\";
					path = directoryPath + "\\Add.aspx.cs";
					DirectoriesManager.ChechDirectory(directoryPath);
				}
				// Create the file.
				string _class = GenerateClass(GenerateClassBody());
				FileManager.CreateFile(path, _class);
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);

			}
		}

	}
}
