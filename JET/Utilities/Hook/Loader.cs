using UnityEngine;

namespace JET.Utilities.Hook
{
	public class Loader<T> where T : MonoBehaviour
	{
		public static GameObject HookObject
		{
			get
			{
				GameObject result = GameObject.Find("JET Instance");

				if (result == null)
				{
					result = new GameObject("JET Common Instance");
					Object.DontDestroyOnLoad(result);
				}

				return result;
			}
		}

		public static T Load()
		{
			return HookObject.GetOrAddComponent<T>();
		}
	}
}
