using System;
using System.Data ;
using System.Data.SqlClient ;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace SPGen
{
    /// <summary>
    /// Stored Procedure Helper class
    /// </summary>
    public class StoredProcedure : Generator
    {
        public StringBuilder Procedures = new StringBuilder();

        public static void Create()
        {
            StoredProcedure sp = new StoredProcedure();
            sp.GenerateInsertProcedure();
            sp.GenerateUpdatePrcedure();
            sp.GenerateGetAllProcedure();
            sp.GenerateGetAllWithPagerProcedure();
            sp.GenerateGetAllWithPagerAndSortingProcedure();
            sp.GenerateGetCountProcedure();
            sp.GenerateReletionalGetData();
            if (sp.ID != null)
            {
                sp.GenerateGetOneProcedure();
            }
            sp.GenerateDeleteProcedure();
            sp.CreateSqlFile();
        }
        //
        public void CreateSqlFile()
        {
            try
            {
                string path = Globals.App_DataDirectory + Table +"SP.sql";
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(Procedures.ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);

            }
        }
        //---------------------------------
        /// <summary>
        /// Generates code for INSERT Stored Procedure
        /// </summary>
        /// <param name="sptypeGenerate">The type of SP to generate, INSERT</param>
        /// <param name="Fields">A SQLDMO.Columns collection</param>
        /// <returns>The SP code</returns>
        private void GenerateInsertProcedure()
        {
            try
            {
                StringBuilder sGeneratedCode = new StringBuilder();
                StringBuilder sParamDeclaration = new StringBuilder();
                StringBuilder sBody = new StringBuilder();
                StringBuilder sINSERTValues = new StringBuilder();
                StringBuilder finnal = new StringBuilder();
                // Setup SP code, begining is the same no matter the type
                sGeneratedCode.AppendFormat("CREATE PROCEDURE [{0}_{1}]", new string[] { Globals.GetProgramatlyName(Table), StoredProcedureTypes.Create.ToString() });
                sGeneratedCode.Append(Environment.NewLine);
                // Setup body code, different for UPDATE and INSERT
                if (ProjectBuilder.ISExcuteScaler)
                {
                    StringBuilder existWhereStatement = new StringBuilder();
                    existWhereStatement.AppendFormat("/*WHERE {0} = @{0}*/", new string[] { ProjectBuilder.IdentityText });
                    
                    sBody.Append("\n--Edit your Unique Column here");
                    sBody.AppendFormat("\nIF( EXISTS( SELECT 1 FROM [" + Globals.GetProgramatlyName(Table) + "] " + existWhereStatement.ToString() + " ) )");
                   
                    sBody.AppendFormat("\nBEGIN");
                    sBody.AppendFormat("\n\tSELECT -1");
                    sBody.AppendFormat("\nEND");
                    sBody.AppendFormat("\n");
                    sBody.AppendFormat("\nELSE");
                    sBody.AppendFormat("\nBEGIN");
                }
                sBody.AppendFormat("\nINSERT INTO [{0}] (", Table);
                sBody.Append(Environment.NewLine);
                sINSERTValues.Append("VALUES (");
                sINSERTValues.Append(Environment.NewLine);
                //------------------------------------------------------
                bool hasPriority = false;
                StringBuilder priorityStatement = new StringBuilder();
                priorityStatement.Append("\nIF EXISTS (SELECT Priority FROM [" + Globals.GetProgramatlyName(Table) + "] WHERE Priority >=@Priority)");
                priorityStatement.Append("\nbegin");
                priorityStatement.Append("\nUPDATE [" + Globals.GetProgramatlyName(Table) + "] SET Priority =Priority + 1 WHERE Priority >=@Priority");
                priorityStatement.Append("\nEND");
                //------------------------------------------------------
                #region Add Parametars
                foreach (SQLDMO.Column colCurrent in Fields)
                {
                    //hasPriority
                    if (colCurrent.Name == ProjectBuilder.PriorityColumnName)
                    {
                        hasPriority = true;
                    }
                    //----------------------
                    if (colCurrent.Name.IndexOf("_") < 0)
                    {
                        sParamDeclaration.AppendFormat("    @{0} {1}", new string[] { Globals.GetProgramatlyName(colCurrent.Name), colCurrent.Datatype });

                        if (ID != null && colCurrent.Name == ID.Name && Globals.CheckIsAddedBySql(colCurrent))
                            sParamDeclaration.AppendFormat(" out");
                        // Only binary, char, nchar, nvarchar, varbinary and varchar may have their length declared								
                        if (
                            colCurrent.Datatype == "binary" ||
                            colCurrent.Datatype == "char" ||
                            colCurrent.Datatype == "nchar" ||
                            colCurrent.Datatype == "nvarchar" ||
                            colCurrent.Datatype == "varbinary" ||
                            colCurrent.Datatype == "varchar")
                            sParamDeclaration.AppendFormat("({0})", colCurrent.Length);

                        sParamDeclaration.Append(",");
                        sParamDeclaration.Append(Environment.NewLine);

                        // Body construction

                        //not Added BySQL
                        if (ID == null || colCurrent.Name != ID.Name || !Globals.CheckIsAddedBySql(colCurrent))
                        {
                            sINSERTValues.AppendFormat("    @{0},", Globals.GetProgramatlyName(colCurrent.Name));
                            sINSERTValues.Append(Environment.NewLine);

                            sBody.AppendFormat("    [{0}],", colCurrent.Name);
                            sBody.Append(Environment.NewLine);
                        }
                    }
                }
                if (ID != null && Globals.CheckIsAddedBySql(ID))
                {
                    finnal.Append("SET @" + Globals.GetProgramatlyName(ID.Name) + " = @@Identity");
                }
                if (ProjectBuilder.ISExcuteScaler)
                {
                    finnal.Append("\nSELECT 1");
                    finnal.Append("\nEND");
                }
                #endregion
                // Now stitch the body parts together into the SP whole			
                sGeneratedCode.Append(sParamDeclaration.Remove(sParamDeclaration.Length - 3, 3));
                sGeneratedCode.Append(Environment.NewLine);
                sGeneratedCode.Append("AS");
                if (hasPriority)
                {
                    sGeneratedCode.Append(Environment.NewLine);
                    sGeneratedCode.Append(priorityStatement);
                }
                sGeneratedCode.Append(Environment.NewLine);

                sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));
                sGeneratedCode.Append(")");
                sGeneratedCode.Append(Environment.NewLine);
                sGeneratedCode.Append(sINSERTValues.Remove(sINSERTValues.Length - 3, 3));
                sGeneratedCode.Append(")");
                //
                sGeneratedCode.Append(Environment.NewLine);
                sGeneratedCode.Append(finnal);
                WriteStoredProcedure(sGeneratedCode.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);
            }
        }
        //---------------------------------
        /// <summary>
        /// Generates code for an UPDATE  Stored Procedure
        /// </summary>
        /// <param name="sptypeGenerate">The type of SP to generate, INSERT or UPDATE</param>
        /// <param name="Fields">A SQLDMO.Columns collection</param>
        /// <returns>The SP code</returns>
        private void GenerateUpdatePrcedure()
        {
            try
            {
                StringBuilder sGeneratedCode = new StringBuilder();
                StringBuilder sParamDeclaration = new StringBuilder();
                StringBuilder sBody = new StringBuilder();
                StringBuilder sINSERTValues = new StringBuilder();
                StringBuilder finnal = new StringBuilder();
                // Setup SP code, begining is the same no matter the type
                sGeneratedCode.AppendFormat("CREATE PROCEDURE [{0}_{1}]", new string[] { Globals.GetProgramatlyName(Table), StoredProcedureTypes.Update.ToString() });
                sGeneratedCode.Append(Environment.NewLine);
                //------------------------------------------------------
                bool hasPriority = false;
                StringBuilder priorityStatement = new StringBuilder();
                priorityStatement.Append("\nDECLARE @OldPriority int");
                priorityStatement.Append("\nset @OldPriority = (SELECT [Priority] FROM [" + Globals.GetProgramatlyName(Table) + "] WHERE  [" + ID.Name + "] =@" + ID.Name + ")");
                priorityStatement.Append("\nif (@OldPriority > @Priority)");
                priorityStatement.Append("\nBEGIN");
                priorityStatement.Append("\n\tUPDATE [" + Globals.GetProgramatlyName(Table) + "] SET [Priority] = [Priority] + 1 WHERE Priority>=@Priority and Priority<@OldPriority");
                priorityStatement.Append("\nEND");
                priorityStatement.Append("\nELSE IF (@OldPriority < @Priority)");
                priorityStatement.Append("\nBEGIN");
                priorityStatement.Append("\n\tUPDATE [" + Globals.GetProgramatlyName(Table) + "] SET [Priority] = [Priority] - 1 WHERE Priority<=@Priority and Priority>@OldPriority");
                priorityStatement.Append("\nEND");

                //------------------------------------------------------
                // Setup body code, different for UPDATE and INSERT
                if (ProjectBuilder.ISExcuteScaler)
                {
                    StringBuilder existWhereStatement = new StringBuilder();
                    existWhereStatement.AppendFormat("/*WHERE {0} = @{0} and {1} <> @{1}*/", new string[] { ProjectBuilder.IdentityText, ID.Name });
                    sBody.Append("\n--Edit your Unique Column here");
                    sBody.AppendFormat("\nIF( EXISTS( SELECT 1 FROM [" + Globals.GetProgramatlyName(Table) + "] " + existWhereStatement.ToString() + " ) )");
                    sBody.AppendFormat("\nBEGIN");
                    sBody.AppendFormat("\n\tSELECT -1");
                    sBody.AppendFormat("\nEND");
                    sBody.AppendFormat("\n");
                    sBody.AppendFormat("\nELSE");
                    sBody.AppendFormat("\nBEGIN");
                }
                sBody.AppendFormat("\nUPDATE [{0}]", Table);
                sBody.Append(Environment.NewLine);
                sBody.Append("SET");
                sBody.Append(Environment.NewLine);

                #region Add Parametars

                foreach (SQLDMO.Column colCurrent in Fields)
                {
                    //hasPriority
                    if (colCurrent.Name == ProjectBuilder.PriorityColumnName)
                    {
                        hasPriority = true;
                    }
                    //----------------------
                    if (colCurrent.Name.IndexOf("_") < 0 && colCurrent.Name.ToLower() != ProjectBuilder.LangID.ToLower())
                    {
                        sParamDeclaration.AppendFormat("    @{0} {1}", new string[] { Globals.GetProgramatlyName(colCurrent.Name), colCurrent.Datatype });


                        // Only binary, char, nchar, nvarchar, varbinary and varchar may have their length declared								
                        if (
                            colCurrent.Datatype == "binary" ||
                            colCurrent.Datatype == "char" ||
                            colCurrent.Datatype == "nchar" ||
                            colCurrent.Datatype == "nvarchar" ||
                            colCurrent.Datatype == "varbinary" ||
                            colCurrent.Datatype == "varchar")
                            sParamDeclaration.AppendFormat("({0})", colCurrent.Length);

                        sParamDeclaration.Append(",");
                        sParamDeclaration.Append(Environment.NewLine);

                        // Body construction, different for INSERT and UPDATE

                        if (ID == null || colCurrent.Name != ID.Name)
                        {

                            sBody.AppendFormat("    [{0}] = @{1},", new string[] { colCurrent.Name, Globals.GetProgramatlyName(colCurrent.Name) });
                            sBody.Append(Environment.NewLine);

                        }
                    }
                }
                if (ID != null)
                {
                    finnal.Append("WHERE    [" + ID.Name + "] =@" + Globals.GetProgramatlyName(ID.Name));

                }
                if (ProjectBuilder.ISExcuteScaler)
                {
                    finnal.Append("\nSELECT 1");
                    finnal.Append("\nEND");
                }
                #endregion

                // Now stitch the body parts together into the SP whole			
                sGeneratedCode.Append(sParamDeclaration.Remove(sParamDeclaration.Length - 3, 3));
                sGeneratedCode.Append(Environment.NewLine);
                sGeneratedCode.Append("AS");
                if (hasPriority)
                {
                    sGeneratedCode.Append(Environment.NewLine);
                    sGeneratedCode.Append(priorityStatement);
                }
                sGeneratedCode.Append(Environment.NewLine);

                sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));

                sGeneratedCode.Append(Environment.NewLine);
                sGeneratedCode.Append(finnal);


                WriteStoredProcedure(sGeneratedCode.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);

            }
        }
        private void GenerateReletionalGetData()
        {
            StringBuilder relationalMethods = new StringBuilder();

            foreach (SQLDMO.Column column in Fields)
            {
                //if ((ID == null || column.Name != ID.Name) && (column.Default == null || column.Default.Length != 0) && column.Name.ToLower() != ProjectBuilder.LangID)
                if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0))
                {
                    TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
                    if (cnstr != null)
                    {
                        GenerateGetAllProcedure(column);
                        GenerateGetAllWithPagerProcedure(column);
                        GenerateGetAllWithPagerAndSortingProcedure(column);
                        GenerateGetCountProcedure(column);
                    }
                }
            }
        }
        //---------------------------------
        private void GenerateGetCountProcedure()
        {
            GenerateGetCountProcedure(null);
        }
        private void GenerateGetCountProcedure(SQLDMO.Column conditionalColumn)
        {
            try
            {
                StringBuilder sGeneratedCode = new StringBuilder();
                string procedureName = StoredProcedureTypes.GetCount.ToString();
                if (conditionalColumn != null)
                    procedureName += "By" + Globals.GetProgramatlyName(conditionalColumn.Name);
                // Setup SP code, begining is the same no matter the type
                sGeneratedCode.AppendFormat("CREATE PROCEDURE [{0}_{1}]", new string[] { Globals.GetProgramatlyName(Table), procedureName });
                //----------------------
                bool hasPreviousParameter = false;
                if (conditionalColumn != null)
                {
                    sGeneratedCode.AppendFormat("\n\t@{0} {1}", new string[] { Globals.GetProgramatlyName(conditionalColumn.Name), conditionalColumn.Datatype });
                    // Only binary, char, nchar, nvarchar, varbinary and varchar may have their length declared								
                    if (
                        conditionalColumn.Datatype == "binary" ||
                        conditionalColumn.Datatype == "char" ||
                        conditionalColumn.Datatype == "nchar" ||
                        conditionalColumn.Datatype == "nvarchar" ||
                        conditionalColumn.Datatype == "varbinary" ||
                        conditionalColumn.Datatype == "varchar")
                    {
                        sGeneratedCode.AppendFormat("({0})", conditionalColumn.Length);
                    }
                    hasPreviousParameter = true;
                }

                //----------------------
                //@LangID 
                bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(SqlProvider.obj.TableName));
                if (isMaultiLanguages && (conditionalColumn== null || conditionalColumn.Name.ToLower() != ProjectBuilder.LangID))
                {
                    //Check Previous Parameter
                    if (hasPreviousParameter)
                        sGeneratedCode.AppendFormat(",");
                    sGeneratedCode.AppendFormat("\n\t@{0} {1}", new string[] { "LangID", "int" });
                    hasPreviousParameter = true;
                }
                //----------------------
                //@IsAvailable
                bool hasIsAvailable = SqlProvider.CheckIsATableHasIsAvailableColumnName(SqlProvider.obj.TableName);
                if (hasIsAvailable)
                {
                    //Check Previous Parameter
                    if (hasPreviousParameter)
                        sGeneratedCode.AppendFormat(",");
                    sGeneratedCode.AppendFormat("\n\t@{0} {1}", new string[] { ProjectBuilder.IsAvailableConditionParam, "bit" });
                    hasPreviousParameter = true;
                }
                //----------------------
                sGeneratedCode.Append("\nAS");
                // Setup body code, different for UPDATE and INSERT
                StringBuilder selectStatements = new StringBuilder();
                selectStatements.Append("\nSELECT COUNT(1)  ");
                selectStatements.AppendFormat("\nFROM [{0}] ", Table);
                //----------------------
                StringBuilder selectConditions = new StringBuilder();
                bool hasPreviousCondition = false;
                //XXXXXXXXXX
                if (conditionalColumn != null)
                {

                    selectConditions.AppendFormat("\nWHERE ( [{0}] = @{1} ) ", new string[] { conditionalColumn.Name, Globals.GetProgramatlyName(conditionalColumn.Name) });
                    hasPreviousCondition = true;

                }
                if (isMaultiLanguages && (conditionalColumn== null || conditionalColumn.Name.ToLower() != ProjectBuilder.LangID))
                {
                    //Check Previous Condition
                    if (hasPreviousCondition)
                        selectConditions.AppendFormat(" AND ");
                    else
                        selectConditions.AppendFormat("\n\tWHERE ");
                    selectConditions.AppendFormat("( [{0}] = @{1} )", new string[] { "LangID", "LangID" });
                    hasPreviousCondition = true;
                }
                if (hasIsAvailable)
                {
                    ///------------------------------------------
                    StringBuilder additionalCondition = new StringBuilder();
                    //Check Previous Condition
                    if (hasPreviousCondition)
                        additionalCondition.AppendFormat(" AND ");
                    else
                        additionalCondition.AppendFormat("\n\tWHERE ");
                    //--------------------------------------------
                    additionalCondition.AppendFormat(" ( [{0}] = 1 )", ProjectBuilder.IsAvailable);
                    hasPreviousCondition = true;
                    //--------------------------------------------
                    //Build strings
                    sGeneratedCode.Append("\n\tif @IsAvailableCondition=1 ");
                    sGeneratedCode.Append("\n\tbegin");
                    sGeneratedCode.Append(selectStatements);
                    sGeneratedCode.Append(selectConditions);
                    sGeneratedCode.Append(additionalCondition);
                    sGeneratedCode.Append("\n\tEND");
                    sGeneratedCode.Append("\n\telse");
                    sGeneratedCode.Append("\n\tbegin");
                    sGeneratedCode.Append(selectStatements);
                    sGeneratedCode.Append(selectConditions);
                    sGeneratedCode.Append("\n\tEND");
                }
                else
                {
                    sGeneratedCode.Append(selectStatements);
                    sGeneratedCode.Append(selectConditions);
                }
                //XXXXXXXXXX
                WriteStoredProcedure(sGeneratedCode.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);

            }
        }
        //---------------------------------
        private void GenerateGetAllProcedure()
        {
            GenerateGetAllProcedure(null);
        }
        private void GenerateGetAllProcedure(SQLDMO.Column conditionalColumn)
        {
            try
            {

                StringBuilder sGeneratedCode = new StringBuilder();

                string procedureName = StoredProcedureTypes.GetAll.ToString();
                if (conditionalColumn != null)
                    procedureName += "By" + Globals.GetProgramatlyName(conditionalColumn.Name);
                // Setup SP code, begining is the same no matter the type
                sGeneratedCode.AppendFormat("CREATE PROCEDURE [{0}_{1}]", new string[] { Globals.GetProgramatlyName(Table), procedureName });
                //XXXXXXXXXX
                //----------------------
                bool hasPreviousParameter = false;
                if (conditionalColumn != null)
                {
                    sGeneratedCode.AppendFormat("\n\t@{0} {1}", new string[] { Globals.GetProgramatlyName(conditionalColumn.Name), conditionalColumn.Datatype });
                    // Only binary, char, nchar, nvarchar, varbinary and varchar may have their length declared								
                    if (
                        conditionalColumn.Datatype == "binary" ||
                        conditionalColumn.Datatype == "char" ||
                        conditionalColumn.Datatype == "nchar" ||
                        conditionalColumn.Datatype == "nvarchar" ||
                        conditionalColumn.Datatype == "varbinary" ||
                        conditionalColumn.Datatype == "varchar")
                    {
                        sGeneratedCode.AppendFormat("({0})", conditionalColumn.Length);
                    }
                    hasPreviousParameter = true;
                }

                //----------------------
                //@LangID 
                bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(SqlProvider.obj.TableName));
                if (isMaultiLanguages && (conditionalColumn== null || conditionalColumn.Name.ToLower() != ProjectBuilder.LangID))
                {
                    //Check Previous Parameter
                    if (hasPreviousParameter)
                        sGeneratedCode.AppendFormat(",");
                    sGeneratedCode.AppendFormat("\n\t@{0} {1}", new string[] { "LangID", "int" });
                    hasPreviousParameter = true;
                }
                //----------------------
                //@IsAvailable
                bool hasIsAvailable = SqlProvider.CheckIsATableHasIsAvailableColumnName(SqlProvider.obj.TableName);
                if (hasIsAvailable)
                {
                    //Check Previous Parameter
                    if (hasPreviousParameter)
                        sGeneratedCode.AppendFormat(",");
                    sGeneratedCode.AppendFormat("\n\t@{0} {1}", new string[] { ProjectBuilder.IsAvailableConditionParam, "bit" });
                    hasPreviousParameter = true;
                }
                //----------------------
                sGeneratedCode.Append("\nAS");
                // Setup body code, different for UPDATE and INSERT
                StringBuilder selectStatements = new StringBuilder();
                selectStatements.Append("\n SELECT * ");
                //int i = 0;
                //foreach (SQLDMO.Column colCurrent in Fields)
                //{
                //    if (i++ > 0)
                //        selectStatements.Append(" , ");
                //    selectStatements.Append("[" + SqlProvider.obj.TableName + "].[" + colCurrent.Name + "]");
                //}
                selectStatements.AppendFormat("\nFROM [{0}] ", Table);
                //----------------------
                StringBuilder selectConditions = new StringBuilder();
                bool hasPreviousCondition = false;
                //XXXXXXXXXX
                if (conditionalColumn != null)
                {

                    selectConditions.AppendFormat("\nWHERE ( [{0}] = @{1} ) ", new string[] { conditionalColumn.Name, Globals.GetProgramatlyName(conditionalColumn.Name) });
                    hasPreviousCondition = true;

                }
                if (isMaultiLanguages && (conditionalColumn== null || conditionalColumn.Name.ToLower() != ProjectBuilder.LangID))
                {
                    //Check Previous Condition
                    if (hasPreviousCondition)
                        selectConditions.AppendFormat(" AND ");
                    else
                        selectConditions.AppendFormat("\n\tWHERE ");
                    selectConditions.AppendFormat("( [{0}] = @{1} )", new string[] { "LangID", "LangID" });
                    hasPreviousCondition = true;
                }
                if (hasIsAvailable)
                {
                    ///------------------------------------------
                    StringBuilder additionalCondition = new StringBuilder();
                    //Check Previous Condition
                    if (hasPreviousCondition)
                        additionalCondition.AppendFormat(" AND ");
                    else
                        additionalCondition.AppendFormat("\n\tWHERE ");
                    //--------------------------------------------
                    additionalCondition.AppendFormat(" ( [{0}] = 1 )", ProjectBuilder.IsAvailable);
                    hasPreviousCondition = true;
                    //--------------------------------------------
                    //Build strings
                    sGeneratedCode.Append("\n\tif @IsAvailableCondition=1 ");
                    sGeneratedCode.Append("\n\tbegin");
                    sGeneratedCode.Append(selectStatements);
                    sGeneratedCode.Append(selectConditions);
                    sGeneratedCode.Append(additionalCondition);
                    sGeneratedCode.Append("\n\tEND");
                    sGeneratedCode.Append("\n\telse");
                    sGeneratedCode.Append("\n\tbegin");
                    sGeneratedCode.Append(selectStatements);
                    sGeneratedCode.Append(selectConditions);
                    sGeneratedCode.Append("\n\tEND");
                }
                else
                {
                    sGeneratedCode.Append(selectStatements);
                    sGeneratedCode.Append(selectConditions);
                }
                //XXXXXXXXXX
                WriteStoredProcedure(sGeneratedCode.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);

            }
        }
        //---------------------------------
        private void GenerateGetAllWithPagerProcedure()
        {
            GenerateGetAllWithPagerProcedure(null);
        }
        private void GenerateGetAllWithPagerProcedure(SQLDMO.Column conditionalColumn)
        {
            if (ID != null)
            {
                try
                {
                    StringBuilder sGeneratedCode = new StringBuilder();

                    string procedureName = StoredProcedureTypes.GetAllWithPager.ToString();
                    if (conditionalColumn != null)
                        procedureName += "By" + Globals.GetProgramatlyName(conditionalColumn.Name);
                    // Setup SP code, begining is the same no matter the type
                    sGeneratedCode.AppendFormat("CREATE PROCEDURE [{0}_{1}]", new string[] { Globals.GetProgramatlyName(Table), procedureName });
                    //XXXXXXXXXX
                    if (conditionalColumn != null)
                    {
                        sGeneratedCode.AppendFormat("\n\t@{0} {1}", new string[] { Globals.GetProgramatlyName(conditionalColumn.Name), conditionalColumn.Datatype });
                        // Only binary, char, nchar, nvarchar, varbinary and varchar may have their length declared								
                        if (
                            conditionalColumn.Datatype == "binary" ||
                            conditionalColumn.Datatype == "char" ||
                            conditionalColumn.Datatype == "nchar" ||
                            conditionalColumn.Datatype == "nvarchar" ||
                            conditionalColumn.Datatype == "varbinary" ||
                            conditionalColumn.Datatype == "varchar")
                        {
                            sGeneratedCode.AppendFormat("({0})", conditionalColumn.Length);
                        }
                        sGeneratedCode.Append(",");

                    }
                    //@LangID 
                    bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(SqlProvider.obj.TableName));
                    if (isMaultiLanguages && (conditionalColumn== null || conditionalColumn.Name.ToLower() != ProjectBuilder.LangID))
                    {
                        sGeneratedCode.AppendFormat("\n\t@{0} {1},", new string[] { "LangID", "int" });
                    }
                    //----------------------
                    //@IsAvailable
                    bool hasIsAvailable = SqlProvider.CheckIsATableHasIsAvailableColumnName(SqlProvider.obj.TableName);
                    if (hasIsAvailable)
                    {
                        sGeneratedCode.AppendFormat("\n\t@{0} {1},", new string[] { ProjectBuilder.IsAvailableConditionParam, "bit" });
                    }
                    //----------------------
                    //XXXXXXXXXX
                    sGeneratedCode.Append("\n\t@PageIndex int,");
                    sGeneratedCode.Append("\n\t@PageSize  int,");
                    sGeneratedCode.Append("\n\t@TotalRecords   int out");
                    sGeneratedCode.Append("\n\tAs");
                    sGeneratedCode.Append("\n\tDECLARE @PageLowerBound int");
                    sGeneratedCode.Append("\n\tSET @PageIndex = @PageIndex-1");
                    sGeneratedCode.Append("\n\tDECLARE @PageUpperBound int");
                    sGeneratedCode.Append("\n\tSET @PageLowerBound = @PageSize * @PageIndex");
                    sGeneratedCode.Append("\n\tSET @PageUpperBound = @PageSize - 1 + @PageLowerBound");
                    sGeneratedCode.Append("\n\t-- Create a temp table TO store the SELECT results");
                    sGeneratedCode.Append("\n\tCREATE TABLE #PageIndexTable");
                    sGeneratedCode.Append("\n\t(");
                    sGeneratedCode.Append("\n\t\tIndexId int IDENTITY (0, 1) NOT NULL,");
                    sGeneratedCode.Append("\n\t\tID int");
                    sGeneratedCode.Append("\n\t)");

                    #region TempTableStatements
                    StringBuilder tempTableStatements = new StringBuilder();

                    tempTableStatements.Append("\n\tINSERT INTO #PageIndexTable (ID)");
                    tempTableStatements.Append("\n\tSELECT [" + Globals.GetProgramatlyName(Table) + "]." + Globals.GetProgramatlyName(ID.Name));
                    tempTableStatements.Append("\n\tFROM [" + Globals.GetProgramatlyName(Table) + "]");
                    StringBuilder tempTableConditions = new StringBuilder();

                    //XXXXXXXXXX
                    bool hasPreviousCondition = false;
                    if (conditionalColumn != null)
                    {
                        tempTableConditions.AppendFormat("\n\tWHERE [{0}] = @{1} ", new string[] { conditionalColumn.Name, Globals.GetProgramatlyName(conditionalColumn.Name) });
                        hasPreviousCondition = true;
                    }

                    if (isMaultiLanguages && (conditionalColumn== null || conditionalColumn.Name.ToLower() != ProjectBuilder.LangID))
                    {
                        //Check Previous Condition
                        if (hasPreviousCondition)
                            tempTableConditions.AppendFormat(" AND ");
                        else
                            tempTableConditions.AppendFormat("\n\tWHERE ");
                        //--------------------------------------------
                        tempTableConditions.AppendFormat(" ( [{0}] = @{1} )", new string[] { "LangID", "LangID" });
                        hasPreviousCondition = true;
                        //--------------------------------------------
                    }

                    #endregion

                    if (hasIsAvailable)
                    {
                        ///------------------------------------------
                        StringBuilder additionalCondition = new StringBuilder();
                        //Check Previous Condition
                        if (hasPreviousCondition)
                            additionalCondition.AppendFormat(" AND ");
                        else
                            additionalCondition.AppendFormat("\n\tWHERE ");
                        //--------------------------------------------
                        additionalCondition.AppendFormat(" ( [{0}] = 1 )", ProjectBuilder.IsAvailable);
                        hasPreviousCondition = true;
                        //--------------------------------------------
                        //Build strings
                        sGeneratedCode.Append("\n\tif @IsAvailableCondition=1 ");
                        sGeneratedCode.Append("\n\tbegin");
                        sGeneratedCode.Append(tempTableStatements);
                        sGeneratedCode.Append(tempTableConditions);
                        sGeneratedCode.Append(additionalCondition);
                        sGeneratedCode.Append("\n\tEND");
                        sGeneratedCode.Append("\n\telse");
                        sGeneratedCode.Append("\n\tbegin");
                        sGeneratedCode.Append(tempTableStatements);
                        sGeneratedCode.Append(tempTableConditions);
                        sGeneratedCode.Append("\n\tEND");
                    }
                    else
                    {
                        sGeneratedCode.Append(tempTableStatements);
                        sGeneratedCode.Append(tempTableConditions);
                    }

                    //XXXXXXXXXX
                    sGeneratedCode.Append("\n\t-------------------------------------------");
                    sGeneratedCode.Append("\n\tSELECT @TotalRecords= @@ROWCOUNT");
                    sGeneratedCode.Append("\n\t-------------------------------------------");
                    sGeneratedCode.Append("\n\tSELECT    [" + Globals.GetProgramatlyName(Table) + "].*");
                    sGeneratedCode.Append("\n\tFROM         #PageIndexTable inner join  " + Globals.GetProgramatlyName(Table) + "");
                    sGeneratedCode.Append("\n\ton " + Globals.GetProgramatlyName(Table) + "." + Globals.GetProgramatlyName(ID.Name) + " =#PageIndexTable.ID");
                    sGeneratedCode.Append("\n\tWHERE  #PageIndexTable.IndexId >= @PageLowerBound AND #PageIndexTable.IndexId <= @PageUpperBound");
                    sGeneratedCode.Append("\n\t-------------------------------------------");
                    sGeneratedCode.Append(Environment.NewLine);

                    WriteStoredProcedure(sGeneratedCode.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("My Generated Code Exception:" + ex.Message);

                }
            }
        }
        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX//
        //---------------------------------
        private void GenerateGetAllWithPagerAndSortingProcedure()
        {
            GenerateGetAllWithPagerAndSortingProcedure(null);
        }
        private void GenerateGetAllWithPagerAndSortingProcedure(SQLDMO.Column conditionalColumn)
        {
            if (ID != null)
            {
                try
                {
                    StringBuilder sGeneratedCode = new StringBuilder();

                    string procedureName = StoredProcedureTypes.GetAllWithPagerAndSorting.ToString();
                    if (conditionalColumn != null)
                        procedureName += "By" + Globals.GetProgramatlyName(conditionalColumn.Name);
                    // Setup SP code, begining is the same no matter the type
                    sGeneratedCode.AppendFormat("CREATE PROCEDURE [{0}_{1}]", new string[] { Globals.GetProgramatlyName(Table), procedureName });
                    //XXXXXXXXXX
                    if (conditionalColumn != null)
                    {
                        sGeneratedCode.AppendFormat("\n\t@{0} {1}", new string[] { Globals.GetProgramatlyName(conditionalColumn.Name), conditionalColumn.Datatype });
                        // Only binary, char, nchar, nvarchar, varbinary and varchar may have their length declared								
                        if (
                            conditionalColumn.Datatype == "binary" ||
                            conditionalColumn.Datatype == "char" ||
                            conditionalColumn.Datatype == "nchar" ||
                            conditionalColumn.Datatype == "nvarchar" ||
                            conditionalColumn.Datatype == "varbinary" ||
                            conditionalColumn.Datatype == "varchar")
                        {
                            sGeneratedCode.AppendFormat("({0})", conditionalColumn.Length);
                        }
                        sGeneratedCode.Append(",");

                    }
                    //@LangID 
                    bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(SqlProvider.obj.TableName));
                    if (isMaultiLanguages && (conditionalColumn== null || conditionalColumn.Name.ToLower() != ProjectBuilder.LangID))
                    {
                        sGeneratedCode.AppendFormat("\n\t@{0} {1},", new string[] { "LangID", "int" });
                    }
                    //----------------------
                    //@IsAvailable
                    bool hasIsAvailable = SqlProvider.CheckIsATableHasIsAvailableColumnName(SqlProvider.obj.TableName);
                    if (hasIsAvailable)
                    {
                        sGeneratedCode.AppendFormat("\n\t@{0} {1},", new string[] { ProjectBuilder.IsAvailableConditionParam, "bit" });
                    }
                    //----------------------
                    //XXXXXXXXXX
                    sGeneratedCode.Append("\n\t@PageIndex int,");
                    sGeneratedCode.Append("\n\t@PageSize  int,");
                    sGeneratedCode.Append("\n\t@TotalRecords   int out,");
                    sGeneratedCode.Append("\n\t@SortExpression varchar(32)");

                    sGeneratedCode.Append("\n\tAs");
                    sGeneratedCode.Append("\n\tDECLARE @PageLowerBound int");
                    sGeneratedCode.Append("\n\tSET @PageIndex = @PageIndex-1");
                    sGeneratedCode.Append("\n\tDECLARE @PageUpperBound int");
                    sGeneratedCode.Append("\n\tSET @PageLowerBound = @PageSize * @PageIndex");
                    sGeneratedCode.Append("\n\tSET @PageUpperBound = @PageSize - 1 + @PageLowerBound");
                    sGeneratedCode.Append("\n\t-- Create a temp table TO store the SELECT results");
                    sGeneratedCode.Append("\n\tCREATE TABLE #PageIndexTable");
                    sGeneratedCode.Append("\n\t(");
                    sGeneratedCode.Append("\n\t\tIndexId int IDENTITY (0, 1) NOT NULL,");
                    sGeneratedCode.Append("\n\t\tID int");
                    sGeneratedCode.Append("\n\t)");
                    sGeneratedCode.Append("\n\t-----------------------------------------------");
                    sGeneratedCode.Append("\n\tdeclare @sqlStatement varchar(1024)");
                    #region TempTableStatements
                    StringBuilder tempTableStatements = new StringBuilder();
                    StringBuilder orderStatement = new StringBuilder();
                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                    
                    tempTableStatements.Append("\n\tset @sqlStatement=' INSERT INTO #PageIndexTable (ID) ");
                    tempTableStatements.Append("\n\t SELECT [" + Globals.GetProgramatlyName(Table) + "]." + Globals.GetProgramatlyName(ID.Name));
                    tempTableStatements.Append("\n\t FROM [" + Globals.GetProgramatlyName(Table) + "]");
                    //
                    orderStatement.Append("\n\t'+ @SortExpression ");
                    //
                    //SELECT @TotalRecords= @@ROWCOUNT
                    //-------------------------------------------
                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                    StringBuilder tempTableConditions = new StringBuilder();

                    //XXXXXXXXXX
                    bool hasPreviousCondition = false;
                    if (conditionalColumn != null)
                    {
                        tempTableConditions.AppendFormat("\n\t WHERE ([{0}] = '+  cast(@{1} AS varchar(16)) +') ", new string[] { conditionalColumn.Name, Globals.GetProgramatlyName(conditionalColumn.Name) });
                        hasPreviousCondition = true;
                    }

                    if (isMaultiLanguages && (conditionalColumn== null || conditionalColumn.Name.ToLower() != ProjectBuilder.LangID))
                    {
                        //Check Previous Condition
                        if (hasPreviousCondition)
                            tempTableConditions.AppendFormat(" AND ");
                        else
                            tempTableConditions.AppendFormat("\n\t WHERE ");
                        //--------------------------------------------
                        tempTableConditions.AppendFormat(" ( [{0}] = '+  cast(@{1} AS varchar(16)) +' )", new string[] { "LangID", "LangID" });
                        hasPreviousCondition = true;
                        //--------------------------------------------
                    }

                    #endregion

                    if (hasIsAvailable)
                    {
                        //------------------------------------------
                        StringBuilder additionalCondition = new StringBuilder();
                        //Check Previous Condition
                        if (hasPreviousCondition)
                            additionalCondition.AppendFormat(" AND ");
                        else
                            additionalCondition.AppendFormat("\n\tWHERE ");
                        //--------------------------------------------
                        additionalCondition.AppendFormat(" ( [{0}] = 1 )", ProjectBuilder.IsAvailable);
                        hasPreviousCondition = true;
                        //--------------------------------------------
                        //Build strings
                        sGeneratedCode.Append("\n\tif @IsAvailableCondition=1 ");
                        sGeneratedCode.Append("\n\tbegin");
                        sGeneratedCode.Append(tempTableStatements);
                        sGeneratedCode.Append(tempTableConditions);
                        sGeneratedCode.Append(additionalCondition);
                        sGeneratedCode.Append(orderStatement);
                        sGeneratedCode.Append("\n\tEND");
                        sGeneratedCode.Append("\n\telse");
                        sGeneratedCode.Append("\n\tbegin");
                        sGeneratedCode.Append(tempTableStatements);
                        sGeneratedCode.Append(tempTableConditions);
                        sGeneratedCode.Append(orderStatement);
                        sGeneratedCode.Append("\n\tEND");
                    }
                    else
                    {
                        sGeneratedCode.Append(tempTableStatements);
                        sGeneratedCode.Append(tempTableConditions);
                        sGeneratedCode.Append(orderStatement);
                    }
                    //XXXXXXXXXX
                    sGeneratedCode.Append("\n\t-------------------------------------------");
                    sGeneratedCode.Append("\n\tEXEC (@sqlStatement)");
                    sGeneratedCode.Append("\n\t-------------------------------------------");
                    sGeneratedCode.Append("\n\tSELECT @TotalRecords= @@ROWCOUNT");
                    sGeneratedCode.Append("\n\t-------------------------------------------");
                    sGeneratedCode.Append("\n\tSELECT    [" + Globals.GetProgramatlyName(Table) + "].*");
                    sGeneratedCode.Append("\n\tFROM         #PageIndexTable inner join  " + Globals.GetProgramatlyName(Table) + "");
                    sGeneratedCode.Append("\n\ton " + Globals.GetProgramatlyName(Table) + "." + Globals.GetProgramatlyName(ID.Name) + " =#PageIndexTable.ID");
                    sGeneratedCode.Append("\n\tWHERE  #PageIndexTable.IndexId >= @PageLowerBound AND #PageIndexTable.IndexId <= @PageUpperBound");
                    sGeneratedCode.Append("\n\t-------------------------------------------");
                    sGeneratedCode.Append(Environment.NewLine);
                    WriteStoredProcedure(sGeneratedCode.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("My Generated Code Exception:" + ex.Message);

                }
            }
        }
        //---------------------------------
        /// <summary>
        /// Generates code for an Get One Stored Procedure
        /// </summary>
        /// <param name="sptypeGenerate">The type of SP to generate, INSERT or UPDATE</param>
        /// <param name="Fields">A SQLDMO.Columns collection</param>
        /// <returns>The SP code</returns>
        private void GenerateGetOneProcedure()
        {
            try
            {
                StringBuilder sGeneratedCode = new StringBuilder();
                StringBuilder sParamDeclaration = new StringBuilder();
                StringBuilder sBody = new StringBuilder();
                StringBuilder sINSERTValues = new StringBuilder();
                string finnal = "";
                // Setup SP code, begining is the same no matter the type
                sGeneratedCode.AppendFormat("CREATE PROCEDURE [{0}_{1}]", new string[] { Globals.GetProgramatlyName(Table), StoredProcedureTypes.GetOneByID.ToString() });
                sGeneratedCode.Append(Environment.NewLine);

                // Setup body code, different for UPDATE and INSERT

                sBody.AppendFormat("SELECT * FROM [{0}] ", Table);
                sBody.Append(Environment.NewLine);
                if (ID != null)
                {
                    sParamDeclaration.AppendFormat("    @{0} {1}", new string[] { Globals.GetProgramatlyName(ID.Name), ID.Datatype });
                    // Only binary, char, nchar, nvarchar, varbinary and varchar may have their length declared								
                    if (
                        ID.Datatype == "binary" ||
                        ID.Datatype == "char" ||
                        ID.Datatype == "nchar" ||
                        ID.Datatype == "nvarchar" ||
                        ID.Datatype == "varbinary" ||
                        ID.Datatype == "varchar")
                        sParamDeclaration.AppendFormat("({0})", ID.Length);

                    sParamDeclaration.Append(",");
                    sParamDeclaration.Append(Environment.NewLine);
                    finnal = "WHERE    [" + ID.Name + "] = @" + Globals.GetProgramatlyName(ID.Name);
                }

                // Now stitch the body parts together into the SP whole			
                sGeneratedCode.Append(sParamDeclaration.Remove(sParamDeclaration.Length - 3, 3));
                sGeneratedCode.Append(Environment.NewLine);
                sGeneratedCode.Append("AS");
                sGeneratedCode.Append(Environment.NewLine);
                sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));

                sGeneratedCode.Append(Environment.NewLine);
                sGeneratedCode.Append(finnal);

                WriteStoredProcedure(sGeneratedCode.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);

            }
        }
        //---------------------------------
        /// <summary>
        /// Generates code for delete Stored Procedure
        /// </summary>
        /// <param name="sptypeGenerate">The type of SP to generate, INSERT or UPDATE</param>
        /// <param name="Fields">A SQLDMO.Columns collection</param>
        /// <returns>The SP code</returns>
        private void GenerateDeleteProcedure()
        {
            try
            {
                StringBuilder sGeneratedCode = new StringBuilder();
                StringBuilder sParamDeclaration = new StringBuilder();
                StringBuilder sBody = new StringBuilder();
                StringBuilder sINSERTValues = new StringBuilder();
                string finnal = "";
                // Setup SP code, begining is the same no matter the type
                sGeneratedCode.AppendFormat("CREATE PROCEDURE [{0}_{1}]", new string[] { Globals.GetProgramatlyName(Table), StoredProcedureTypes.Delete.ToString() });
                // Setup body code
                sBody.AppendFormat("Delete  FROM [{0}] ", Table);
                sBody.Append(Environment.NewLine);
                if (ID != null)
                {
                    sParamDeclaration.AppendFormat("    @{0} {1}", new string[] { Globals.GetProgramatlyName(ID.Name), ID.Datatype });
                    // Only binary, char, nchar, nvarchar, varbinary and varchar may have their length declared								
                    if (
                        ID.Datatype == "binary" ||
                        ID.Datatype == "char" ||
                        ID.Datatype == "nchar" ||
                        ID.Datatype == "nvarchar" ||
                        ID.Datatype == "varbinary" ||
                        ID.Datatype == "varchar")
                        sParamDeclaration.AppendFormat("({0})", ID.Length);

                    sParamDeclaration.Append(",");
                    sParamDeclaration.Append(Environment.NewLine);
                    finnal = "WHERE    [" + ID.Name + "] = @" + Globals.GetProgramatlyName(ID.Name);
                }

                // Now stitch the body parts together into the SP whole		
                sGeneratedCode.Append(Environment.NewLine);
                sGeneratedCode.Append(sParamDeclaration.Remove(sParamDeclaration.Length - 3, 3));
                sGeneratedCode.Append(Environment.NewLine);
                sGeneratedCode.Append("AS");
                sGeneratedCode.Append(Environment.NewLine);
                sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));
                sGeneratedCode.Append(Environment.NewLine);
                sGeneratedCode.Append(finnal);

                WriteStoredProcedure(sGeneratedCode.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);

            }
        }
        //---------------------------------
        public void WriteStoredProcedure(string procedure)
        {
            Procedures.Append(procedure);
            Procedures.Append("\n\nGo\n\n");
            SqlProvider.obj.ExecuteNonQuery(procedure);
        }
        //----------------------------------
        #region Old and Unused code
        /// <summary>
        /// Generates code for an UPDATE or INSERT Stored Procedure
        /// </summary>
        /// <param name="sptypeGenerate">The type of SP to generate, INSERT or UPDATE</param>
        /// <param name="Fields">A SQLDMO.Columns collection</param>
        /// <returns>The SP code</returns>
        private void Generate(StoredProcedureTypes sptypeGenerate, SqlProvider dmoMain)
        {
            try
            {
                StringBuilder sGeneratedCode = new StringBuilder();
                StringBuilder sParamDeclaration = new StringBuilder();
                StringBuilder sBody = new StringBuilder();
                StringBuilder sINSERTValues = new StringBuilder();
                string finnal = "";
                // Setup SP code, begining is the same no matter the type
                sGeneratedCode.AppendFormat("CREATE PROCEDURE [{0}_{1}]", new string[] { Globals.GetProgramatlyName(Table), sptypeGenerate.ToString() });
                sGeneratedCode.Append(Environment.NewLine);

                // Setup body code, different for UPDATE and INSERT
                switch (sptypeGenerate)
                {
                    case StoredProcedureTypes.Create:
                        sBody.AppendFormat("INSERT INTO [{0}] (", Table);
                        sBody.Append(Environment.NewLine);


                        sINSERTValues.Append("VALUES (");
                        sINSERTValues.Append(Environment.NewLine);
                        break;

                    case StoredProcedureTypes.Update:
                        sBody.AppendFormat("UPDATE [{0}]", Table);
                        sBody.Append(Environment.NewLine);
                        sBody.Append("SET");
                        sBody.Append(Environment.NewLine);
                        break;
                    case StoredProcedureTypes.GetAll:
                        sBody.AppendFormat("SELECT * FROM [{0}] ", Table);
                        sBody.Append(Environment.NewLine);
                        break;
                    case StoredProcedureTypes.GetOneByID:
                        sBody.AppendFormat("SELECT * FROM [{0}] ", Table);
                        sBody.Append(Environment.NewLine);
                        break;
                    case StoredProcedureTypes.Delete:
                        sBody.AppendFormat("Delete * FROM [{0}] ", Table);
                        sBody.Append(Environment.NewLine);
                        break;
                }
                #region Add Parametars
                if (sptypeGenerate == StoredProcedureTypes.GetAll)
                {
                    sGeneratedCode.Append("AS");
                    sGeneratedCode.Append(Environment.NewLine);
                    sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));


                }
                else if (sptypeGenerate != StoredProcedureTypes.GetOneByID || sptypeGenerate != StoredProcedureTypes.Delete)
                {
                    //The finanal of procedure
                    if (ID != null)
                    {
                        sParamDeclaration.AppendFormat("    @{0} {1}", new string[] { Globals.GetProgramatlyName(ID.Name), ID.Datatype });
                        finnal = "WHERE    [" + ID.Name + "] =@" + Globals.GetProgramatlyName(ID.Name);
                    }
                }
                else
                {
                    foreach (SQLDMO.Column colCurrent in Fields)
                    {
                        if (ID != null && colCurrent.Name == ID.Name)
                            sParamDeclaration.AppendFormat("    @{0} {1}", new string[] { Globals.GetProgramatlyName(colCurrent.Name), colCurrent.Datatype });
                        if (ID != null && colCurrent.Name == ID.Name && Globals.CheckIsAddedBySql(colCurrent) && sptypeGenerate == StoredProcedureTypes.Create)
                            sParamDeclaration.AppendFormat(" out");
                        // Only binary, char, nchar, nvarchar, varbinary and varchar may have their length declared								
                        if (
                            colCurrent.Datatype == "binary" ||
                            colCurrent.Datatype == "char" ||
                            colCurrent.Datatype == "nchar" ||
                            colCurrent.Datatype == "nvarchar" ||
                            colCurrent.Datatype == "varbinary" ||
                            colCurrent.Datatype == "varchar")
                            sParamDeclaration.AppendFormat("({0})", colCurrent.Length);

                        sParamDeclaration.Append(",");
                        sParamDeclaration.Append(Environment.NewLine);

                        // Body construction, different for INSERT and UPDATE
                        switch (sptypeGenerate)
                        {
                            case StoredProcedureTypes.Create:
                                //not Added BySQL
                                if (ID == null && colCurrent.Name != ID.Name || !Globals.CheckIsAddedBySql(colCurrent))
                                {
                                    sINSERTValues.AppendFormat("    @{0},", Globals.GetProgramatlyName(colCurrent.Name));
                                    sINSERTValues.Append(Environment.NewLine);

                                    sBody.AppendFormat("    [{0}],", colCurrent.Name);
                                    sBody.Append(Environment.NewLine);
                                }
                                break;

                            case StoredProcedureTypes.Update:
                                if (ID == null && colCurrent.Name != ID.Name)
                                {
                                    sBody.AppendFormat("    [{0}] = @{1},", new string[] { colCurrent.Name, Globals.GetProgramatlyName(colCurrent.Name) });
                                    sBody.Append(Environment.NewLine);

                                }
                                break;
                            case StoredProcedureTypes.GetOneByID:
                                //							sBody.AppendFormat("WHERE    {0} = @{0},", new string[]{colCurrent.Name, });											
                                sBody.Append(Environment.NewLine);
                                break;
                        }
                    }
                    //The finanal of procedure
                    if (ID != null)
                    {
                        if (sptypeGenerate == StoredProcedureTypes.Create && Globals.CheckIsAddedBySql(ID))
                            finnal = "SET @" + Globals.GetProgramatlyName(ID.Name) + " = @@Identity";
                        else if (sptypeGenerate == StoredProcedureTypes.Update)
                            finnal = "WHERE    [" + ID.Name + "] =@" + Globals.GetProgramatlyName(ID.Name);
                    }
                #endregion
                    // Now stitch the body parts together into the SP whole			
                    sGeneratedCode.Append(sParamDeclaration.Remove(sParamDeclaration.Length - 3, 3));
                    sGeneratedCode.Append(Environment.NewLine);
                    sGeneratedCode.Append("AS");
                    sGeneratedCode.Append(Environment.NewLine);
                    sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));
                    if (sptypeGenerate == StoredProcedureTypes.Create)
                    {
                        sGeneratedCode.Append(")");
                        sGeneratedCode.Append(Environment.NewLine);
                        sGeneratedCode.Append(sINSERTValues.Remove(sINSERTValues.Length - 3, 3));
                        sGeneratedCode.Append(")");
                    }
                }
                sGeneratedCode.Append(Environment.NewLine);
                sGeneratedCode.Append(finnal);


                WriteStoredProcedure(sGeneratedCode.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);

            }
        }
        #endregion
    }
}
