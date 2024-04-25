using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isGameOver;
    public bool isPause;
    public bool isSwitchCamera;

    [SerializeField] private TextMeshProUGUI playerCoinText;
    [SerializeField] private TextMeshProUGUI playerCoinGameOverText;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Image increaseFlash;
    [SerializeField] private Image decreaseFlash;

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
        listMaps[SaveLoadData.Instance.levelIndex].SetActive(true);
        Vector3 spawn = GameObject.Find("SpawnPosition").transform.position;
        
        vehicle = Instantiate(listVehicles[SaveLoadData.Instance.vehicleID]);
        vehicle.position = spawn;

        isSwitchCamera = true;
        box = GameObject.Find("Box").transform;
        cinemachineVirtualCamera[0].Follow = vehicle;
        cinemachineVirtualCamera[1].Follow = box;
        SwitchCamera(isSwitchCamera);

        SetCoinText();
        SoundManager.Instance.vehicleAudioSource.Play();
    }

    public void BackToHome()
    {
        isGameOver = true;
        if(box) box.GetComponent<Box>().KillAnimation();
        SaveLoadData.Instance.levelIndex = 0;
        SceneManager.LoadScene("TopScene");
    }

    public void OpenStore()
    {
        isGameOver = true;
        if(box) box.GetComponent<Box>().KillAnimation();
        SceneManager.LoadScene("SelectScene");
    }

    public void SetCoinText()
    {
        playerCoinText.text = SaveLoadData.Instance.playerInfo.coins.ToString();
        playerCoinGameOverText.text = SaveLoadData.Instance.playerInfo.coins.ToString();
    }

    public void OnGameOver(bool completed)
    {
        isGameOver = true;
        if(completed)
        {
            if(SaveLoadData.Instance.levelIndex < listMaps.Length - 1) 
            {
                StartCoroutine(Effect());
            }
            else
            {
                gameOverPanel.SetActive(true);
            }
        }
        else
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        isGameOver = true;
        if(box) box.GetComponent<Box>().KillAnimation();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator Effect()
    {
        SaveLoadData.Instance.levelIndex ++;
        yield return new WaitForSeconds(1.5f);
        RestartGame();
    }

    public async void AddForce(GameObject gameObject, bool increase)
    {
        Destroy(gameObject);
        if(increase)
        {
            await Fade(increaseFlash);
        }
        else
        {
            await Fade(decreaseFlash);
        }
    }

    private async UniTask Fade(Image image)
    {
        image.DOFade(1, 0);
        image.gameObject.SetActive(true);
        image.DOFade(0, 1);

        await UniTask.Delay(TimeSpan.FromSeconds(1));
        image.gameObject.SetActive(false);
    }

    public void SwitchCamera(bool on)
    {
        cinemachineVirtualCamera[0].gameObject.SetActive(on);
        cinemachineVirtualCamera[1].gameObject.SetActive(!on);
    }
}
