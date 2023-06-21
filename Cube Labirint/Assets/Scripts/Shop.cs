using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;
    [Header("DataBase")]
    [SerializeField] private Skins skins;
    [Header("Stats")]
    [SerializeField] private int money;

    Renderer render;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        render = player.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) PlayerPrefs.DeleteAll();
    }

    public void Buy(string nameID) {
        string[] split = nameID.Split(","[0]);
        int ID = int.Parse(split[0]);
        int cost = int.Parse(split[1]);

        Skin[] Skin = skins.skins.ToArray();

        if (money >= cost && !PlayerPrefs.HasKey(Skin[ID].name)) {
            money -= cost;

            Texture2D texture = Skin[ID].skin;
            SetSkin(texture);

            PlayerPrefs.SetString(Skin[ID].name, Skin[ID].name);
        }
        else if (PlayerPrefs.HasKey(Skin[ID].name)) SetSkin(Skin[ID].skin);
        else return;
    }

    public void SetSkin(Texture2D texture) {
        render.material.mainTexture = texture;
    }
}
