using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;
    [Header("DataBase")]
    [SerializeField] private Skins skins;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject close;
    [Header("Stats")]
    [SerializeField] private int money;

    public static Shop Instance;

    Renderer render;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        render = player.GetComponent<Renderer>();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Space)) PlayerPrefs.DeleteAll();
    }

    public void Buy(int ID, int cost) {
        Skin[] Skin = skins.skins.ToArray();

        if (money >= cost && !PlayerPrefs.HasKey(Skin[ID].name)) {
            money -= cost;

            Texture2D texture = Skin[ID].skin;
            SetSkin(texture);

            PlayerPrefs.SetString(Skin[ID].name, Skin[ID].name);
            print("Buyed " + ID + "|" + cost);
        }
        else if (PlayerPrefs.HasKey(Skin[ID].name)) SetSkin(Skin[ID].skin);
        else return;
    }

    public void SetSkin(Texture2D texture) {
        render.material.mainTexture = texture;
        print("SET SKIN");
    }

    public void OpenShop() {
        shop.SetActive(true);
        close.SetActive(true);
    }
    public void CloseShop() {
        shop.SetActive(false);
        close.SetActive(false);
    }
}
