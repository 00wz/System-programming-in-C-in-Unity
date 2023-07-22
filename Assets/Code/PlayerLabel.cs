using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace UI
{
    public class PlayerLabel : MonoBehaviour
    {
        public void DrawLabel(Camera camera)
        {
            if (camera == null)
            {
                return;
            }
            var style = new GUIStyle();
            style.normal.background = Texture2D.redTexture;
            style.normal.textColor = Color.blue;
            
            var objects = NetworkManager.Singleton.SpawnManager.SpawnedObjectsList;
            for (int i = 0; i < objects.Count; i++)
            {
                var obj = objects.ElementAt(i);
                var position =camera.WorldToScreenPoint(obj.transform.position);
                if (IsVisible(position)&&obj.transform != transform)
                {
                    GUI.Label(new Rect(new Vector2(position.x, Screen.height -
                    position.y), new Vector2(10, name.Length * 10.5f)),
                    obj.name, style);
                }
            }
        }
        private bool IsVisible(Vector3 position)
        {
            return position.x > 0 && position.x < Screen.width &&
                position.y > 0 && position.y < Screen.height&&
                position.z>0;
        }
    }
}
