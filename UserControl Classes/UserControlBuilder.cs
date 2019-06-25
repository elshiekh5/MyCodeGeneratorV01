using System;
using System.Text ;
namespace SPGen
{
	/// <summary>
	/// Summary description for CodeBehindBuilder.
	/// </summary>
	public class CodeBehindBuilder:Generator
	{
		private InterfaceType _Type;
		public InterfaceType Type
		{
			get
			{
				return _Type;
			}
			set
			{
				_Type = value;
			}
		}
		//
        protected string GenerateUsingBlock()
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
            return Usingblock;
        }
        //------------------------------
        protected string GeneratePageLoadHandler(string PageLoadBody)
        {
			StringBuilder pageLoad = new StringBuilder();
			pageLoad.Append("\n\t#region ---------------Page_Load---------------");
			pageLoad.Append("\n\t//-----------------------------------------------");
			pageLoad.Append("\n\t//Page_Load");
			pageLoad.Append("\n\t//-----------------------------------------------");
            pageLoad.Append( "\n\tprivate void Page_Load(object sender, System.EventArgs e)");
			pageLoad.Append("\n\t{");
			pageLoad.Append(PageLoadBody);
			pageLoad.Append("\n\t}");
			pageLoad.Append("\n\t//-----------------------------------------------");
			pageLoad.Append("\n\t#endregion");
			
            return pageLoad.ToString();
        }
		//-------------------------------
        protected string GenerateClass(string classBody)
        {
            string _class = "";
            _class += GenerateUsingBlock();
			if (Type == InterfaceType.WEbUserControl)
			{
				_class += "\npublic partial class " + ClassName + " : System.Web.UI.UserControl";
			}
			else
			{
                _class += "\npublic partial class Admin" + ClassName + " : System.Web.UI.Page";
			}
			_class += "\n{" + classBody + "\n}";

            return _class;
        }
	}
}
