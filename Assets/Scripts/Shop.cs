using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    public GameObject bar;

    public ShopItem[] allShopItems;
    public int currentCar = 0;
    public Transform scrollPanel;


    public GameObject[] allShopObjects;

    int currentItem = 0;
    float initialPos = 0;
    float rotationsPerMinute = 20.0f;

    public Image playImage;
    public Image shopImage;

    public Canvas shopCanvas;
    public Canvas itemsCanvas;
    public Canvas mainMenuCanvas;
    public Canvas gameOverCanvas;

    public GameObject[] allCarPrefabs;
    public Text descriptionText;
    public Text nameText;

    //Purchaser purchaser;
    void Start()
    {
        //purchaser = transform.GetComponent<Purchaser>();
        initialPos = scrollPanel.localPosition.x;
    }
    void Update()
    {
        currentItem = (int)(initialPos - scrollPanel.localPosition.x) / 300;
        currentItem = Mathf.Clamp(currentItem, 0, 6);
        allShopObjects[currentItem].transform.Rotate(0, 6.0f * rotationsPerMinute * Time.deltaTime, 0);
        UpdateLabel();
    }

    void UpdateLabel()
    {
        if (currentItem == 0)
            allShopItems[currentItem].isBought = true;
        else
            allShopItems[currentItem].isBought = PlayerPrefs.GetInt("IsBought" + currentItem.ToString(), 0) == 1 ? true : false;

        nameText.text = allShopItems[currentItem].name;
        if (Manager.Instance.highScore > allShopItems[currentItem].unlockScore)
        {
            playImage.gameObject.SetActive(true);
            shopImage.gameObject.SetActive(false);
            descriptionText.text = "";
        }
        else if (allShopItems[currentItem].isBought)
        {
            playImage.gameObject.SetActive(true);
            shopImage.gameObject.SetActive(false);
            descriptionText.text = "";
        }
        else
        {
            playImage.gameObject.SetActive(false);
            shopImage.gameObject.SetActive(true);
            descriptionText.text = allShopItems[currentItem].description;
        }
    }

    public void Buy()
    {
        //purchaser.BuyNonConsumable(currentItem);
    }

    public void Select()
    {
        Manager.Instance.currentCar = currentItem;
        PlayerPrefs.SetInt("currentCar", currentItem);

        if (Manager.Instance.currentGameState == Manager.GameStates.MainMenu)
        {
            Destroy(GameObject.Find("Car"));
            GameObject go = Instantiate(allCarPrefabs[currentItem]);
            go.name = "Car";
        }
        

        CloseShop();
    }
    
    public void CloseShop()
    {
        if (Manager.Instance.currentGameState == Manager.GameStates.GameOver)
        {
            gameOverCanvas.gameObject.SetActive(true);
        }
        else
        {
            mainMenuCanvas.gameObject.SetActive(true);
        }
        Manager.Instance.inShop = false;
        shopCanvas.gameObject.SetActive(false);
        itemsCanvas.gameObject.SetActive(false);

        
    }

    public void ShowShop()
    {
        Transform allObstacles = GameObject.Find("AllObstacles").transform;

        foreach(Transform child in allObstacles)
        {
            Destroy(child.gameObject);
        }
        if (gameOverCanvas.gameObject.activeSelf)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
        Manager.Instance.inShop = true;
        shopCanvas.gameObject.SetActive(true);
        itemsCanvas.gameObject.SetActive(true);

        mainMenuCanvas.gameObject.SetActive(false);
    }

    public void IsShopButtonDown()
    {
        Manager.Instance.isMainMenuButtonPressed = true;
    }

    public void IsShopButtonup()
    {
        Manager.Instance.isMainMenuButtonPressed = false;
    }
}
