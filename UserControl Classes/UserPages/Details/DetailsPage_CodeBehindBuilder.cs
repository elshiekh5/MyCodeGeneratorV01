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
	public class DetailsPage_CodeBehindBuilder  : CodeBehindBuilder
	{
		//----------------------------------
		string boxHeader;
		string masterBoxID;
		//----------------------------------
		public DetailsPage_CodeBehindBuilder()
		{
			global =new Globals();
			ClassName = "Details";
			boxHeader = global.TableProgramatlyName + "DetailsBoxHeader";
			masterBoxID = "mb" + global.TableProgramatlyName;
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
		public static void Create()
		{
			DetailsPage_CodeBehindBuilder cr = new DetailsPage_CodeBehindBuilder();
			cr.CreatePageFile();
		}
		//--------------------------------------
        //
		protected string GenerateClass()
		{
			string id = Globals.GetProgramatlyName(ID.Name);
			id = Globals.ConvetStringToCamelCase(id);
			string headerProperity="";
			string boxHeader="";
			if (Fields.Count >= 2)
			{
				SQLDMO.Column headerColumn =(SQLDMO.Column) Fields.Item(1);

				headerProperity = Globals.GetProgramatlyName(headerColumn.Name);
				boxHeader = global.EntityClassObject + "." + headerProperity;
			}
			else
			{
				boxHeader = "\"\"";
			}
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
            pageCode.Append("\npublic partial class Details : System.Web.UI.Page");
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
                pageCode.Append("\n\t\t\tSiteOptions.CheckModuleWithHandling(Resources.SiteOptions." + SiteOptionsBuilder.GetHasPropertyString(Table) + ", ViewerType.User);");
            }
            pageCode.Append("\n\t\t\t//Set Boxes Headers");
			pageCode.Append("\n\t\t\tSetBoxesHeaders();");
			pageCode.Append("\n\t\t}");
			pageCode.Append("\n\t}");
			pageCode.Append("\n\t//--------------------------------------------------------");
			pageCode.Append("\n\t#endregion");
			pageCode.Append("\n\t");
			pageCode.Append("\n\t#region Set Boxes Headers");
			pageCode.Append("\n\t//--------------------------------------------------------");
			pageCode.Append("\n\t//Set Boxes Headers");
			pageCode.Append("\n\t//--------------------------------------------------------");
			pageCode.Append("\n\tprotected void SetBoxesHeaders()");
			pageCode.Append("\n\t{");
			SiteUrlsBuilder.AddParameter(Globals.GetProgramatlyName(ID.Name));
			pageCode.Append("\n\t\tif(MoversFW.Components.UrlManager.ChechIsValidIntegerParameter("+SiteUrlsBuilder.GetIdentifire() + Globals.GetProgramatlyName(ID.Name) + "))");
			pageCode.Append("\n\t\t{");
			pageCode.Append("\n\t\t\t" + Globals.GetAliasDataType(ID.Datatype) + " " + id + " = Convert.To" + Globals.GetDataType(ID.Datatype) + "(Request.QueryString["+SiteUrlsBuilder.GetIdentifire() + Globals.GetProgramatlyName(ID.Name) + "]);");

			pageCode.Append("\n\t\t\t" + global.TableEntityClass + " " + global.EntityClassObject + " =" + global.TableFactoryClass + ".Get" + global.TableProgramatlyName + "Object(" + id + ");");
			pageCode.Append("\n\t\t\tif (" + global.EntityClassObject + " != null)");
			pageCode.Append("\n\t\t\t{");
			pageCode.Append("\n\t\t\t\t" + masterBoxID + ".Header = " + boxHeader + ";");
			pageCode.Append("\n\t\t\t}");
			
			pageCode.Append("\n\t\t\telse");
			pageCode.Append("\n\t\t\t{");
			pageCode.Append("\n\t\t\t\tResponse.Redirect(\"/default.aspx\");");
			pageCode.Append("\n\t\t\t}");
			pageCode.Append("\n\t\t}");
			pageCode.Append("\n\t\telse");
			pageCode.Append("\n\t\t{");
			pageCode.Append("\n\t\t\tResponse.Redirect(\"/default.aspx\");");
			pageCode.Append("\n\t\t}");
			
			pageCode.Append("\n\t}");
			pageCode.Append("\n\t//--------------------------------------------------------");
			pageCode.Append("\n\t#endregion");
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
