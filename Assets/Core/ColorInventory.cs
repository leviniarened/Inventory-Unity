using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/Create", order = 1)]
[System.Serializable]
public class InventoryItem : ScriptableObject
{
    [SerializeField]
    private List<Color> colors = new List<Color>();

    public void OnValidate()
    {
        for (int i = 0; i < colors.Count; i++)
        {
            for (int j = i + 1; j < colors.Count; j++)
            {
                if (colors[i] == colors[j])
                    Debug.LogError("Colors in list is not unique");
            }
        }
    }

    public List<Color> GetList()
    {
        return colors;
    }
}

public class ColorInventory : Inventory<Color>
{
    [SerializeField]
    InventoryItem AvailableElements;

    [SerializeField]
    public List<Texture2D> list;
    Dictionary<Color, Texture2D> sprites = new Dictionary<Color, Texture2D>();


    void Start()
    {
        SetSaveGamePath(Application.persistentDataPath + @"\savegame.json");

        AvailableElements.GetList().ForEach(s =>
        {
            var tex = new Texture2D(1, 1);
            tex.SetPixel(1, 1, s);
            tex.Apply();
            tex.wrapMode = TextureWrapMode.Repeat;
            sprites.Add(s, tex);
        });

        list = sprites.Select(s => s.Value).ToList();

        Render += ColorInventory_Render1;

        Load();
    }

    private void OnDestroy()
    {
        Save();
    }

    bool createNew = false;

    void DrawBox(Rect position, Color color)
    {
        Color oldColor = GUI.color;

        GUI.color = color;
        GUI.Box(position, "");

        GUI.color = oldColor;
    }

    private void ColorInventory_Render1(List<Color> obj)
    {
        foreach (var v in obj)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(sprites[v], GUILayout.Width(32), GUILayout.Height(32) ))
            {
                RemoveItem(v);
                return;
            }
            var lastRect = GUILayoutUtility.GetLastRect();
            GUI.DrawTexture(lastRect, sprites[v]);

            GUILayout.EndHorizontal();
        }
        if (!createNew)
            if (GUILayout.Button("Add"))
                createNew = true;
        if (createNew)
        {
            GUILayout.BeginHorizontal();
            foreach (var v in AvailableElements.GetList())
            {
                if (GUILayout.Button("", GUILayout.Width(32), GUILayout.Height(32)))
                {
                    AddItem(v);
                    createNew = false;
                    return;
                }
                var lastRect = GUILayoutUtility.GetLastRect();
                GUI.DrawTexture(lastRect, sprites[v]);
            }
            if (GUILayout.Button("Cancel"))
            {
                createNew = false;
            }
            GUILayout.EndHorizontal();
        }
    }

    private void OnGUI()
    {
        RenderCall();
    }

}
