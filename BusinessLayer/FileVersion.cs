using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class FileVersion
    {
        public FileVersion()
        {

        }

        public Guid? FileVersion_Save(Entity.FileVersion fileVersion)
        {
            return DataLayer.FileVersion.FileVersion_Save(fileVersion);
        }

        public List<Entity.FileVersion> FileVersion_GetByFileGuid(long fileId, int pageIndex, int pageSize)
        {
            return DataLayer.FileVersion.FileVersion_GetByFileGuid(fileId, pageIndex, pageSize);
        }
    }
}
