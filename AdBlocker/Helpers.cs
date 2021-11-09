using System.Linq;
using UnityEngine;

namespace AdBlocker
{
    public static class Helpers
    {
        public static GameObject FindObject(this GameObject parent, string name)
        {
            Transform[] tfs = parent.GetComponentsInChildren<Transform>(true);
            return (from t in tfs where t.name == name select t.gameObject).FirstOrDefault();
        }
    }
}