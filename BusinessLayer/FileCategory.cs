﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLayer
{
    public class FileCategory : Disposable
    {
        public FileCategory()
        { }

        public int FileCategorySave(Entity.FileCategory fileCategory)
        {
            return DataLayer.FileCategory.FileCategory_Save(fileCategory);
        }

        public List<Entity.FileCategory> FileCategoryGetAll(Entity.FileCategory fileCategory)
        {
            return DataLayer.FileCategory.FileCategory_GetAll(fileCategory);
        }

        public int FileCategoryDelete(int fileCategoryId)
        {
            return DataLayer.FileCategory.FileCategory_Delete(fileCategoryId);
        }

        ~ FileCategory()
        {
            Dispose(false);
        }

        //public DataTable GetById(Entity.FileCategory objEntityFileCategory)
        //{
        //    return DataAccess.FileCategory.GetById(objEntityFileCategory);
        //}

        //public int Delete(Entity.FileCategory objEntityFileCategory)
        //{
        //    return DataAccess.FileCategory.Delete(objEntityFileCategory);
        //}
    }
}
