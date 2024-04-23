using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
public static class ab 
{

    public static string ConnectingText = "در حال اتصال...";
    public static string NotConnectedText = "اینترنت وصل نیست!  تلاش مجدد";
    public static string StartText = "ورود به برنامه";
    public static string TomanText = "تومان";
    public static string DiscountText = "تخفیف";
    public static string AfterDiscountText = "قیمت فعلی:";
   
    public static string PackageURL = "https://spssland.ir/MobileApp/list.txt";
    public static string PackageIconURLPrefix = "https://spssland.ir/MobileApp/";

    public static string IsPackageBoughtTag = "PackageBought";
    public static string IsRatingDoneTag = "isRatingDone";
    
    
    public static int MaxNumberOfDownloadRetry = 10;
    public static string ShowRatingURL = "myket://comment?id="+Application.identifier;
    public static string ShareText = "SPSS Land Free Learning Platform: \n https://myket.ir/app/" + Application.identifier;
    public static string ShareSubject = "آموزش رایگان SPSS و پایان نامه نویسی"; 
    public static string ShareChooserTitle = "Send SPSS Land to...";
    
    public static string GetHtmlFromUri(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        //We are limiting the array to 80 so we don't have
                        //to parse the entire html document feel free to 
                        //adjust (probably stay under 300)
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }
}
