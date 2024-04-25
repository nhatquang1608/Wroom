using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject guidePanel;

    private void Start()
    {
        foreach(Vehicle vehicle in SaveLoadData.Instance.playerInfo.listVehicles)
        {
            if(vehicle.used)
            {
                SaveLoadData.Instance.vehicleID = vehicle.id;
            }
        }

        guidePanel.SetActive(false);
        SoundManager.Instance.vehicleAudioSource.Stop();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenStore()
    {
        SceneManager.LoadScene("SelectScene");
    }
}
