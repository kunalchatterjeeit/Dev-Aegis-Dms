﻿using System;
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

        public static Guid? FileSave(Entity.File file)
        {
            return DataLayer.File.File_Save(file);
        }

        public static Guid FileUpdate(Entity.File file)
        {
            return DataLayer.File.File_Update(file);
        }

        public static DataTable FileGetAll(Entity.File file)
        {
            return DataLayer.File.File_GetAll(file);
        }

        public static DataTable GetPendingContentTransferFiles()
        {
            return DataLayer.File.GetPendingContentTransferFiles();
        }

        public static int File_Content_Save(Guid fileGuid, string content)
        {
            return DataLayer.File.File_Content_Save(fileGuid, content);
        }

        public static DataTable FileSearchByMetadata(string condition)
        {
            return DataLayer.File.File_SearchByMetadata(condition);
        }

        public static DataTable FileSearchByPhrase(string phrase)
        {
            return DataLayer.File.File_SearchByPhrase(phrase);
        }

        public static DataTable FileGetByFileGuid(string fileGuid)
        {
            return DataLayer.File.File_GetByFileGuid(fileGuid);
        }

        public static int FileStatusChange(Guid fileGuid, int status)
        {
            return DataLayer.File.File_StatusChange(fileGuid, status);
        }

        public static int FileDelete(Guid fileGuid)
        {
            return DataLayer.File.File_Delete(fileGuid);
        }

        // Destructor
        ~File()
        {
            Dispose(false);
        }
    }
}