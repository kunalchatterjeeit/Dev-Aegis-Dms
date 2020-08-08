using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class File : Disposable
    {
        public File() { }

        public Guid? FileSave(Entity.File file)
        {
            return DataLayer.File.File_Save(file);
        }

        public Guid FileUpdate(Entity.File file)
        {
            return DataLayer.File.File_Update(file);
        }

        public DataTable FileGetAll(Entity.File file)
        {
            return DataLayer.File.File_GetAll(file);
        }

        public DataTable GetPendingContentTransferFiles()
        {
            return DataLayer.File.GetPendingContentTransferFiles();
        }

        public int File_Content_Save(Guid fileGuid, string content)
        {
            return DataLayer.File.File_Content_Save(fileGuid, content);
        }

        public List<Entity.SearchResult> FileSearchByMetadata(string condition, int userId)
        {
            return DataLayer.File.File_SearchByMetadata(condition, userId);
        }

        public List<Entity.SearchResult> FileSearchByPhrase(string phrase, int userId)
        {
            return DataLayer.File.File_SearchByPhrase(phrase, userId);
        }

        public Entity.File FileGetByFileGuid(string fileGuid)
        {
            return DataLayer.File.File_GetByFileGuid(fileGuid);
        }

        public int FileStatusChange(Guid fileGuid, int status)
        {
            return DataLayer.File.File_StatusChange(fileGuid, status);
        }

        public int FileDelete(Guid fileGuid)
        {
            return DataLayer.File.File_Delete(fileGuid);
        }

        public DataTable MetadataFileMappingGetByFileGuid(string fileGuid)
        {
            return DataLayer.File.MetadataFileMapping_GetByFileGuid(fileGuid);
        }

        public DataTable PdfSeperatorGetAll()
        {
            return DataLayer.File.PdfSeperator_GetAll();
        }

        public List<Entity.SearchResult> PrepareAdvanceSearch(List<Entity.Metadata> model, int userId)
        {
            List<Entity.SearchResult> searchResults = new List<Entity.SearchResult>();

            string query = string.Empty;
            string queryType = string.Empty;
            int commonOccurance = 1;

            if (model != null && model.Any())
            {
                foreach (Entity.Metadata drQuery in model)
                {
                    if (!(drQuery.Name.Equals("AND")) && !(drQuery.Name.Equals("OR")))
                    {
                        query += "(MetaDataId = " + drQuery.MetadataId;
                    }

                    if ((drQuery.Name.Equals("AND")) || (drQuery.Name.Equals("OR")))
                    {
                        if (string.IsNullOrEmpty(queryType))
                        {
                            queryType = drQuery.Name; //setting AND/OR to decide which query to call
                        }

                        query += " " + "OR" + " "; //OR for all query types //drQuery["MetadataName"].ToString()

                        ++commonOccurance; //Used for AND type query
                    }

                    if (!string.IsNullOrEmpty(drQuery.MetadataValue) || !string.IsNullOrEmpty(drQuery.MetadataValue))
                    {
                        query += " AND MetaDataContent = '" + drQuery.MetadataValue.Trim() + "')";
                    }
                }

                if (queryType == "AND" && commonOccurance > 0)
                {
                    query += " GROUP BY FileGuid HAVING COUNT(FileGuid) >= " + commonOccurance;
                }

                searchResults = new BusinessLayer.File().FileSearchByMetadata(query, userId);
            }
            return searchResults;
        }

        // Destructor
        ~File()
        {
            Dispose(false);
        }
    }
}
