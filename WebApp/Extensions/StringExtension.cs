﻿using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebApp.Extensions
{
    public static class StringExtension
    {
        public static string GenerateAppSecretProof(this String accessToken)
        {
            //Creates a Facebook appsecret_proof value to be used for each graph api call when appsecret_proof has been enabled for the facebook app
            //Facebook appsecret_proof is SHA256 encrypted string of the current facebook access token using the facebook app secret value as the private key
            using (HMACSHA256 algorithm = new HMACSHA256(Encoding.ASCII.GetBytes(AppConfiguration.Facebook_AppSecret)))
            {
                byte[] hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(accessToken));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
                }
                return builder.ToString();
            }
        } 

        public static string GraphAPICall(this string baseGraphApiCall, params object[] args)
        {
            //returns a formatted Graph Api Call with a version prefix and appends a query string parameter containing the appsecret_proof value
            if (!string.IsNullOrEmpty(baseGraphApiCall))
            {
                if (args != null &&
                    args.Count() > 0)
                {
                    //Determine if we need to concatenate appsecret_proof query string parameter or inject it as a single query string paramter
                    string _graphApiCall = string.Empty;
                    if (baseGraphApiCall.Contains("?"))
                        _graphApiCall = string.Format(baseGraphApiCall + "&appsecret_proof={" + (args.Count() - 1) + "}", args);
                    else
                        _graphApiCall = string.Format(baseGraphApiCall + "?appsecret_proof={" + (args.Count() - 1) + "}", args);

                    //prefix with Graph API Version
                    return string.Format("v2.4/{0}", _graphApiCall);
                }
                else
                    throw new Exception("GraphAPICall requires at least one string parameter that contains the appsecret_proof value.");
            }
            else
                return string.Empty;
        }
    }
}