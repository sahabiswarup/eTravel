using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace e_Travel.WebHandler
{
    /// <summary>
    /// Summary description for DisplayIconImage
    /// </summary>
    public class DisplayIconImage : IHttpHandler
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
                    //case "AT": // Album Thumbnail
                    //    objImageData = (new BLLPhotoAlbumMaster()).SelectAlbumThumbnail(long.Parse(context.Request.QueryString["ID"]));
                    //    break;
                    case "PT": // Photo Thumbnail
                        // objImageData = (new ImageFileUploaderHelper()).SelectPhotoThumbnail(context.Request.QueryString["ID"].ToString());
                        break;
                    case "OP": // Original Photo
                        //  objImageData = (new ImageFileUploaderHelper()).SelectPhoto(context.Request.QueryString["ID"].ToString());
                        break;
                    case "AT"://Photo ThumbNail from PhotoActivity Log
                        //  objImageData = (new ImageFileUploaderHelper()).SelectPhotoThumbnailFromLogTable(context.Request.QueryString["ID"].ToString());
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