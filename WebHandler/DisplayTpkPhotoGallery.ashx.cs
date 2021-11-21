using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using e_TravelBLL.TourPackage;

namespace e_Travel.WebHandler
{
    /// <summary>
    /// Summary description for DisplayTpkPhotoGallery
    /// </summary>
    public class DisplayTpkPhotoGallery : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            // Set up the response settings
            context.Response.ContentType = "image/jpeg";
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.BufferOutput = false;

            byte[] objImageData = null;
            if (string.IsNullOrEmpty(context.Request.QueryString["IT"]) == false && string.IsNullOrEmpty(context.Request.QueryString["ID"]) == false)
            {
                switch (context.Request.QueryString["IT"])
                {
                    case "PT": // Photo Thumbnail
                        objImageData = (new TpkPackageMst()).SelectPhotoThumbnail(context.Request.QueryString["ID"].ToString());
                        break;
                    case "PN": // Original Photo
                        objImageData = (new TpkPackageMst()).SelectPhotoNormal(context.Request.QueryString["ID"].ToString());
                        break;
                }
                if (objImageData != null && objImageData.Length > 0)
                {
                    context.Response.OutputStream.Write(objImageData, 0, objImageData.Length);
                }
            }

            context.Response.End();
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