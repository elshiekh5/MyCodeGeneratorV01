using System;

namespace SPGen
{
	/// <summary>
	/// Summary description for VirtualDirectoryBuilder.
	/// </summary>
	public class VirtualDirectoryBuilder
	{
		public static void Create()
		{
			VirtualDirectoryBuilder vdb=new  VirtualDirectoryBuilder();
			string phisicalPath=ProjectBuilder.PhysicalPath+"\\"+ProjectBuilder.ProjectName;
			phisicalPath=phisicalPath.Replace("\\\\","\\");
			vdb.CreateVirtualDirectory(phisicalPath,ProjectBuilder.ServerName,ProjectBuilder.ProjectName);
		}
		private void CreateVirtualDirectory(string physicalPath,string serverName, string vDirName)
		{
			
			VDirLib objVDirLib = new VDirLib();

			
			objVDirLib.strDirectoryType = VDirLib.VDirType.WEB_IIS_DIR;

			objVDirLib.strPhysicalPath = physicalPath;
			objVDirLib.strServerName =serverName;
			objVDirLib.strVDirName = vDirName;
			objVDirLib.CreateVDir();
		}   
	}
}
