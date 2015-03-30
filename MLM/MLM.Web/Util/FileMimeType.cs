using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLM.Util
{
    public class FileMimeType
    {
        private static MimeAssociation[] MimeTypeList = {
            new MimeAssociation{Extensions = new string[]{".pdf"}, MimeType = "application/pdf"},
            new MimeAssociation{Extensions = new string[]{".doc", ".docx"}, MimeType = "application/msword"},
            new MimeAssociation{Extensions = new string[]{".ppt", ".pptx"}, MimeType = "application/vnd.ms-powerpoint"},
            new MimeAssociation{Extensions = new string[]{".xls", ".xlsx"}, MimeType = "application/vnd.ms-excel"}
        };

        public static string GetMimeType(string extension)
        {
            MimeAssociation defaultType = new MimeAssociation { Extensions = new string[] { "" }, MimeType = "application/octet-stream" };
            return FileMimeType.MimeTypeList.Where(i => i.Extensions.Contains(extension)).DefaultIfEmpty(defaultType).First().MimeType;
        }
    }

    public class MimeAssociation
    {
        public string[] Extensions { get; set; }
        public string MimeType { get; set; }
    }
}