using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class IItems <T> //Serializable item collection
{
    public List<T> Items = new List<T>();
}

[Serializable]
public abstract class Inventory <T> : MonoBehaviour
{
    IItems<T> items = new IItems<T>(); //Inventory collection
    public event Action<List<T>> Render; //add multiple renderer implementation

    /// <summary>
    /// Inventory render call
    /// </summary>
    public void RenderCall()
    {
        Render?.Invoke(items.Items);
    }

    /// <summary>
    /// Add item to inventory
    /// </summary>
    /// <param name="obj"></param>
    public virtual void AddItem(T obj)
    {
        items.Items.Add(obj);
    }

    /// <summary>
    /// Remove Item from inventory
    /// </summary>
    /// <param name="obj"></param>
    public virtual void RemoveItem(T obj)
    {
        items.Items.Remove(obj);
    }

    string path = "";

    /// <summary>
    /// Set save information path
    /// </summary>
    /// <param name="p">path</param>
    public void SetSaveGamePath(string p)
    {
        path = p;
    }

    /// <summary>
    /// Implement this to create custom loading
    /// </summary>
    public virtual void Save() //ByDefault - JsonImplSave
    {
        string s = JsonUtility.ToJson(items);
        using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(path, false))
        {
            file.Write(s);
        }
        Debug.Log($"Success {path}");
    }

    /// <summary>
    /// Implement this to create custom saving
    /// </summary>
    public virtual void Load() //ByDefault - JsonImplRead
    {
        if (File.Exists(path))
        {
            using (System.IO.StreamReader file =
               new System.IO.StreamReader(path))
            {
                string s = file.ReadToEnd();
                items = JsonUtility.FromJson<IItems<T>>(s);
            }
            Debug.Log($"Success {path}");
        }
    }
}
