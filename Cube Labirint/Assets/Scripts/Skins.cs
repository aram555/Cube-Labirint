using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Skins : ScriptableObject
{
    public List<Skin> skins = new List<Skin>();
}

[System.Serializable]
public class Skin {
    public string name;
    public int price;
    public Texture2D skin;

}
