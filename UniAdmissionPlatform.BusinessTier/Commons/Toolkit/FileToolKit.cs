using System;
using System.Net;
using System.Linq;
using Microsoft.AspNetCore.Http;
using UniAdmissionPlatform.BusinessTier.Responses;

namespace UniAdmissionPlatform.BusinessTier.Commons.Toolkit
{
    public static class FileToolKit
    {
        public static void ValidateFileName(IFormFile file, string[] extensions, long fileMaxSize)
        {
            var lastIndexOf = file.FileName.LastIndexOf(".", StringComparison.Ordinal);
            
            if (lastIndexOf == -1 || !extensions.Contains(file.FileName[lastIndexOf..file.FileName.Length].ToLower()))
            {
                throw new ErrorResponse((int)HttpStatusCode.BadRequest, "Định dạng file bắt buộc là " + string.Join(" hoặc ",extensions) );
            }

            if (file.Length > fileMaxSize)
            {
                throw new ErrorResponse((int) HttpStatusCode.BadRequest, "Dung lượng file phải bé hơn 10 MB");
            }
        }
    }
}