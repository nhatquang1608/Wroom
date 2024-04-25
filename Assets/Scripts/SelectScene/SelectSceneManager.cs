using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectSceneManager : MonoBehaviour
{
    public int vehicleID;
    [SerializeField] private TextMeshProUGUI playerCoin;
    [SerializeField] private ScrollRect vehicleScroll;
    [SerializeField] private OperationButton operationButton;

    private float[] nails = {0, 1/5f, 2/5f, 3/5f, 4/5f, 1};

    private void Awake()
    {
        SetCoinText();
        foreach(Vehicle vehicle in SaveLoadData.Instance.playerInfo.listVehicles)
        {
            if(vehicle.used)
            {
                vehicleID = vehicle.id;
                vehicleScroll.horizontalNormalizedPosition = nails[vehicleID];
                SetOperationButton();
            }
        }
        SoundManager.Instance.vehicleAudioSource.Stop();
    }

    public void SetCoinText()
    {
        playerCoin.text = SaveLoadData.Instance.playerInfo.coins.ToString();
    }

    public void OnPrevious()
    {
        vehicleID--;
        if(vehicleID == -1) vehicleID = 5;
        vehicleScroll.horizontalNormalizedPosition = nails[vehicleID];
        SetOperationButton();
    }

    public void OnNext()
    {
        vehicleID++;
        if(vehicleID == 6) vehicleID = 0;
        vehicleScroll.horizontalNormalizedPosition = nails[vehicleID];
        SetOperationButton();
    }

    private void SetOperationButton()
    {
        foreach(Vehicle vehicle in SaveLoadData.Instance.playerInfo.listVehicles)
        {
            if(vehicle.id == vehicleID)
            {
                operationButton.SetUp(vehicle.price, vehicle.owned, vehicle.used);
            }
        }
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("TopScene");
    }
}
