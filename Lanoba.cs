using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

/// <summary>
/// http://www.lanoba.com/
/// </summary>
class Lanoba
{
    private string apiSecret;

    public Lanoba(string apiSecret)
    {
        this.apiSecret = apiSecret;
    }

    public dynamic Authenticate(string token)
    {
        return Post("authenticate", string.Format("api_secret={0}&token={1}", apiSecret, token));
    }

    public dynamic Profiles(string userId)
    {
        return Post("profiles", string.Format("api_secret={0}&user_id={1}", apiSecret, userId));
    }

    public dynamic Map(string userId, string primaryKey)
    {
        return Post("map", string.Format("api_secret={0}&user_id={1}&primary_key={2}", apiSecret, userId, primaryKey));
    }

    public dynamic Share(string userId, string message, string title, string description, string link)
    {
        return Post("share", string.Format("api_secret={0}&user_id={1}&message={2}&title={3}&description={4}&link={5}", apiSecret, userId, message, title, description, link));
    }

    public dynamic Unmap(string userId, string primaryKey)
    {
        return Post("unmap", string.Format("api_secret={0}&user_id={1}", apiSecret, userId));
    }

    private dynamic Post(string method, string parameters)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(parameters);

        WebRequest request = WebRequest.Create("https://api.lanoba.com/" + method);
        request.ContentLength = bytes.Length;
        request.ContentType = "application/x-www-form-urlencoded";
        request.Method = "POST";

        try
        {
            request.GetRequestStream().Write(bytes, 0, bytes.Length);
            string response = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<dynamic>(response);
        }
        catch (WebException e)
        {
            // handle exception
            return null;
        }
    }
}
