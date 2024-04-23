using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class Database 
{
    [Serializable]
    public class PackageInfo
    {
        
        public  string package_id;
        public  string package_name;
        public  int package_price;
        public  string package_icon;
        public  int package_discount;
        public  bool package_global_discount_accepted;
        public  bool package_is_bought;
        public PreviewInfo[] package_previews;
        public DownloadInfo[] package_downloads;

    }
    [Serializable]
    public class PreviewInfo
    {
        public  string preview_name;
        public  string preview_youtube_url;
        public  string preview_aparat_url;

    }

    [Serializable]
    public class DownloadInfo
    {
        public  string download_name;
        public  string download_url;
        
    }
    [Serializable]
    public  class Packages
    {
        public PackageInfo[] all_packages;
    }


    public static Packages SPSSLandPackes;
    public static void LoadData()
    {

        // Loading Demos
        TextAsset jdata = Resources.Load<TextAsset>("packages");
        
        SPSSLandPackes = JsonUtility.FromJson<Packages>(jdata.text);

        
    }
    public static void LoadData(string iDataToLoad)
    {

        // Loading Demos
        
        
        SPSSLandPackes = JsonUtility.FromJson<Packages>(iDataToLoad);

        
    }
    public static bool _IsFree(string iPackageID)
    {
        bool isFree = false;
        foreach(PackageInfo pi in SPSSLandPackes.all_packages)
        {
            if (pi.package_id.Equals(iPackageID))
            {
                if(pi.package_discount == 100)
                {
                    isFree = true;
                    break;
                }
            }
        }
        return isFree;
    }


}
