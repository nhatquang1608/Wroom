using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OperationButton : MonoBehaviour
{
    public enum ButtonType
    {
        Purchase,
        SetUse,
        Used
    }
    public ButtonType buttonType;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject Used;
    [SerializeField] private GameObject SetUse;
    [SerializeField] private SelectSceneManager selectSceneManager;

    private void Awake()
    {
        button.onClick.AddListener(Operation);
    }

    private void Operation()
    {
        switch(buttonType)
        {
            case ButtonType.Purchase:
                if(SaveLoadData.Instance.playerInfo.coins >= SaveLoadData.Instance.playerInfo.listVehicles[selectSceneManager.vehicleID].price)
                {
                    foreach(Vehicle vehicle in SaveLoadData.Instance.playerInfo.listVehicles)
                    {
                        if(vehicle.id != selectSceneManager.vehicleID && vehicle.owned)
                        {
                            vehicle.used = false;
                        }
                        else if(vehicle.id == selectSceneManager.vehicleID && !vehicle.owned)
                        {
                            vehicle.used = true;
                            vehicle.owned = true;
                            SetUp(vehicle.price, vehicle.owned, vehicle.used);
                            SaveLoadData.Instance.playerInfo.coins -= vehicle.price;
                            SaveLoadData.Instance.vehicleID = vehicle.id;
                        }
                    }
                    SaveLoadData.Instance.SaveData();
                    selectSceneManager.SetCoinText();
                    SoundManager.Instance.PlaySound(SoundManager.Instance.purchaseSound);
                }
                break;
            case ButtonType.Used:
                break;
            case ButtonType.SetUse:
                foreach(Vehicle vehicle in SaveLoadData.Instance.playerInfo.listVehicles)
                {
                    if(vehicle.id != selectSceneManager.vehicleID && vehicle.owned)
                    {
                        vehicle.used = false;
                    }
                    else if(vehicle.id == selectSceneManager.vehicleID && vehicle.owned)
                    {
                        vehicle.used = true;
                        SetUp(vehicle.price, vehicle.owned, vehicle.used);
                        SaveLoadData.Instance.vehicleID = vehicle.id;
                    }
                }
                SaveLoadData.Instance.SaveData();
                selectSceneManager.SetCoinText();
                SoundManager.Instance.PlaySound(SoundManager.Instance.usedSound);
                break;
        }
    }

    public void SetUp(int price, bool owned, bool used)
    {
        priceText.text = price.ToString();
        SetUse.SetActive(owned);
        Used.SetActive(used);

        if(owned && used)
        {
            buttonType = ButtonType.Used;
        }
        else if(owned && !used)
        {
            buttonType = ButtonType.SetUse;
        }
        else
        {
            buttonType = ButtonType.Purchase;
        }
    }
}
