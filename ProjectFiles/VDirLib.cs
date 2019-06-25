using System;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.DirectoryServices;

namespace SPGen
{
    class VDirLib
    {
        public VDirLib()
        {
            //By default,we are creating web IIS virual directory.
            this._strDirectoryType = VDirType.WEB_IIS_DIR;
        }

        /// <summary>
        /// Type of virtual directory to be created.
        /// </summary>
        
        public enum VDirType
        {
            FTP_DIR, WEB_IIS_DIR
        };

        /// <summary>
        /// Type of virtual directory to be created.
        /// That could be FTP or WebIIS Virtual Directory.
        /// </summary>

        private VDirType _strDirectoryType;

        public VDirType strDirectoryType
        {
            get
            {
                return _strDirectoryType;
            }
            set
            {
                _strDirectoryType = value;
            }
        }
        
        /// <summary>
        /// Physical path that would be mapped to the virtual directory
        /// </summary>
        
        private string _strPhysicalPath;
       
        public string strPhysicalPath
        {
            get
            {
                return _strPhysicalPath;
            }
            set
            {
                _strPhysicalPath = value;
            }
        }

        /// <summary>
        /// Name of the Virtaul directory to be created
        /// </summary>
        
        private string _strVDirName;
        
        public string strVDirName
        {
            get
            {
                return _strVDirName;
            }
            set
            {
                _strVDirName = value;
            }
        }

        /// <summary>
        /// Name of the server where to create the virtual directory
        /// </summary>

        private string _strServerName;

        public string strServerName
        {
            get
            {
                return _strServerName;
            }
            set
            {
                _strServerName = value;
            }

        }
        /// <summary>
        /// Creates the virual directory
        /// </summary>
        
        public string CreateVDir()
        {
            System.DirectoryServices.DirectoryEntry oDE;
            System.DirectoryServices.DirectoryEntries oDC;
            System.DirectoryServices.DirectoryEntry oVirDir;
            try
            {
                //check whether to create FTP or Web IIS Virtual Directory
                if (this._strDirectoryType == VDirType.WEB_IIS_DIR)
                {
                    oDE = new DirectoryEntry("IIS://"+this._strServerName+"/W3SVC/1/Root");
                }
                else
                {
                    oDE = new DirectoryEntry("IIS://" + this._strServerName + "/MSFTPSVC/1/Root");
                }
                
                //Get Default Web Site
                oDC = oDE.Children ;

                //Add row
                oVirDir = oDC.Add(this._strVDirName, oDE.SchemaClassName.ToString());
                
                //Commit changes for Schema class File
                oVirDir.CommitChanges();

                //Create physical path if it does not exists
                if (!Directory.Exists(this._strPhysicalPath))
                {
                    Directory.CreateDirectory(this._strPhysicalPath);
                }

                //Set virtual directory to physical path
                oVirDir.Properties["Path"].Value = this._strPhysicalPath;

                //Set read access
                oVirDir.Properties["AccessRead"][0] = true;

                //Create Application for IIS Application (as for ASP.NET)
                if (this._strDirectoryType == VDirType.WEB_IIS_DIR)
                {
                    oVirDir.Invoke("AppCreate", true);
                    oVirDir.Properties["AppFriendlyName"][0] = this._strVDirName;
                }
                
                //Save all the changes
                oVirDir.CommitChanges();

                return "Virtual Directory created sucessfully";

            }
            catch (Exception exc)
            {
                return exc.Message.ToString();
            }
        }
        /// <summary>
        /// Deletes the virtual directory
        /// </summary>
       
        public string DeleteVDir()
        {
            System.DirectoryServices.DirectoryEntry oDE;
            System.DirectoryServices.DirectoryEntries oDC;
            try
            {
                //check whether to delete FTP or Web IIS Virtual Directory
                if (this._strDirectoryType == VDirType.WEB_IIS_DIR)
                {
                    oDE = new DirectoryEntry("IIS://" + this._strServerName + "/W3SVC/1/Root");
                }
                else
                {
                    oDE = new DirectoryEntry("IIS://" + this._strServerName + "/MSFTPSVC/1/Root");
                }
                oDC = oDE.Children;
                
                //Find and remove the row from Directory entry.
                oDC.Remove(oDC.Find(this._strVDirName, oDE.SchemaClassName.ToString()));
                
                //Save the changes
                oDE.CommitChanges();

                return "Virtual Directory deleted sucessfully";

            }
            catch (Exception exc)
            {
                return exc.Message.ToString();
            }
        }


    }
}
