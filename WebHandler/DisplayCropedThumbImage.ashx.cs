using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sibin.Utilities.Imaging.TwoD;
using System.Web.SessionState;

namespace CliqueCityWeb.Handlers
{
    /// <summary>
    /// Summary description for DisplayCropedThumbImage
    /// </summary>
    public class DisplayCropedThumbImage : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            byte[] buffer = null;
            switch (context.Request.QueryString["TY"])
            {
                case "T":
                    if (HttpContext.Current.Session["ucPhotoUploaderUploadedImage"] != null)
                    {
                        buffer = (byte[])HttpContext.Current.Session["ucPhotoUploaderUploadedImage"];
                    }
                    break;
                case "P":
                    if (HttpContext.Current.Session["ucPhotoUploaderUploadedImagePhoto"] != null)
                    {
                        buffer = (byte[])HttpContext.Current.Session["ucPhotoUploaderUploadedImagePhoto"];
                    }
                    break;
            }
            if (context.Request.QueryString["TY"] == null)
            {
                if (HttpContext.Current.Session["ucPhotoUploaderUploadedImage"] != null)
                {
                    buffer = (byte[])HttpContext.Current.Session["ucPhotoUploaderUploadedImage"];
                }

            }


            //if (HttpContext.Current.Session["ucPhotoUploaderUploadedImage"] != null)
            //{
            //    buffer = (byte[])HttpContext.Current.Session["ucPhotoUploaderUploadedImage"];
            //}
            //else if (HttpContext.Current.Session["ucPhotoUploaderUploadedImagePhoto"] != null)
            //{
            //    buffer = (byte[])HttpContext.Current.Session["ucPhotoUploaderUploadedImagePhoto"];
            //}

            if (buffer != null)
            {
                if ((context.Request.QueryString["cropImageX1"] != "0" && context.Request.QueryString["cropImageX1"].Trim() != string.Empty) ||
               (context.Request.QueryString["cropImageY1"] != "0" && context.Request.QueryString["cropImageY1"].Trim() != string.Empty) ||
               (context.Request.QueryString["cropImageW"] != "0" && context.Request.QueryString["cropImageW"].Trim() != string.Empty) ||
               (context.Request.QueryString["cropImageH"] != "0" && context.Request.QueryString["cropImageH"].Trim() != string.Empty))
                {
                    buffer = ImageShaper.Crop(buffer, int.Parse(context.Request.QueryString["cropImageW"].Trim()), int.Parse(context.Request.QueryString["cropImageH"].Trim()), int.Parse(context.Request.QueryString["cropImageX1"].Trim()), int.Parse(context.Request.QueryString["cropImageY1"].Trim()));
                    int tempHeight = (int)((float.Parse(context.Request.QueryString["cropImageH"].Trim()) / float.Parse(context.Request.QueryString["cropImageW"].Trim())) * 160);
                    //buffer = ImageShaper.ResizeImage(buffer, 160, tempHeight);
                    HttpContext.Current.Session["ucPhotoUploaderUploadedImage"] = buffer;
                }

                context.Response.ContentType = "image/jpeg";
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.BufferOutput = false;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }

            context.Response.End();
        }
        //public void ProcessRequest(HttpContext context)
        //{
        //    byte[] buffer = null;
        //    switch (context.Request.QueryString["TY"])
        //    {
        //        case "T":
        //            if (HttpContext.Current.Session["ucPhotoUploaderUploadedImage"] != null)
        //            {
        //                buffer = (byte[])HttpContext.Current.Session["ucPhotoUploaderUploadedImage"];
        //            }
        //            break;
        //        case "P":
        //            if (HttpContext.Current.Session["ucPhotoUploaderUploadedImagePhoto"] != null)
        //            {
        //                buffer = (byte[])HttpContext.Current.Session["ucPhotoUploaderUploadedImagePhoto"];
        //            }
        //            break;
        //    }
        //    if (context.Request.QueryString["TY"] == null)
        //    {
        //        if (HttpContext.Current.Session["ucPhotoUploaderUploadedImage"] != null)
        //        {
        //            buffer = (byte[])HttpContext.Current.Session["ucPhotoUploaderUploadedImage"];
        //        }

        //    }

            
        //    //if (HttpContext.Current.Session["ucPhotoUploaderUploadedImage"] != null)
        //    //{
        //    //    buffer = (byte[])HttpContext.Current.Session["ucPhotoUploaderUploadedImage"];
        //    //}
        //    //else if (HttpContext.Current.Session["ucPhotoUploaderUploadedImagePhoto"] != null)
        //    //{
        //    //    buffer = (byte[])HttpContext.Current.Session["ucPhotoUploaderUploadedImagePhoto"];
        //    //}

        //    if (buffer != null)
        //    {
        //        if ((context.Request.QueryString["cropImageX1"] != "0" && context.Request.QueryString["cropImageX1"].Trim() != string.Empty) ||
        //       (context.Request.QueryString["cropImageY1"] != "0" && context.Request.QueryString["cropImageY1"].Trim() != string.Empty) ||
        //       (context.Request.QueryString["cropImageW"] != "0" && context.Request.QueryString["cropImageW"].Trim() != string.Empty) ||
        //       (context.Request.QueryString["cropImageH"] != "0" && context.Request.QueryString["cropImageH"].Trim() != string.Empty))
        //        {
        //            buffer = ImageShaper.Crop(buffer, int.Parse(context.Request.QueryString["cropImageW"].Trim()), int.Parse(context.Request.QueryString["cropImageH"].Trim()), int.Parse(context.Request.QueryString["cropImageX1"].Trim()), int.Parse(context.Request.QueryString["cropImageY1"].Trim()));
        //            int tempHeight = (int)((float.Parse(context.Request.QueryString["cropImageH"].Trim()) / float.Parse(context.Request.QueryString["cropImageW"].Trim())) * 160);
        //            buffer = ImageShaper.ResizeImage(buffer, 160, tempHeight);
        //        }

        //        context.Response.ContentType = "image/jpeg";
        //        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        context.Response.BufferOutput = false;
        //        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
        //    }

        //    context.Response.End();
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}