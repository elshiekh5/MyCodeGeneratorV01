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
	public class GetAllWithSorting_InterfaceBuilder : InterfaceBuilder
    {
		//
        public GetAllWithSorting_InterfaceBuilder(InterfaceType type)
		{
			ClassName = global.GetAllWithSortingCodeBehindClass;
			HeaderTitle = global.HeaderTitle_GetAll;
			Type = type;
			
        }
		//----------------------------------
		private string GenerateDirective()
		{
			if (Type == InterfaceType.WEbUserControl)
			{
				return "<%@ Control Language=\"c#\" AutoEventWireup=\"true\" CodeFile=\"" + ClassName + ".ascx.cs\" Inherits=\"" + ClassName + "\" %><%@ Register Assembly=\"MoversFW\" Namespace=\"MoversFW\" TagPrefix=\"cc1\" %>";
			}
			else
			{
				return "<%@ Page language=\"c#\" MasterPageFile=\"/App_Design/Admin/Admin.master\" CodeFile=\"default.aspx.cs\" Inherits=\"Admin" + ClassName + "\"  %><%@ Register Assembly=\"MoversFW\" Namespace=\"MoversFW\" TagPrefix=\"cc1\" %>";
			}
		}
        //----------------------------------
        private string GenerateControls()
        {
			string dataGridID = "dg" + global.TableProgramatlyName;
            StringBuilder controls = new StringBuilder();

			controls.Append("\n\t\t\t\t<tr>");
            controls.Append("\n\t\t\t\t\t<td class=\"Result\" align=\"center\" colspan=\"2\">");
			controls.Append("\n\t\t\t\t\t\t<asp:Label ID=\"lblResult\" runat=\"server\" ForeColor=\"Red\" Text=\"\"></asp:Label>");
			controls.Append("\n\t\t\t\t\t</td>");
			controls.Append("\n\t\t\t\t</tr>");
            #region DataGrid Declaretion And Propreties
			controls.Append("\n\t\t\t\t<tr>");

            controls.Append("\n\t\t\t\t\t<td class=\"GridControl\" align=\"center\" colspan=\"2\">");
			controls.Append("\n\t\t\t\t\t\t<asp:DataGrid id=\"" + dataGridID + "\" runat=\"server\" SkinId=\"GridViewSkin\" ");
			controls.Append("\n\t\t\t\t\t\tOnDeleteCommand=\"" + dataGridID + "_DeleteCommand\" OnItemDataBound=\"" + dataGridID + "_ItemDataBound\" OnSortCommand=\"" + dataGridID + "_SortCommand\"   >");
            #endregion
			//
			if (Fields.Count > 0)
			{
				controls.Append("\n\t\t\t\t\t\t<Columns>");
				int i = 0;
                controls.Append("\n\t\t\t\t\t\t\t<asp:BoundColumn ItemStyle-Width=\"20\" ItemStyle-CssClass=\"ItemStyle\" DataField=\"Index\"  HeaderText=\"Index\"></asp:BoundColumn>");
                foreach (SQLDMO.Column column in Fields)
				{
					if ((ID == null || column.Name != ID.Name) && i < 2)
					{

                        controls.Append("\n\t\t\t\t\t\t\t<asp:BoundColumn SortExpression=\"" + column.Name + "\" ItemStyle-CssClass=\"ItemStyleTitle\" DataField=\"" + column.Name + "\" HeaderText=\"" + ResourcesTesxtsBuilder.AddUserText(column.Name, TextType.ServerControl) + "\"></asp:BoundColumn>");
					}
					++i;
				}
				if (ID != null)
				{
                    controls.Append("\n\t\t\t\t\t\t\t<asp:TemplateColumn ItemStyle-Width=\"20px\" ItemStyle-CssClass=\"ItemStyleButton\"  HeaderText=\"" + ResourcesTesxtsBuilder.AddAdminGlobalText("Update", TextType.ServerControl) + "\">");
					controls.Append("\n\t\t\t\t\t\t\t<ItemTemplate>");
					controls.Append("\n\t\t\t\t\t\t\t\t<a href='<%# \"Edit.aspx?" + Globals.GetProgramatlyName(ID.Name) + "=\"+DataBinder.Eval(Container.DataItem, \"" + ID.Name + "\")+\"&iK=" + global.TableProgramatlyName + "\" %>' class='Link'>");
					controls.Append("\n\t\t\t\t\t\t\t\t\t<img src=\"/App_Design/Admin/Icons/edit.gif\" style=\"border-width:0px\" alt=\"<%#" + ResourcesTesxtsBuilder.AddAdminGlobalText("Update", TextType.Text) + "%>\" /></a>");
					controls.Append("\n\t\t\t\t\t\t\t</ItemTemplate>");
					controls.Append("\n\t\t\t\t\t\t\t</asp:TemplateColumn>");
					//Delete column.
                    controls.Append("\n\t\t\t\t\t\t\t<asp:TemplateColumn ItemStyle-Width=\"20px\" ItemStyle-CssClass=\"ItemStyleButton\"  HeaderText=\"" + ResourcesTesxtsBuilder.AddAdminGlobalText("Delete", TextType.ServerControl) + "\">");
					controls.Append("\n\t\t\t\t\t\t\t<ItemTemplate>");
					controls.Append("\n\t\t\t\t\t\t\t\t<asp:ImageButton ID=\"lbtnDelete\" AlternateText=\"\" ImageUrl=\"/App_Design/Admin/Icons/delete.gif\" CommandName=\"Delete\" runat=\"server\"></asp:ImageButton>");
					controls.Append("\n\t\t\t\t\t\t\t</ItemTemplate>");
					controls.Append("\n\t\t\t\t\t\t\t</asp:TemplateColumn>");
				}
				controls.Append("\n\t\t\t\t\t\t</Columns>");
				controls.Append("\n\t\t\t\t\t\t</asp:DataGrid>");
			}
            controls.Append("\n\t\t\t\t\t</td>");
            controls.Append("\n\t\t\t\t</tr>");
			controls.Append("\n\t\t\t\t<tr>");
            controls.Append("\n\t\t\t\t\t<td class=\"Control\" align=\"center\" colspan=\"2\">");
            controls.Append("\n\t\t\t\t\t\t<cc1:OurPager ID=\"pager\" runat=\"server\" OnPageIndexChang=\"Pager_PageIndexChang\"></cc1:OurPager>");
            controls.Append("\n\t\t\t\t\t</td>");
			controls.Append("\n\t\t\t\t</tr>");
            //--------------------------------

            return controls.ToString();

        }
        //
		public static void Create(InterfaceType type)
        {
			GetAllWithSorting_InterfaceBuilder cr = new GetAllWithSorting_InterfaceBuilder(type);
			cr.GenerateInterface();
        }
		//

		//
		private void GenerateInterface()
		{
			if (Type == InterfaceType.WEbUserControl)
			{
				GenerateUserControlInterface();
			}
			else
			{
				GeneratePageInterface();
			}
		}
		//
		private void GenerateUserControlInterface()
		{
			//Begin create Control and check the free text box editor
			string controls = GenerateControls();
			StringBuilder userControl = new StringBuilder();
			userControl.Append(GenerateDirective());
			userControl.Append("\n" + ControlRegisters);
			userControl.Append("\n" + TableHeader);
			userControl.Append("\n" + controls);
			userControl.Append("\n" + TableFooter);

			//
			DirectoryInfo dInfo = Directory.CreateDirectory(Globals.UserControlsDirectory + global.TableProgramatlyName);
			string path = dInfo.FullName + "\\" + ClassName + ".ascx";
			//
			FileManager.CreateFile(path, userControl.ToString());

		}
		//
		private void GeneratePageInterface()
		{
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
		}
        //
    }
}
