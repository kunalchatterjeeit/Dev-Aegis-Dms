﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLayer
{
    public class FileType : Disposable
    {
        public FileType() { }

        public int Save(Entity.FileType fileType)
        {
            return DataLayer.FileType.FileType_Save(fileType);
        }

        public List<Entity.FileType> GetAll(Entity.FileType fileType)
        {
            return DataLayer.FileType.FileType_GetAll(fileType);
        }

        public int FileTypeDelete(int fileTypeId)
        {
            return DataLayer.FileType.FileType_Delete(fileTypeId);
        }

        ~FileType()
        {
            Dispose(false);
        }

        //public DataTable GetAllById(Entity.FileType objEntity)
        //{
        //    return DataAccess.FileType.GetAllById(objEntity);
        //}

        //public int Delete(Entity.FileType objEntity)
        //{
        //    return DataAccess.FileType.Delete(objEntity);
        //}
    }
}
