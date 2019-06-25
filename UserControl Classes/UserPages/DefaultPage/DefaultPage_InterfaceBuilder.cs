using System;
using System.IO;
using System.Text ;
using System.Windows.Forms;
using System.Collections;
namespace SPGen
{
    /// <summary>
    /// Summary description for UserPage_CodeBehindBuilder.
    /// </summary>
	public class UserDefaultPage_InterfaceBuilder
    {
		//----------------------------------
		Globals global;
		string ClassName;
		string userControl;
		string masterBoxID;
        string boxHeader;
		//----------------------------------
		public UserDefaultPage_InterfaceBuilder()
		{
			global =new Globals();
			ClassName = "Default";
			userControl = global.TableProgramatlyName + "_" + "GetAllForUser";
			masterBoxID = "mb" + global.TableProgramatlyName;
            boxHeader = global.TableProgramatlyName + "BoxHeader";
        }
        //----------------------------------
		private string GenerateHtml()
		{
			StringBuilder controls = new StringBuilder();
			controls.Append("<%@ Page Language=\"C#\" MasterPageFile=\"/App_Design/User/MasterPages/Site.master\" AutoEventWireup=\"true\" CodeFile=\"Default.aspx.cs\" Inherits=\"Default\"  %>");
            controls.Append("\n<%@ Register Assembly=\"MoversFW\" Namespace=\"MoversFW\" TagPrefix=\"cc1\" %>");
			controls.Append("\n<asp:Content ID=\"Content1\" ContentPlaceHolderID=\"BasicContents\" runat=\"Server\">");
            controls.Append("\n\t<cc1:MasterBox ID=\"" + masterBoxID + "\" runat=\"server\" Header =\" " + ResourcesTesxtsBuilder.AddUserText(boxHeader, TextType.ServerControl) + "\" MasterBoxFile=\"/App_Design/User/MasterBoxes/General.ascx\" ContentFile=\"~/UserControls/" + global.TableProgramatlyName + "/" + userControl + ".ascx\" />");
            controls.Append("\n</asp:Content>");
			return controls.ToString();
		}
        //
		//
		public static void Create()
		{
			UserDefaultPage_InterfaceBuilder gaBuilder = new UserDefaultPage_InterfaceBuilder();

			gaBuilder.GenerateUserControlInterface();
		}
		//----------------------------------------
		//
		private void GenerateUserControlInterface()
		{
			//Begin create Control and check the free text box editor
			string html = GenerateHtml();
			//
			DirectoryInfo dInfo = Directory.CreateDirectory(Globals.SiteForms + global.TableProgramatlyName);
			string path = dInfo.FullName + "\\" + ClassName + ".aspx";
			//
			FileManager.CreateFile(path, html);
		}
        //
    }
}
