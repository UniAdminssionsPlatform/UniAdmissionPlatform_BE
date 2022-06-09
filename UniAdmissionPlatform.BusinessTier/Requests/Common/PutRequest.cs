using System.Collections.Generic;

namespace UniAdmissionPlatform.BusinessTier.Requests.Common
{
    public class PutRequest<TN, TU, TD>
    {
        public List<TN> NewList { get; set; }
        public List<TU> UpdateList { get; set; }
        public List<TD> DeleteList { get; set; }
    }
}