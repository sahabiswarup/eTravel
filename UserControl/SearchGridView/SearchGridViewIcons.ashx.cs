using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using Sibin.Utilities.Imaging.TwoD;

namespace e_Travel.UserControl.SearchGridView
{
    /// <summary>
    /// Summary description for GridViewIcons
    /// </summary>
    public class SearchGridViewIcons : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            // Set up the response settings
            context.Response.ContentType = "image/jpeg";
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.BufferOutput = false;

            FileStream fs = File.OpenRead(HttpContext.Current.Server.MapPath("~/UserControl/SearchGridView/Images/Sprite.png"));
            byte[] objImageData = new byte[fs.Length];
            fs.Read(objImageData, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            byte[] objCropedImgData = null;

            switch (HttpContext.Current.Request.QueryString["T"])
            {
                case "S":
                    objCropedImgData = ImageShaper.Crop(objImageData, 17, 17, 72, 0);
                    break;
                case "D":
                    objCropedImgData = ImageShaper.Crop(objImageData, 17, 17, 72, 18);
                    break;
            }

            if (objCropedImgData != null && objCropedImgData.Length > 0)
            {
                context.Response.OutputStream.Write(objCropedImgData, 0, objCropedImgData.Length);
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