using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.IO;

namespace e_Travel.WebHandler
{
    /// <summary>
    /// Summary description for DisplayUploadedImage
    /// </summary>
    public class DisplayUploadedImage : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            try
            {
                if (HttpContext.Current.Session["ucPhotoUploaderUploadedImage"] != null)
                {
                    byte[] buffer = (byte[])HttpContext.Current.Session["ucPhotoUploaderUploadedImage"];

                    context.Response.ContentType = "image/jpeg";
                    context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    context.Response.BufferOutput = false;
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                }
                else
                {
                    Byte[] buffer = null;
                    FileStream file = File.OpenRead(HttpContext.Current.Request.PhysicalApplicationPath + "\\Images\\NoLogo.jpg");
                    buffer = new byte[file.Length];
                    file.Read(buffer, 0, int.Parse(file.Length.ToString()));
                    context.Response.ContentType = "image/jpeg";
                    context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    context.Response.BufferOutput = false;
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                }
                context.Response.End();
            }
            catch (Exception)
            {

            }
            finally
            {
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