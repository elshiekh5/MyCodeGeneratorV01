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
	public class UserPage_CodeBehindBuilder  
	{
		//----------------------------------
		Globals global;
		string ClassName;
		string boxHeader;
		string masterBoxID;
		//----------------------------------
		public UserPage_CodeBehindBuilder()
		{
			global =new Globals();
			ClassName = "Default";
			boxHeader = global.TableProgramatlyName + "BoxHeader";
			masterBoxID = "mb" + global.TableProgramatlyName;
        }
        //----------------------------------
        //private string GeneratePageLoad()
        //{
        //    StringBuilder pageLoad = new StringBuilder();
        //    pageLoad.Append("\n\t#region ---------------Page_Load---------------");
        //    pageLoad.Append("\n\t//-----------------------------------------------");
        //    pageLoad.Append("\n\t//Page_Load");
        //    pageLoad.Append("\n\t//-----------------------------------------------");
        //    pageLoad.Append("\n\tprivate void Page_Load(object sender, System.EventArgs e)");
        //    pageLoad.Append("\n\t{");
        //    pageLoad.Append("\n\t\tif(!IsPostBack)");
        //    pageLoad.Append("\n\t\t{");
        //    pageLoad.Append("\n\t\t\tLoadData();");
        //    pageLoad.Append("\n\t\t}");
        //    pageLoad.Append("\n\t}");
        //    pageLoad.Append("\n\t//-----------------------------------------------");
        //    pageLoad.Append("\n\t#endregion");

        //    return pageLoad.ToString();
        //}
		//--------------------------------------
		public static void Create()
		{
			UserPage_CodeBehindBuilder cr = new UserPage_CodeBehindBuilder();
			cr.CreatePageFile();
		}
		//--------------------------------------
        //
		protected string GenerateClass()
		{
			StringBuilder pageCode = new StringBuilder();
			pageCode.Append("using System;\n");
			pageCode.Append("using System.Data;\n");
			pageCode.Append("using System.Configuration;\n");
			pageCode.Append("using System.Collections;\n");
			pageCode.Append("using System.Web;\n");
			pageCode.Append("using System.Web.Security;\n");
			pageCode.Append("using System.Web.UI;\n");
			pageCode.Append("using System.Web.UI.WebControls;\n");
			pageCode.Append("using System.Web.UI.WebControls.WebParts;\n");
			pageCode.Append("using System.Web.UI.HtmlControls;\n");
			pageCode.Append("using System.Drawing;\n");
			pageCode.Append("\npublic partial class Default : System.Web.UI.Page");
			pageCode.Append("\n{");
			pageCode.Append("\n\t#region --------------LoadData--------------");
			pageCode.Append("\n\t//---------------------------------------------------------");
			pageCode.Append("\n\t//Page_Load");
			pageCode.Append("\n\t//---------------------------------------------------------");
			pageCode.Append("\n\tprotected void Page_Load(object sender, EventArgs e)");
			pageCode.Append("\n\t{");
            pageCode.Append("\n\t\tif (!IsPostBack)");
            pageCode.Append("\n\t\t{");
            if (ProjectBuilder.HasConfiguration)
            {
                pageCode.Append("\n\t\t\tSiteOptions.CheckModuleWithHandling(Resources.SiteOptions." + SiteOptionsBuilder.GetHasPropertyString(global.TableProgramatlyName) + ", ViewerType.User);");
            }
            pageCode.Append("\n\t\t}");
			pageCode.Append("\n\t}");
			pageCode.Append("\n\t//--------------------------------------------------------");
			pageCode.Append("\n\t#endregion");
            pageCode.Append("\n\t");
			pageCode.Append("\n}");
			return pageCode.ToString();
		}
		//
		private void CreatePageFile()
		{
			DirectoryInfo dInfo;
			string path;
			try
			{
				dInfo = Directory.CreateDirectory(Globals.SiteForms + global.TableProgramatlyName);
				path = dInfo.FullName + "\\" + ClassName + ".aspx.cs";
				// Create the file.
				string _class = GenerateClass();
				FileManager.CreateFile(path, _class);
				//CREATE THE WEB FORM    
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);

			}
		}
		
	}
}
