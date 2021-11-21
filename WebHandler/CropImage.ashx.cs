using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sibin.Imaging.TwoD;
using System.Text;
using System.Web.SessionState;

namespace e_Travel.WebHandler
{
    /// <summary>
    /// Summary description for CropImage
    /// </summary>
    public class CropImage : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["X1"] != null && context.Request.QueryString["X1"] != string.Empty && context.Request.QueryString["Y1"] != null && context.Request.QueryString["Y1"] != string.Empty && context.Request.QueryString["X2"] != null && context.Request.QueryString["X2"] != string.Empty && context.Request.QueryString["Y2"] != null && context.Request.QueryString["Y2"] != string.Empty)
            {
                int x1 = int.Parse(context.Request.QueryString["X1"]);
                int y1 = int.Parse(context.Request.QueryString["Y1"]);
                int x2 = int.Parse(context.Request.QueryString["X2"]);
                int y2 = int.Parse(context.Request.QueryString["Y2"]);

                byte[] Photograph = (byte[])HttpContext.Current.Session["ucPhotoUploaderUploadedImage"];
                Photograph = ImageShaper.Crop(Photograph, x2, y2, x1, y1);
                //HttpContext.Current.Session["ucPhotoUploaderUploadedImage"] = ImageShaper.ResizeImage(Photograph, 600, 480);
            
                HttpContext.Current.Session["ucPhotoUploaderUploadedImage"] = Photograph;
                context.Response.ContentType = "application/json";
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.Write("{ \"message\":\"Success\" }");
                context.Response.StatusCode = 200;
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