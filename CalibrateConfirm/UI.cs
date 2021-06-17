using UnityEngine;
using UnityEngine.UI;

namespace CalibrateConfirm
{
    public class UI
    {
        public class Button
        {
            public static GameObject Create(GameObject dest, string text, string desc, string name, Vector3 pos, UnityEngine.UI.Button.ButtonClickedEvent onClick)
            {
                GameObject btn = GameObject.Instantiate(GameObject.Find("UserInterface/QuickMenu/QuickModeMenus/QuickModeInviteResponseMoreOptionsMenu/BlockButton").transform, dest.transform).gameObject;
                btn.transform.localPosition = Utils.ConvertToUnityUnits(pos);
                btn.name = name;
                btn.GetComponentInChildren<Text>().text = text;
                btn.GetComponent<UnityEngine.UI.Button>().onClick = onClick;
                btn.GetComponent<UiTooltip>().field_Public_String_0 = desc;
                btn.SetActive(true);
                return btn;
            }
        }
    }
}