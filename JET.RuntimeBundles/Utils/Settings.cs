using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;
using JET.Common.Utils.HTTP;
/*
namespace JET.RuntimeBundles.Utils
{
    public class Settings
    {
		private static string Session;
		private static string BackendUrl;
        public static readonly string cachePach = "Cache/StreamingAssets/windows/";
        public readonly static Dictionary<string, BundleInfo> bundles = new Dictionary<string, BundleInfo>();

		public Settings(string session, string backendUrl)
		{
            Session = session;
            BackendUrl = backendUrl;

            Init(); 
        }

		private void Init()
		{
            try
            {
                CleanCache();
            } catch
            {
                Debug.LogError("JET.RuntimeBundles: The cache cleanup failed and will try again at the next game startup.");
            }
            
            string json = new Request(Session, BackendUrl).GetJson("/singleplayer/bundles/");
            if (string.IsNullOrEmpty(json))
			{
				Debug.LogError("JET.RuntimeBundles: Bundles data is Null, using fallback");
				return;
			}

            var jArray = JArray.Parse(json);
            foreach (var jObj in jArray)
            {
                BundleInfo bundle;
                if (!bundles.TryGetValue(jObj["key"].ToString(), out bundle))
                {
                    bundle = new BundleInfo(jObj["key"].ToString(), jObj["path"].ToString(), jObj["dependencyKeys"].ToObject<List<string>>().ToArray());
                    bundles.Add(bundle.Key, bundle);
                }
            }
            
            Debug.LogError("JET.RuntimeBundles: Successfully received Bundles");
		}

        private void CleanCache()
        {
            if (Directory.Exists(cachePach))
            {
                Directory.Delete(cachePach, true);
            }
        }
    }

    public class BundleInfo
    {
        public string Key { get;}
        public string Path { get;}
        public string[] DependencyKeys { get;}

        public BundleInfo(string key, string path, string[] dependencyKeys)
        {
            Key = key;
            Path = path;
            DependencyKeys = dependencyKeys;
        }
    }
}*/
