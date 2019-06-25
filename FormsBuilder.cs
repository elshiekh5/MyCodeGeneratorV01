using System;
using System.Text ;
namespace SPGen
{
	/// <summary>
	/// Summary description for FormsBuilder.
	/// </summary>
	public class FormsBuilder 
	{
		public static string CreateAdminPage(string parentFolder,string userControl)
		{
			
			StringBuilder adminform=new StringBuilder();
			adminform.Append("<%@ Page language=\"c#\" MasterPageFile=\"/App_Design/Admin/Admin.master\" %>");
			adminform.Append("\n <%@ Register TagPrefix=\"uc1\" TagName=\""+userControl+"\" Src=\"../../UserControls/"+parentFolder+"/"+userControl+".ascx\" %>");
			adminform.Append("\n <asp:Content id=\"Content1\" ContentPlaceHolderID=\"BasicContents\" runat=\"server\">");
            adminform.Append("\n\t<uc1:"+userControl+" id=\""+userControl+"\" runat=\"server\"></uc1:"+userControl+">");
            adminform.Append("\n</asp:Content>");		
			return adminform.ToString();
		}
		public static void CreateTheFile(string parentFolder,string userControl)
		{
			string directoryPath=Globals.AdminFolder+parentFolder+"\\";
			string path= directoryPath+userControl+".aspx";
			string contents=CreateAdminPage(parentFolder,userControl);
			DirectoriesManager.ChechDirectory(directoryPath);
			FileManager.CreateFile(path,contents);

		}
	}
}
