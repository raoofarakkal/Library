using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace Library2.Cs.Web
{
    public class HttpHandlerBase : IHttpHandler, IReadOnlySessionState
    {
        protected event onProcessRequestDelegate onProcessRequest;
        protected delegate void onProcessRequestDelegate(string Command, EventArgs e);

        protected HttpContext ThisContext;

        public bool IsReusable
        {
            get { return false; }
        }

        private NameValueCollection mParameters = null;
        protected NameValueCollection Parameters
        {

            get
            {
                if (mParameters == null)
                {
                    if (ThisContext.Request.Form.Count > 0)
                    {
                        mParameters = ThisContext.Request.Form;
                        return ThisContext.Request.Form;
                    }
                    else if (ThisContext.Request.QueryString.Count > 0)
                    {
                        mParameters = ThisContext.Request.QueryString;
                        return ThisContext.Request.QueryString;
                    }
                    else
                    {
                        //For JSON Object Parsing
                        string mData = new System.IO.StreamReader(ThisContext.Request.InputStream).ReadToEnd();
                        NameValueCollection mQuery = HttpUtility.ParseQueryString(mData);
                        mParameters = mQuery;

                        if (mQuery.Count != 0)
                        {
                            return mQuery;
                        }
                        else
                        {
                            return null;
                        }


                    }

                }

                else
                {
                    return mParameters;
                }
            }
        }

        protected void Write(string str)
        {
            ThisContext.Response.Write(str);
        }

        protected class JsonResponse
        {
            public JsonResponse()
            {
            }
            public bool Success { get; set; }
            public string Error { get; set; }
            public object Results { get; set; }

            public static string CreateError(string error)
            {
                JsonResponse mRet = new JsonResponse();
                mRet.Success = false;
                mRet.Error = error;
                mRet.Results = null;
                return (new JavaScriptSerializer()).Serialize(mRet);
            }

            public static string CreateSuccess(object result)
            {
                JsonResponse mRet = new JsonResponse();
                mRet.Success = true;
                mRet.Error = null;
                mRet.Results = result;
                return (new JavaScriptSerializer()).Serialize(mRet);
            }

        }

        public void ProcessRequest(HttpContext context)
        {
            ThisContext = context;
            ThisContext.Response.ContentType = "text/plain";
            //ThisContext.Response.ContentType = "text/json";
            //System.Threading.Thread.Sleep(10000);
            if (onProcessRequest != null)
            {
                if (Parameters != null)
                {
                    if (Parameters["cmd"] != null)
                    {
                        onProcessRequest(Parameters["cmd"], null);
                    }
                }
            }

        }

    }
}
