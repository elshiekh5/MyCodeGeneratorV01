using System;
using System.IO;
using System.Text ;
using System.Windows.Forms;
using System.Collections;
namespace SPGen
{
    /// <summary>
    /// Summary description for GetAllForUser_CodeBehindBuilder.
    /// </summary>
	public class GetAllForUser_InterfaceBuilder
    {
		Globals global;
		string ClassName;
		string repeaterID;
		public GetAllForUser_InterfaceBuilder()
		{
			global =new Globals();
			ClassName = global.TableProgramatlyName + "_" + "GetAllForUser";
			repeaterID = "dl" + global.TableProgramatlyName;
        }
        //----------------------------------
		private string GenerateUserControl()
		{
			StringBuilder controls = new StringBuilder();
			controls.Append("\n<%@ Control Language=\"c#\" AutoEventWireup=\"true\" CodeFile=\"" + ClassName + ".ascx.cs\" Inherits=\"" + ClassName + "\" %><%@ Register Assembly=\"MoversFW\" Namespace=\"MoversFW\" TagPrefix=\"cc1\" %>");
			controls.Append("\n<%@ Register Assembly=\"MoversFW\" Namespace=\"MoversFW\" TagPrefix=\"cc1\" %>");
            //controls.Append("\n<%@ Register Src=\"/UserControls/Pager/UserPager.ascx\" TagName=\"Pager\" TagPrefix=\"uc1\" %>");
			controls.Append("\n<asp:UpdatePanel ID=\"UpdatePanel1\" runat=\"server\">");
			controls.Append("\n\t<ContentTemplate>");
			controls.Append("\n\t\t<table id=\"Table1\" style=\"width: 100%;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">");
			controls.Append("\n\t\t\t<tr>");
			controls.Append("\n\t\t\t\t<td style=\"height: 100%;\" valign=\"top\" align=\"center\">");
			controls.Append("\n\t\t\t\t\t<asp:DataList ID=\"" + repeaterID + "\" runat=\"server\" />");
			controls.Append("\n\t\t\t\t</td>");
			controls.Append("\n\t\t\t</tr>");
			controls.Append("\n\t\t\t<tr>");
			controls.Append("\n\t\t\t\t<td align=\"center\" style=\"padding-bottom: 0px; height: 20px\">");
			controls.Append("\n\t\t\t\t\t<cc1:OurPager ID=\"pager\" runat=\"server\" OnPageIndexChang=\"Pager_PageIndexChang\"></cc1:OurPager>");
			controls.Append("\n\t\t\t\t</td>");
			controls.Append("\n\t\t\t</tr>");
			controls.Append("\n\t\t</table>");
			controls.Append("\n\t</ContentTemplate>");
			controls.Append("\n</asp:UpdatePanel>");
			return controls.ToString();

		}
        //
		//
		public static void Create()
		{
			GetAllForUser_InterfaceBuilder gaBuilder = new GetAllForUser_InterfaceBuilder();

			gaBuilder.GenerateUserControlInterface();
		}
		//----------------------------------------
		//
		private void GenerateUserControlInterface()
		{
			//Begin create Control and check the free text box editor
			string html = GenerateUserControl();
			//
			DirectoryInfo dInfo = Directory.CreateDirectory(Globals.UserControlsDirectory + global.TableProgramatlyName);
			string path = dInfo.FullName + "\\" + ClassName + ".ascx";
			//
			FileManager.CreateFile(path, html);
		}
		//
		private void GeneratePageInterface()
		{/*
			//Begin create Control and check the free text box editor
			string controls = GenerateControls();
			StringBuilder userControl = new StringBuilder();
			userControl.Append(GenerateDirective());
			userControl.Append("\n" + ControlRegisters);
			userControl.Append("\n <asp:Content id=\"Content1\" ContentPlaceHolderID=\"BasicContents\" runat=\"server\">");
			userControl.Append("\n" + TableHeader);
			userControl.Append("\n" + controls);
			userControl.Append("\n" + TableFooter);
			userControl.Append("\n</asp:Content>");
			//
			string directoryPath = Globals.AdminFolder + global.TableProgramatlyName + "\\";
			string path = directoryPath + "\\default.aspx";
			DirectoriesManager.ChechDirectory(directoryPath);
			FileManager.CreateFile(path, userControl.ToString());
		  * */
		}
        //
    }
}
