using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Game.UI
{
    public class UIUtils
    {
        public static void BalancePrefabs(GameObject prefab, int amount, Transform parent)
        {
            // instantiate until amount
            for (int i = parent.childCount; i < amount; ++i)
            {
                GameObject go = GameObject.Instantiate(prefab);
                go.transform.SetParent(parent, false);
            }

            // delete everything that's too much
            // (backwards loop because Destroy changes childCount)
            for (int i = parent.childCount-1; i >= amount; --i)
                GameObject.Destroy(parent.GetChild(i).gameObject);
        }

        // find out if any input is currently active by using Selectable.all
        // (FindObjectsOfType<InputField>() is far too slow for huge scenes)
        public static bool AnyInputActive()
        {
            // avoid Linq.Any because it is HEAVY(!) on GC and performance
            foreach (Selectable sel in Selectable.allSelectablesArray)
                if (sel is InputField inputField && inputField.isFocused)
                    return true;
            return false;
        }

        // deselect any UI element carefully
        // (it throws an error when doing it while clicking somewhere, so we have to
        //  double check)
        public static void DeselectCarefully()
        {
            if (!Input.GetMouseButton(0) &&
                !Input.GetMouseButton(1) &&
                !Input.GetMouseButton(2))
                EventSystem.current.SetSelectedGameObject(null);
        }
        public static void DestroyChildren(Transform parent)
        {
            if(parent.childCount > 0)
            {
                for(int i = 0; i < parent.childCount; i++)
                {
                    GameObject.Destroy(parent.GetChild(i).gameObject);
                }
            }
        }
        public static List<T> BalancePrefabs<T>(GameObject prefab, int amount, Transform parent) {
            if(prefab.GetComponent<T>() == null) {
                // ui show error
                Debug.Log($"prefab doesn't have script [ {nameof(T)} ] on it");
                return default;
            }
            List<T> results = new List<T>();
            // instantiate until amount
            for(int i = parent.childCount; i < amount; ++i)
                results.Add(GameObject.Instantiate(prefab, parent, false).GetComponent<T>());
            // delete everything that's too much
            // (backwards loop because Destroy changes childCount)
            for (int i = parent.childCount-1; i >= amount; --i)
                GameObject.Destroy(parent.GetChild(i).gameObject);
            return results;
        }
        public static string TimeSince(double seconds, bool useMS = false) {
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            string res = "";
            if (t.Days > 0) res += t.Days + LanguageManger.Decide("d", "ي");
            if (t.Hours > 0) res += " " + t.Hours + LanguageManger.Decide("h", "س");
            if (t.Minutes > 0) res += " " + t.Minutes + LanguageManger.Decide("m", "د");
            // 0.5s, 1.5s etc. if any milliseconds. 1s, 2s etc. if any seconds
            if (t.Seconds > 0) res += " " + t.Seconds + LanguageManger.Decide("s", "ث");
            // if the string is still empty because the value was '0', then at least
            // return the seconds instead of returning an empty string
            return res != "" ? res : "0" + LanguageManger.Decide("s", "ث");
        }
        public static string PrettySeconds(double seconds) {
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            string res = "";
            if (t.Days > 0) res += t.Days + LanguageManger.Decide("d", "ي");
            if (t.Hours > 0) res += " " + t.Hours + LanguageManger.Decide("h", "س");
            if (t.Minutes > 0) res += " " + t.Minutes + LanguageManger.Decide("m", "د");
            // 0.5s, 1.5s etc. if any milliseconds. 1s, 2s etc. if any seconds
            if (t.Seconds > 0) res += " " + t.Seconds + LanguageManger.Decide("s", "ث");
            // if the string is still empty because the value was '0', then at least
            // return the seconds instead of returning an empty string
            return res != "" ? res : "0" + LanguageManger.Decide("s", "ث");
        }
        public static T FindInParents<T>(GameObject go) where T : Component
		{
			if (go == null)
				return null;
			
			var comp = go.GetComponent<T>();
			
			if (comp != null)
				return comp;
			
			Transform t = go.transform.parent;
			
			while (t != null && comp == null)
			{
				comp = t.gameObject.GetComponent<T>();
				t = t.parent;
			}
			
			return comp;
		}
    }
}