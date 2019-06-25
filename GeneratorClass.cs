using System;

namespace SPGen
{
	/// <summary>
	/// Summary description for GeneratorClass.
	/// </summary>
	public class Generator 
	{
		public string _ClassName="";
		public string ClassName
		{
			get{return _ClassName;}
			set{_ClassName=value;}
		}
		public SQLDMO.Columns Fields=null ;
		public SQLDMO.Column ID=null;
		public Globals global ;
		public string Table;
		public Generator()
		{
			Fields=SqlProvider.obj.Fields;
			//-----------------------------
			ID=SqlProvider.obj.ID;
			Table=SqlProvider.obj.TableName;
			global=new Globals();
		}
	}
}
