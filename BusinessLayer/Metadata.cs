using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    public class Metadata
    {
        public int MetadataFileMapping_Save(Guid? fileGuid, List<Entity.Metadata> metadatas, int userId)
        {
            int retValue = 0;
            foreach (Entity.Metadata metadata in metadatas)
            {
                if (!string.IsNullOrEmpty(metadata.MetadataValue.Trim()))
                {
                    Entity.MetadataFileMapping metadataFileMapping = new Entity.MetadataFileMapping()
                    {
                        MetadataId = metadata.MetadataId,
                        FileGuid = fileGuid,
                        MetadataContent = metadata.MetadataValue.Trim(),
                        CreatedDate = DateTime.Now,
                        CreatedBy = userId
                    };
                    int respose = BusinessLayer.MetadataFileMapping.MetadataFileMappingSave(metadataFileMapping);
                    if (respose > 0)
                    {
                        retValue++;
                    }
                }
            }
            return retValue;
        }
        public int MetadataFileMapping_Save(Guid? fileGuid, List<Entity.Metadata> metadatas, int userId, string fileOriginalName)
        {
            int retValue = 0;
            try
            {
                string[] metadataValues = fileOriginalName.Split('_');
                for (int metadataIndex = 0; metadataIndex < metadataValues.Count(); metadataIndex++)
                {
                    if (!string.IsNullOrEmpty(metadataValues[metadataIndex]) && (metadatas.Count > 0 && metadatas.Count > metadataIndex))
                    {
                        Entity.MetadataFileMapping metadataFileMapping = new Entity.MetadataFileMapping()
                        {
                            MetadataId = metadatas[metadataIndex].MetadataId,
                            FileGuid = fileGuid,
                            MetadataContent = metadataValues[metadataIndex].Trim(),
                            CreatedDate = DateTime.Now,
                            CreatedBy = userId
                        };

                        int respose = BusinessLayer.MetadataFileMapping.MetadataFileMappingSave(metadataFileMapping);
                        if (respose > 0)
                        {
                            retValue++;
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                //ex.Log(Request.Url.AbsoluteUri, Convert.ToInt32(HttpContext.Current.User.Identity.Name));
            }
            return retValue;
        }

        public bool IsMetadataManual(List<Entity.Metadata> metadatas)
        {
            bool retValue = false;
            foreach (Entity.Metadata metadata in metadatas)
            {
                if (!string.IsNullOrEmpty(metadata.MetadataValue.Trim()))
                {
                    retValue = true;
                }
            }
            return retValue;
        }
    }
}
