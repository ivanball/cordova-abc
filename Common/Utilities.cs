using System;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net.Mime;

namespace Common
{
    public class Utilities
    {
        // Send error log mail
        public static void LogError(Exception ex)
        {
            // send error email in case the option is activated in Web.Config
            if (AppConfiguration.EnableErrorLogEmail)
            {
                string strFrom = AppConfiguration.SMTPFrom;
                string strTo = AppConfiguration.ErrorLogEmail;
                string strSubject = AppConfiguration.SiteName + " error report";
                string strBody = GetLogErrorMessage(ex);
                SendMail(strFrom, strTo, strSubject, strBody);
            }
        }

        /// <summary>
        /// Get Detail log messages
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string GetLogErrorMessage(Exception exception)
        {
            try
            {
                // get the current date and time
                string strDateTime = DateTime.Now.ToLongDateString() + ", at " + DateTime.Now.ToString("hh:mm:ss");
                // stores the error message
                string strErrorMessage = "Exception generated on " + strDateTime;
                // obtain the page that generated the error
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                if (context != null)
                {
                    strErrorMessage += "\n\n RawUrl: " + context.Request.RawUrl;
                    strErrorMessage += "\n\n Absolute Uri: " + context.Request.Url.AbsoluteUri;
                    strErrorMessage += "\n\n Host: " + context.Request.Url.Host;
                    strErrorMessage += "\n\n HostNameType: " + context.Request.Url.HostNameType;
                    strErrorMessage += "\n\n UrlReferrer: " + context.Request.UrlReferrer;
                    strErrorMessage += "\n\n UserAgent: " + context.Request.UserAgent;
                    strErrorMessage += "\n\n UserHostAddress: " + context.Request.UserHostAddress;
                    strErrorMessage += "\n\n UserHostName: " + context.Request.UserHostName;
                    strErrorMessage += "\n\n Logged user: " + context.User.Identity.Name;
                    strErrorMessage += "\n\n ClientIPAddress: " + context.Request.ServerVariables["HTTP_CLIENT_IP"];
                }
                // build the error message
                strErrorMessage += "\n\n Message: " + exception.Message;
                strErrorMessage += "\n\n Source: " + exception.Source;
                strErrorMessage += "\n\n Method: " + exception.TargetSite;
                strErrorMessage += "\n\n Stack Trace: \n\n" + exception.StackTrace;
                if (exception.InnerException != null)
                {
                    strErrorMessage += "\n\n Inner Message: " + exception.InnerException.Message;
                    strErrorMessage += "\n\n Inner Source: " + exception.InnerException.Source;
                    strErrorMessage += "\n\n Inner Method: " + exception.InnerException.TargetSite;
                    strErrorMessage += "\n\n Inner Stack Trace: \n\n" + exception.InnerException.StackTrace;
                }

                string strNewLine = "\n";

                if (context != null)
                {
                    System.Web.HttpBrowserCapabilities browser = context.Request.Browser;

                    string strBrowserType = browser.Type;
                    string strBrowserName = browser.Browser;
                    string strBrowserVersion = browser.Version;
                    int intBrowserMajorVersion = browser.MajorVersion;
                    string strBrowserMinorVersionString = browser.MinorVersionString;
                    string strBrowserPlatform = browser.Platform;
                    string strBrowserIsMobileDevice = (browser.IsMobileDevice == true ? "Y" : "N");
                    string strBrowserMobileDeviceModel = browser.MobileDeviceModel;
                    string strBrowserMobileDeviceManufacturer = browser.MobileDeviceManufacturer;
                    string strBrowserEcmaScriptVersion = browser.EcmaScriptVersion.ToString();
                    string strBrowserJavaScriptVersion = browser["JavaScriptVersion"];
                    string strBrowserIsAOL = (browser.AOL == true ? "Y" : "N");
                    string strBrowserIsBeta = (browser.Beta == true ? "Y" : "N");
                    string strBrowserIsWebCrawler = (browser.Crawler == true ? "Y" : "N");
                    string strBrowserIsWin16 = (browser.Win16 == true ? "Y" : "N");
                    string strBrowserIsWin32 = (browser.Win32 == true ? "Y" : "N");
                    string strBrowserSupportsActiveXControls = (browser.ActiveXControls == true ? "Y" : "N");
                    string strBrowserSupportsCookies = (browser.Cookies == true ? "Y" : "N");
                    string strBrowserSupportsFrames = (browser.Frames == true ? "Y" : "N");
                    string strBrowserSupportsJavaApplets = (browser.JavaApplets == true ? "Y" : "N");
                    string strBrowserSupportsTables = (browser.Tables == true ? "Y" : "N");
                    string strBrowserSupportsVBScript = (browser.VBScript == true ? "Y" : "N");
                    int intBrowserScreenPixelsWidth = browser.ScreenPixelsWidth;
                    int intBrowserScreenPixelsHeight = browser.ScreenPixelsHeight;

                    string strBrowserCapabilities =
                        strNewLine + "Type = " + strBrowserType
                        + strNewLine + "Name = " + strBrowserName
                        + strNewLine + "Version = " + strBrowserVersion
                        + strNewLine + "Major Version = " + intBrowserMajorVersion.ToString()
                        + strNewLine + "Minor Version = " + strBrowserMinorVersionString
                        + strNewLine + "Platform = " + strBrowserPlatform
                        + strNewLine + "Is Mobile Device = " + strBrowserIsMobileDevice
                        + strNewLine + "Mobile Device Model = " + strBrowserMobileDeviceModel
                        + strNewLine + "Mobile Device Manufacturer = " + strBrowserMobileDeviceManufacturer
                        + strNewLine + "Ecma Script Version = " + strBrowserEcmaScriptVersion
                        + strNewLine + "JavaScript Version = " + browser["JavaScriptVersion"]
                        + strNewLine + "Is AOL = " + strBrowserIsAOL
                        + strNewLine + "Is Beta = " + strBrowserIsBeta
                        + strNewLine + "Is Web Crawler = " + strBrowserIsWebCrawler
                        + strNewLine + "Is Win16 = " + strBrowserIsWin16
                        + strNewLine + "Is Win32 = " + strBrowserIsWin32
                        + strNewLine + "Supports ActiveX Controls = " + strBrowserSupportsActiveXControls
                        + strNewLine + "Supports Cookies = " + strBrowserSupportsCookies
                        + strNewLine + "Supports Frames = " + strBrowserSupportsFrames
                        + strNewLine + "Supports Java Applets = " + strBrowserSupportsJavaApplets
                        + strNewLine + "Supports Tables = " + strBrowserSupportsTables
                        + strNewLine + "Supports VBScript = " + strBrowserSupportsVBScript
                        + strNewLine + "Screen Pixels Width = " + intBrowserScreenPixelsWidth.ToString()
                        + strNewLine + "Screen Pixels Height = " + intBrowserScreenPixelsHeight.ToString();

                    strErrorMessage += "\n\nBrowser Capabilities:" + strBrowserCapabilities;
                }

                return strErrorMessage;
            }
            catch (Exception ex)
            {
                return "Could not collect ERROR - Log Information. MESSAGE: " + ex.Message + " STACKTRACE: " + ex.StackTrace;
            }
        }

        // Generic method for sending emails
        public static void SendMail(string strTo, string strBody)
        {
            SendMail(null, strTo, null, null, strBody, null, false, null);
        }

        // Generic method for sending emails
        public static void SendMail(string strFrom, string strTo, string strSubject, string strBody)
        {
            SendMail(strFrom, strTo, null, strSubject, strBody, null, false, null);
        }

        // Generic method for sending emails
        public static void SendMail(string strFrom, string strTo, string strSubject, string strBody, bool blnIsBodyHtml, string strBcc)
        {
            SendMail(strFrom, strTo, null, strSubject, strBody, null, blnIsBodyHtml, strBcc);
        }

        // Generic method for sending emails
        public static void SendMail(string strFrom, string strTo, string strCc, string strSubject, string strBody)
        {
            SendMail(strFrom, strTo, strCc, strSubject, strBody, null, false, null);
        }

        // Generic method for sending emails
        public static void SendMail(string strFrom, string strTo, string strCc, string strSubject, string strBody, string strAttachment, bool blnIsBodyHtml, string strBcc)
        {
            try
            {
                if (AppConfiguration.EnableEmail)
                {
                    // Set from field if not specified
                    if (String.IsNullOrEmpty(strFrom))
                    {
                        strFrom = AppConfiguration.SMTPFrom;
                    }

                    MailAddress madFrom = new MailAddress(strFrom);
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = madFrom;

                    // Set to field if not specified
                    if (String.IsNullOrEmpty(strTo))
                    {
                        string[] addrs = AppConfiguration.ApplicationNotificationEmail.Split(';');
                        foreach (string addr in addrs)
                        {
                            if (!String.IsNullOrEmpty(addr)) mailMessage.To.Add(addr);
                        }
                    }
                    else
                    {
                        string[] addrs = strTo.Split(';');
                        foreach (string addr in addrs)
                        {
                            if (!String.IsNullOrEmpty(addr)) mailMessage.To.Add(addr);
                        }
                    }

                    // Add cc field if exists
                    if (!String.IsNullOrEmpty(strCc))
                    {
                        string[] addrs = strCc.Split(';');
                        foreach (string addr in addrs)
                        {
                            if (!String.IsNullOrEmpty(addr)) mailMessage.CC.Add(addr);
                        }
                    }

                    // Add Bcc field if exists
                    if (!String.IsNullOrEmpty(strBcc))
                    {
                        string[] addrs = strBcc.Split(';');
                        foreach (string addr in addrs)
                        {
                            if (!String.IsNullOrEmpty(addr)) mailMessage.Bcc.Add(addr);
                        }
                    }

                    // Set subject field if not specified
                    if (String.IsNullOrEmpty(strSubject))
                    {
                        strSubject = AppConfiguration.SiteName + " message";
                    }
                    mailMessage.Subject = strSubject;

                    mailMessage.Body = strBody;
                    mailMessage.IsBodyHtml = blnIsBodyHtml;

                    // Create  the file attachment for this e-mail message if not null
                    if (!String.IsNullOrEmpty(strAttachment))
                    {
                        Attachment data = new Attachment(strAttachment, MediaTypeNames.Application.Pdf);
                        //Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                        // Add time stamp information for the file.
                        //ContentDisposition disposition = data.ContentDisposition;
                        //disposition.CreationDate = Now;
                        //disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                        //disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                        // Add the file attachment to this e-mail message.
                        mailMessage.Attachments.Add(data);
                    }

                    /*
                    // For SMTP servers that require authentication
                    message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", 1);
                    message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "SmtpHostUserName");
                    message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "SmtpHostPassword");
                    */
                    //mailClient.Credentials = new NetworkCredential("user", "password"); 

                    SmtpClient mailClient = new SmtpClient();
                    mailClient.Host = AppConfiguration.SMTPServerAddress;
                    mailClient.Port = AppConfiguration.SMTPPort;
                    mailClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    mailClient.Credentials = new System.Net.NetworkCredential(AppConfiguration.SMTPUsername, AppConfiguration.SMTPPassword);
                    mailClient.EnableSsl = AppConfiguration.SMTPEnableSSL;
                    mailClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                ex.Source = AppConfiguration.SiteName + ": " + ex.Source;
                /*
                TextWriter twLog = new StreamWriter(Path.Combine(AppConfiguration.OutputDirectory, "SendMailException.txt"), true);
                twLog.WriteLine(DateTime.Now + " SendMail. Parameters:");
                twLog.WriteLine("From: " + strFrom);
                twLog.WriteLine("To: " + strTo);
                twLog.WriteLine("Cc: " + strCc);
                twLog.WriteLine("Subject: " + strSubject);
                twLog.WriteLine("Body: " + strBody);
                twLog.WriteLine("Attachment: " + strAttachment);
                twLog.WriteLine("IsBodyHtml: " + blnIsBodyHtml.ToString());
                twLog.WriteLine("Bcc: " + strBcc);
                twLog.WriteLine("Exception Message: " + ex.Message);
                twLog.WriteLine("Exception Source: " + ex.Source);
                twLog.WriteLine("Exception Method: " + ex.TargetSite);
                twLog.WriteLine("Exception Stack Trace: " + ex.StackTrace);
                twLog.WriteLine();
                twLog.Close();
                 */
                throw;
            }
        }
    }

    public class MyHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500)
            {
                return;
            }

            if (!ExceptionType.IsInstanceOfType(filterContext.Exception))
            {
                return;
            }

            // if the request is AJAX return JSON else view.
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        error = true,
                        message = filterContext.Exception.Message
                    }
                };
            }
            else
            {
                var controllerName = (string)filterContext.RouteData.Values["controller"];
                var actionName = (string)filterContext.RouteData.Values["action"];
                var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

                filterContext.Result = new ViewResult
                {
                    ViewName = View,
                    MasterName = Master,
                    ViewData = new ViewDataDictionary(model),
                    TempData = filterContext.Controller.TempData
                };
            }

            // log the error by using your own method
            Utilities.LogError(filterContext.Exception);

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;

            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }

    public class Encryptor
    {
        public static string Decrypt(string cipherText)
        {
            if (String.IsNullOrEmpty(cipherText)) return "";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_Pwd, _Salt);
            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        private static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = null;
            try
            {
                Rijndael alg = Rijndael.Create();
                alg.Key = Key;
                alg.IV = IV;
                cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(cipherData, 0, cipherData.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
            catch
            {
                return null;
            }
            finally
            {
                cs.Close();
            }
        }

        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_Pwd, _Salt);
            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return Convert.ToBase64String(encryptedData);
        }


        private static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = null;
            try
            {
                Rijndael alg = Rijndael.Create();
                alg.Key = Key;
                alg.IV = IV;
                cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(clearData, 0, clearData.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
            catch
            {
                return null;
            }
            finally
            {
                cs.Close();
            }
        }

        static string _Pwd = "isstaff"; //Be careful storing this in code unless it’s secured and not distributed
        static byte[] _Salt = new byte[] { 0x45, 0xF1, 0x61, 0x6e, 0x20, 0x00, 0x65, 0x64, 0x76, 0x65, 0x64, 0x03, 0x76 };
    }

}
