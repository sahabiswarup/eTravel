using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.IO;
using Sibin.Utilities.Imaging.TwoD;

namespace e_Travel.WebHandler
{
    /// <summary>
    /// Summary description for AjaxFileUploader
    /// </summary>
    public class AjaxFileUploader : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.BufferOutput = false;

                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(context.Request.Files[0].InputStream))
                {
                    fileData = binaryReader.ReadBytes(context.Request.Files[0].ContentLength);
                }
                if (context.Request.QueryString["file"] == null)
                {
                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    // Section Where you can set the Max Image Width and Height for upload -------------------------------
                    //----------------------------------------------------------------------------------------------------
                    //                                                                Image width and Height for display.
                    HttpContext.Current.Session["ucPhotoUploaderUploadedImage"] = ImageShaper.ResizeImage(fileData, 600, 450);
                    //====================================================================================================
                }
                else
                {
                    HttpContext.Current.Session["ucFileUploaderUploadedFile"] = fileData;
                }
                string msg = "{";
                msg += string.Format("error:'{0}',\n", string.Empty);
                msg += string.Format("msg:'{0}'\n", "Upload completed.");
                msg += "}";
                context.Response.Write(msg);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}