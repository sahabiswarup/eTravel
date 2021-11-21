using System;
using System.Web.UI;
using System.Text;
       
    /// <summary>
    /// The types of dialog boxes availble in web scenario
    /// </summary>
    public enum DialogTypes
    {
        /// <summary>
        /// The dialog type used to display success message.
        /// </summary>
        Success = 0,

        /// <summary>
        /// The dialog type used to display error message.
        /// </summary>
        Error = 1,

        /// <summary>
        /// The dialog type used to display information.
        /// </summary>
        Information = 2,

        /// <summary>
        /// The dialog type used to display confirmation.
        /// </summary>
        Question = 3,

        /// <summary>
        /// The dialog type used to display warning.
        /// </summary>
        Warning = 4
    }

    /// <summary>
    /// Provider features to support browser replacement dialogs (Fancy Dialogs)
    /// </summary>
    public class WebMessenger
    {


        /// <summary>
        /// Show the messenger
        /// </summary>
        /// <param name="Terget">The System.Web.UI.Control</param>
        /// <param name="Message">The message to be displayed in the dialog</param>
        /// <param name="Title">The title of the message box</param>
        /// <param name="DialogType">The type of the message box</param>
        /// <param name="CallbackFunctionName">Optional client side callback function name for question dialog</param>
        public static void Show(string ipaddress, System.Web.UI.Control Target, string Message, string Title, DialogTypes DialogType, string CallbackFunctionName = "")
        {
            StringBuilder jsSnippet = new StringBuilder();
            if (DialogType == DialogTypes.Error)
            {
                //Log the Error Messages in Text File Use Log for net
                //Commented As We Are Not Using Log4net error Logging
                //LogUtil.WriteToLog("IP:" + ipaddress + "Error:" + Message, LogUtil.LogType.INFO);

                Message = System.Web.HttpUtility.HtmlEncode(Message);

                
            }
            jsSnippet.Append(@"$.Zebra_Dialog('" + Message + "',");

            if (DialogType == DialogTypes.Success)
            {
                jsSnippet.Append(@"{'type': 'confirmation',");
            }
            else if (DialogType == DialogTypes.Error)
            {
                jsSnippet.Append(@"{'type': 'error',");
            }
            else if (DialogType == DialogTypes.Information)
            {
                jsSnippet.Append(@"{'type': 'information',");
            }
            else if (DialogType == DialogTypes.Question)
            {
                jsSnippet.Append(@"{'type': 'question',");

                if (!string.IsNullOrEmpty(CallbackFunctionName) && !string.IsNullOrWhiteSpace(CallbackFunctionName))
                {
                    jsSnippet.Append(@"'onClose':  function(caption) { " + CallbackFunctionName + "(caption); },");
                }
            }
            else if (DialogType == DialogTypes.Warning)
            {
                jsSnippet.Append(@"{'type': 'warning',");
            }
            else
            {
                throw new Exception("Invalid Messenger Settings!, Please configure messenger properly");
            }

            jsSnippet.Append(@"'title': '" + Title + "'});");


            ScriptManager.RegisterStartupScript(Target, Target.GetType(), Guid.NewGuid().ToString(), jsSnippet.ToString(), true);
        }

        public static void Show(System.Web.UI.Control Target, string Message, string Title, DialogTypes DialogType, string CallbackFunctionName = "")
        {
            StringBuilder jsSnippet = new StringBuilder();
            if (DialogType == DialogTypes.Error)
            {
                
                Message = System.Web.HttpUtility.HtmlEncode(Message);


            }
            jsSnippet.Append(@"$.Zebra_Dialog('" + Message + "',");

            if (DialogType == DialogTypes.Success)
            {
                jsSnippet.Append(@"{'type': 'confirmation',");
            }
            else if (DialogType == DialogTypes.Error)
            {
                jsSnippet.Append(@"{'type': 'error',");
            }
            else if (DialogType == DialogTypes.Information)
            {
                jsSnippet.Append(@"{'type': 'information',");
            }
            else if (DialogType == DialogTypes.Question)
            {
                jsSnippet.Append(@"{'type': 'question',");

                if (!string.IsNullOrEmpty(CallbackFunctionName) && !string.IsNullOrWhiteSpace(CallbackFunctionName))
                {
                    jsSnippet.Append(@"'onClose':  function(caption) { " + CallbackFunctionName + "(caption); },");
                }
            }
            else if (DialogType == DialogTypes.Warning)
            {
                jsSnippet.Append(@"{'type': 'warning',");
            }
            else
            {
                throw new Exception("Invalid Messenger Settings!, Please configure messenger properly");
            }

            jsSnippet.Append(@"'title': '" + Title + "'});");


            ScriptManager.RegisterStartupScript(Target, Target.GetType(), Guid.NewGuid().ToString(), jsSnippet.ToString(), true);
        }

    }

