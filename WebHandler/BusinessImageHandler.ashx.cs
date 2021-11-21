using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace e_Travel.WebHandler
{
    /// <summary>
    /// Summary description for BusinessImageHandler
    /// </summary>
    public class BusinessImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            // Set up the response settings
            context.Response.ContentType = "image";
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.BufferOutput = false;
            BusinessMstBLL objBusMst = new BusinessMstBLL();
            byte[] objImageData = null;
            if (string.IsNullOrEmpty(context.Request.QueryString["BID"]) == false && string.IsNullOrEmpty(context.Request.QueryString["IMG"]) == false)
            {
                //Fetch Business Logo
                if (context.Request.QueryString["IMG"] == "BL")
                {
                    objImageData = objBusMst.GetBusinessLogoByBusID(context.Request.QueryString["BID"].ToString());
                }
                //Fetch Social Network Icon
                else if (context.Request.QueryString["IMG"] == "SN")
                {
                    if (string.IsNullOrEmpty(context.Request.QueryString["FID"]) == false)
                    {
                        
                    }
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