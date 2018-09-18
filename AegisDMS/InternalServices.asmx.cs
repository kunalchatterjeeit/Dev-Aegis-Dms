using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AegisDMS
{
    /// <summary>
    /// Summary description for InternalServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class InternalServices : System.Web.Services.WebService
    {

        [WebMethod]
        public List<string> LoadAutoCompleteMetadata(string metadataId, string searchContent)
        {
            List<string> result = new List<string>();

            DataTable dtMetadataValues = BusinessLayer.MetadataFileMapping.MetadataFileMappingByMetadataId(Convert.ToInt64(metadataId));
            DataView dvMetadataValues = new DataView(dtMetadataValues);
            dvMetadataValues.RowFilter = "MetaDataContent LIKE '" + searchContent + "%'";
            dtMetadataValues = dvMetadataValues.ToTable();
            result = dtMetadataValues.AsEnumerable().Select(x => x[0].ToString()).ToList();
            return result;
        }
    }
}
