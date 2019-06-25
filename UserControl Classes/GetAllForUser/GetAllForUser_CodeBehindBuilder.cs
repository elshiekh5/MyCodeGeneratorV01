using System;
using System.IO;
using System.Text ;
using System.Windows.Forms;
using System.Collections;
namespace SPGen
{
	/// <summary>
	/// Summary description for Ceate_CodeBehindBuilder.
	/// </summary>
	public class GetAllForUser_CodeBehindBuilder  
	{
		//--------------------------------------
		Globals global;
		string ClassName;
		string repeaterID;
		string repeaterTemplateID;
		//----------------------------------
		public GetAllForUser_CodeBehindBuilder()
		{
			global =new Globals(); 
			ClassName = global.TableProgramatlyName + "_" + "GetAllForUser";
			repeaterID = "dl" + global.TableProgramatlyName;
			repeaterTemplateID = repeaterID + "Template";
        }
        //----------------------------------
		private string GeneratePageLoad()
		{
			StringBuilder pageLoad = new StringBuilder();
			pageLoad.Append("\n\t#region ---------------Page_Load---------------");
			pageLoad.Append("\n\t//-----------------------------------------------");
			pageLoad.Append("\n\t//Page_Load");
			pageLoad.Append("\n\t//-----------------------------------------------");
			pageLoad.Append("\n\tprivate void Page_Load(object sender, System.EventArgs e)");
			pageLoad.Append("\n\t{");
			pageLoad.Append("\n\t\tif(!IsPostBack)");
			pageLoad.Append("\n\t\t{");
			pageLoad.Append("\n\t\t\tLoadData();");
			pageLoad.Append("\n\t\t}");
			pageLoad.Append("\n\t}");
			pageLoad.Append("\n\t//-----------------------------------------------");
			pageLoad.Append("\n\t#endregion");

			return pageLoad.ToString();
		}
		//--------------------------------------
        private string CreateLoadData()
		{
			StringBuilder loadData=new StringBuilder();
			loadData.Append("\n\t#region --------------LoadData--------------");
			loadData.Append("\n\t//---------------------------------------------------------");
			loadData.Append("\n\t//LoadData");
			loadData.Append("\n\t//---------------------------------------------------------");
			

			loadData.Append("\n\tprivate void LoadData()");
			loadData.Append("\n\t{");
			loadData.Append("\n\t\tpager.PageSize = SiteSettings.Instance.DefaultPageSize;");
			loadData.Append("\n\t\tint totalRecords = 0;");
            string methodParameters = "";
            string methodName = StoredProcedureTypes.GetAllWithPager.ToString();
            bool hasIsAvailable = SqlProvider.CheckIsATableHasIsAvailableColumnName(SqlProvider.obj.TableName);
            if (hasIsAvailable)
                methodName += "ForUser";
            bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(SqlProvider.obj.TableName));
            if (isMaultiLanguages)
            {
                if (!hasIsAvailable)
                {
                    loadData.Append("\n\t\tLanguages langID = (Languages)ResourceManager.GetCurrentLanguage();");
                    methodParameters = "langID,";
                }
            }
            methodParameters += "pager.CurrentPage, pager.PageSize, out totalRecords";
            loadData.Append("\n\t\tList<" + global.TableEntityClass + "> " + global.EntityClassList + " = " + global.TableFactoryClass + "." + methodName + "(" + methodParameters + ");");
            loadData.Append("\n\t\tif(" + global.EntityClassList + "!=null&&" + global.EntityClassList + ".Count >0)");
			loadData.Append("\n\t\t{");
            loadData.Append("\n\t\t\t//Load data list design");
            loadData.Append("\n\t\t\tDataList dl;");
            loadData.Append("\n\t\t\tdl = LoadDataList(\"" + repeaterID + "\");");
            loadData.Append("\n\t\t\t" + repeaterID + ".ItemTemplate = dl.ItemTemplate;");
            loadData.Append("\n\t\t\t" + repeaterID + ".HeaderTemplate = dl.HeaderTemplate;");
            loadData.Append("\n\t\t\t" + repeaterID + ".FooterTemplate = dl.FooterTemplate;");
            loadData.Append("\n\t\t\t" + repeaterID + ".ShowHeader = dl.ShowHeader;");
            loadData.Append("\n\t\t\t" + repeaterID + ".ShowFooter = dl.ShowFooter;");
            loadData.Append("\n\t\t\t" + repeaterID + ".Width = dl.Width;");
            loadData.Append("\n\t\t\t" + repeaterID + ".RepeatColumns = dl.RepeatColumns;");
            loadData.Append("\n\t\t\t//-----------------------------------------");
            loadData.Append("\n\t\t\t" + repeaterID + ".DataSource= " + global.EntityClassList + ";");
			loadData.Append("\n\t\t\t" + repeaterID + ".DataBind();");
			loadData.Append("\n\t\t\t" + repeaterID + ".Visible = true;");
			//--------
			loadData.Append("\n\t\t\tpager.Visible = true;");
			loadData.Append("\n\t\t\tpager.TotalRecords = totalRecords;");
			loadData.Append("\n\t\t\tPagerManager.PrepareUserPager(pager);");
			//--------
			
			loadData.Append("\n\t\t}");
			loadData.Append("\n\t\telse");
			loadData.Append("\n\t\t{");
			loadData.Append("\n\t\t\t" + repeaterID + ".Visible=false;");
			loadData.Append("\n\t\t\tpager.Visible = false;");
			loadData.Append("\n\t\t}");
			loadData.Append("\n\t}");
			loadData.Append("\n\t//--------------------------------------------------------");
			loadData.Append("\n\t#endregion");
			return loadData.ToString();
		}
		//--------------------------------------
        private string CreatePageIndexHandler()
		{
            StringBuilder pageIndexHandler=new StringBuilder();
			pageIndexHandler.Append("\n\t#region --------------Pager_PageIndexChang--------------");
			pageIndexHandler.Append("\n\t//---------------------------------------------------------");
			pageIndexHandler.Append("\n\t//Pager_PageIndexChang");
			pageIndexHandler.Append("\n\t//---------------------------------------------------------");
			pageIndexHandler.Append("\n\tprotected void Pager_PageIndexChang()");
			pageIndexHandler.Append("\n\t{");
			pageIndexHandler.Append("\n\t\tLoadData();");
			pageIndexHandler.Append("\n\t}");
			pageIndexHandler.Append("\n\t//--------------------------------------------------------");
			pageIndexHandler.Append("\n\t#endregion"); 
			return pageIndexHandler.ToString();
		}
		//--------------------------------------
		public static void Create()
		{
			GetAllForUser_CodeBehindBuilder cr = new GetAllForUser_CodeBehindBuilder();
			cr.CreateUserControlFile();
		}
		//--------------------------------------
        private string GenerateClassBody()
        {
            StringBuilder classBody = new StringBuilder();
            classBody.Append(GeneratePageLoad());
            classBody.Append("\n" + CreateLoadData());

            classBody.Append("\n" + CreatePageIndexHandler());

            return classBody.ToString();
        }
		//--------------------------------------
		protected string GenerateClass(string classBody)
		{
			string Usingblock = "";
			Usingblock += "using System;\n";
			Usingblock += "using System.Data;\n";
			Usingblock += "using System.Configuration;\n";
			Usingblock += "using System.Collections;\n";
            Usingblock += "using System.Collections.Generic;\n";
			Usingblock += "using System.Web;\n";
			Usingblock += "using System.Web.Security;\n";
			Usingblock += "using System.Web.UI;\n";
			Usingblock += "using System.Web.UI.WebControls;\n";
			Usingblock += "using System.Web.UI.WebControls.WebParts;\n";
			Usingblock += "using System.Web.UI.HtmlControls;\n";
			Usingblock += "using System.Drawing;\n";
			Usingblock += "using System.IO;\n";
			string _class = "";
			_class += Usingblock;
			_class += "\npublic partial class " + ClassName + " : OurTemplateUserControl";
			_class += "\n{" + classBody + "\n}";
			return _class;
		}
		//--------------------------------------
		private void CreateUserControlFile()
		{
			DirectoryInfo dInfo;
			string path;
			try
			{
				dInfo = Directory.CreateDirectory(Globals.UserControlsDirectory + global.TableProgramatlyName);
				path = dInfo.FullName + "\\" + ClassName + ".ascx.cs";
				// Create the file.
                string _class = GenerateClass(GenerateClassBody());
                FileManager.CreateFile(path, _class);
				//CREATE THE WEB FORM    
			}
			catch(Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:"+ex.Message);
				
			}
		}
		//--------------------------------------
		private void CreatePageFile()
		{/*
			DirectoryInfo dInfo;
			string path;
			try
			{
				string directoryPath = Globals.UserControlsDirectory + global.TableProgramatlyName + "\\";
				path = directoryPath + "\\default.aspx.cs";
				DirectoriesManager.ChechDirectory(directoryPath);
				// Create the file.
				string _class = GenerateClass(GenerateClassBody());
				FileManager.CreateFile(path, _class);
				//CREATE THE WEB FORM    
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);

			}*/
		}
		//--------------------------------------
	}
}
