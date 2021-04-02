using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hager_crm.Utils
{
    public static class MaintainURL
    {
        /// <summary>
        /// Maintain the URL for and Index View including
        /// filter, sort and page informaiton.
        /// Works with our default route by setting the href to /Controller.
        /// </summary>
        /// <param name="_context">the HttpContext</param>
        /// <param name="ControllerName">Name of the Controller</param>
        /// <returns>The Index URL with paramerters if required</returns>
        public static string ReturnURL(HttpContext _context, string ControllerName)
        {
            string cookieName = ControllerName + "URL";
            string SearchText = "/" + ControllerName + "?";
            //Get the URL of the page that sent us here
            string returnURL = _context.Request.Headers["Referer"].ToString();
            if (returnURL.Contains(SearchText))
            {
                //Came here from the Index with some parameters
                //Save the Parameters in a Cookie
                returnURL = returnURL.Substring(returnURL.LastIndexOf(SearchText));
                CookieHelper.CookieSet(_context, cookieName, returnURL, 30);
                return returnURL;
            }
            else
            {
                //Get it from the Cookie
                //Note that this might return an empty string but we will
                //change it to go back to the Index of the Controller.
                returnURL = _context.Request.Cookies[cookieName];
                return returnURL ?? "/" + ControllerName;
            }
        }
    }
}
