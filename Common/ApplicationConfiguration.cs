using System;
using System.Configuration;

namespace Common
{
    /// <summary>
    /// Repository for configuration settings
    /// </summary>
    public static class AppConfiguration
    {
        static AppConfiguration()
        {
        }

        public static string SMTPServerAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPServerAddress"];
            }
        }

        public static int SMTPPort
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
            }
        }

        public static string SMTPFrom
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPFrom"];
            }
        }

        public static string SMTPUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["SMTPUsername"];
            }
        }

        public static string SMTPPassword
        {
            get
            {
                return Encryptor.Decrypt(ConfigurationManager.AppSettings["SMTPPassword"]);
            }
        }

        public static bool SMTPEnableSSL
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["SMTPEnableSSL"]);
            }
        }

        // Send emails?
        public static bool EnableEmail
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["EnableEmail"]);
            }
        }

        // Send error log emails?
        public static bool EnableErrorLogEmail
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["EnableErrorLogEmail"]);
            }
        }

        // Returns the email address where to send error reports
        public static string ErrorLogEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["ErrorLogEmail"];
            }
        }

        // Returns the email address where to send Application notifications
        public static string ApplicationNotificationEmail
        {
            get
            {
                return ConfigurationManager.AppSettings["ApplicationNotificationEmail"];
            }
        }

        // Returns the Site name
        public static string SiteName
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteName"];
            }
        }

        // Returns the Application name
        public static string ApplicationName
        {
            get
            {
                return ConfigurationManager.AppSettings["ApplicationName"];
            }
        }

        // Returns the Pass phrase
        public static string PassPhrase
        {
            get
            {
                return ConfigurationManager.AppSettings["PassPhrase"];
            }
        }

        // Returns the output directory
        public static string OutputDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["OutputDirectory"];
            }
        }

        // Returns the inetpub output directory
        public static string inetpubOutputDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["inetpubOutputDirectory"];
            }
        }

        // Returns the Facebook App ID
        public static string Facebook_AppID
        {
            get
            {
                return ConfigurationManager.AppSettings["Facebook_AppID"];
            }
        }

        // Returns the Facebook App Secret
        public static string Facebook_AppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["Facebook_AppSecret"];
            }
        }

        // Returns the Facebook Scope
        public static string Facebook_Scope
        {
            get
            {
                return ConfigurationManager.AppSettings["Facebook_Scope"];
            }
        }
    }
}