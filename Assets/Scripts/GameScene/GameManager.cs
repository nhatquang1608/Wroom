using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameOver;
    public bool isPause;
    public bool isSwitchCamera;
    [SerializeField] private int levelIndex;
    [SerializeField] private int vehicleId;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;


    [SerializeField] private Transform box;
    [SerializeField] private Transform vehicle;
    [SerializeField] private CinemachineVirtualCamera[] cinemachineVirtualCamera;
    [SerializeField] private Transform[] listVehicles;
    [SerializeField] private GameObject[] listMaps;

    private void Start()
    {
        NewGame();
    }

    public void PauseGame()
    {
        if(!isGameOver)
        {
            pausePanel.SetActive(true);
        }
    }

    private void NewGame()
    {
        foreach(GameObject map in listMaps)
        {
            map.SetActive(false);
        }
        listMaps[levelIndex].SetActive(true);
        Vector3 spawn = GameObject.Find("SpawnPosition").transform.position;
        
        vehicle = Instantiate(listVehicles[vehicleId]);
        vehicle.position = spawn;

        isSwitchCamera = true;
        box = GameObject.Find("Box").transform;
        cinemachineVirtualCamera[0].Follow = vehicle;
        cinemachineVirtualCamera[1].Follow = box;
        SwitchCamera(isSwitchCamera);
    }

    public void OnGameOver(bool completed)
    {
        isGameOver = true;
        if(completed)
        {
            if(levelIndex < listMaps.Length - 1) 
            {
                StartCoroutine(Effect());
            }
        }
        else
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator Effect()
    {
        levelIndex++;
        yield return new WaitForSeconds(2);

        Destroy(vehicle.gameObject);
        RestartGame();
    }

    public void SwitchCamera(bool on)
    {
        cinemachineVirtualCamera[0].gameObject.SetActive(on);
        cinemachineVirtualCamera[1].gameObject.SetActive(!on);
    }
}
