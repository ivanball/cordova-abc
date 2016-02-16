using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using WebApp.Extensions;
using WebApp.Filters;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Configuration;
using WebApp;
using WebApp.Models;
using Common;

namespace WebApp.Controllers
{
    [Authorize]
    [FacebookAccessTokenAttribute]
    public class FacebookController : Controller
    {

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("ExternalCallBack","Facebook");
                return uriBuilder.Uri;
            }
        }

        private RedirectResult GetFacebookLoginURL()
        {

            if  (Session["AccessTokenRetryCount"] == null ||
                (Session["AccessTokenRetryCount"] != null &&
                 Session["AccessTokenRetryCount"].ToString() == "0"))
            {
                Session.Add("AccessTokenRetryCount", "1");

                FacebookClient fb = new FacebookClient();
                fb.AppId = AppConfiguration.Facebook_AppID;
                return Redirect(fb.GetLoginUrl(new
                {
                    scope = AppConfiguration.Facebook_Scope,
                    redirect_uri = RedirectUri.AbsoluteUri,
                    response_type = "code"
                }).ToString());
            }
            else
            {
                ViewBag.ErrorMessage = "Unable to obtain a valid Facebook Token, contact support";
                return Redirect(Url.Action("Index", "Error"));
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is FacebookApiLimitException)
            {
                //Status message banner notifying user to try again later
                filterContext.ExceptionHandled = true;
                ViewBag.GlobalStatusMessage = "Facebook Graph API limit reached, Please try again later...";
            }
            else if (filterContext.Exception is FacebookOAuthException)
            {
                FacebookOAuthException OAuth_ex = (FacebookOAuthException)filterContext.Exception;
                if (OAuth_ex.ErrorCode==190 || OAuth_ex.ErrorSubcode > 0)
                {
                    filterContext.ExceptionHandled = true;
                    filterContext.Result = GetFacebookLoginURL();
                }
                else
                {
                    //redirect to Facebook Custom Error Page
                    ViewBag.ErrorMessage = filterContext.Exception.Message;
                    filterContext.ExceptionHandled = true;
                    filterContext.Result = RedirectToAction("Index", "Error");
                }

            }
            else if (filterContext.Exception is FacebookApiException)
            {
                //redirect to Facebook Custom Error Page
                ViewBag.ErrorMessage = filterContext.Exception.Message;
                filterContext.ExceptionHandled = true;
                filterContext.Result = RedirectToAction("Index", "Error");
            }
            else
                base.OnException(filterContext);
        }

        public async Task<ActionResult> ExternalCallBack (string code)
        {
            //Callback return from Facebook will include a unique login encrypted code
            //for this user's login with our application id
            //that we can use to obtain a new access token
            FacebookClient fb = new FacebookClient();
            
            //Exchange encrypted login code for an access_token
            dynamic newTokenResult = await fb.GetTaskAsync(
                                        string.Format("oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}",
                                        AppConfiguration.Facebook_AppID,
                                        Url.Encode(RedirectUri.AbsoluteUri),
                                        AppConfiguration.Facebook_AppSecret,
                                        code));
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if (UserManager != null)
            {
                // Retrieve the existing claims for the user and add the FacebookAccessTokenClaim 
                var userId = HttpContext.User.Identity.GetUserId();
               
                IList<Claim> currentClaims = await UserManager.GetClaimsAsync(userId);
               
                //check to see if a claim already exists for FacebookAccessToken
                Claim OldFacebookAccessTokenClaim = currentClaims.First(x => x.Type == "FacebookAccessToken"); 

                //Create new FacebookAccessToken claim
                Claim newFacebookAccessTokenClaim  = new Claim("FacebookAccessToken", newTokenResult.access_token); 
                if (OldFacebookAccessTokenClaim == null)
                {
                    //Add new FacebookAccessToken Claim
                    await UserManager.AddClaimAsync(userId, newFacebookAccessTokenClaim);
                }
                else
                {
                    //Remove the existing FacebookAccessToken Claim
                    await UserManager.RemoveClaimAsync(userId, OldFacebookAccessTokenClaim); 
                    //Add new FacebookAccessToken Claim
                    await UserManager.AddClaimAsync(userId, newFacebookAccessTokenClaim);    
                }
                Session.Add("AccessTokenRetryCount", "0");
            }
            
            return RedirectToAction("Index");
        }

        // GET: Facebook
        public async Task<ActionResult> Index()
        {
                var access_token = HttpContext.Items["access_token"].ToString();
                //try
                //{
                    var appsecret_proof = access_token.GenerateAppSecretProof();

                    //string _tempAccessToken = string.Empty;
                    //if (Session["NewAccessToken"] == null)
                    //{
                    //    _tempAccessToken = access_token + "abc";
                    //}
                    //else
                    //{
                    //    _tempAccessToken = access_token;
                    //}
                    var fb = new FacebookClient(access_token);

                    //Get current user's profile
                    dynamic myInfo = await fb.GetTaskAsync("me?fields=first_name,last_name,link,locale,email,name,birthday,gender,location,bio,age_range".GraphAPICall(appsecret_proof));               

                    //get current picture
                    dynamic profileImgResult = await fb.GetTaskAsync("{0}/picture?width=100&height=100&redirect=false".GraphAPICall((string)myInfo.id, appsecret_proof));

                    //Hydrate FacebookProfileViewModel with Graph API results
                    var facebookProfile = DynamicExtension.ToStatic<FacebookProfileViewModel>(myInfo);
                    facebookProfile.ImageURL = profileImgResult.data.url;
                    return View(facebookProfile);
                //}
                //catch (FacebookApiLimitException ex)
                //{
                //    throw new HttpException(500, ex.Message);
                //}
                //catch (FacebookOAuthException ex)
                //{
                //    throw new HttpException(500, ex.Message);
                //}
                //catch (FacebookApiException ex)
                //{
                //    throw new HttpException(500, ex.Message);
                //}
                //catch (Exception ex)
                //{
                //    throw new HttpException(500, ex.Message);
                //}


        }

        public async Task<ActionResult> FB_TaggableFriends()
        {
            var access_token = HttpContext.Items["access_token"].ToString();
            if (access_token != null)
            {
                var appsecret_proof = access_token.GenerateAppSecretProof();

                var fb = new FacebookClient(access_token);
                dynamic myInfo = await fb.GetTaskAsync("me/taggable_friends?limit=5000".GraphAPICall(appsecret_proof));
                var friendsList = new List<FacebookFriendViewModel>();
                foreach (dynamic friend in myInfo.data)
                {
                    FacebookFriendViewModel facebookFriendViewModel = DynamicExtension.ToStatic<FacebookFriendViewModel>(friend);
                    /*
                    dynamic friendInfo = await fb.GetTaskAsync((facebookFriendViewModel.TaggingId).GraphAPICall(appsecret_proof));
                    FacebookProfileViewModel facebookProfile = DynamicExtension.ToStatic<FacebookProfileViewModel>(friendInfo);
                    facebookFriendViewModel.facebookProfileViewModel = facebookProfile;
                    */
                    friendsList.Add(facebookFriendViewModel);
                }

                friendsList.Sort(delegate (FacebookFriendViewModel x, FacebookFriendViewModel y)
                {
                    if (x.Name == null && y.Name == null) return 0;
                    else if (x.Name == null) return -1;
                    else if (y.Name == null) return 1;
                    else return x.Name.CompareTo(y.Name);
                });

                return PartialView(friendsList);
            }
            else
                throw new HttpException(404, "Missing Access Token");
        }

        public async Task<ActionResult> FB_AdminPages()
        {
            var access_token = HttpContext.Items["access_token"].ToString();
            if (access_token != null)
            {
                var appsecret_proof = access_token.GenerateAppSecretProof();

                var fb = new FacebookClient(access_token);
                dynamic myPages = await fb.GetTaskAsync(
                    "me/accounts?fields=id, name, link, is_published, likes, talking_about_count"
                    .GraphAPICall(appsecret_proof));
                var pageList = new List<FacebookPageViewModel>();
                foreach (dynamic page in myPages.data)
                {

                    pageList.Add(DynamicExtension.ToStatic<FacebookPageViewModel>(page));
                }

                return PartialView(pageList);
            }
            else
                throw new HttpException(404, "Missing Access Token");
        }

 
    }
}