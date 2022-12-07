using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform[] panels;
    public enum GameState
    {
        GamePlay,
        Win,
        Fail,
    }
    
    public float laneSize = 10;
    public static GameManager Instance;
    public Player player;
    private void Awake()
    {
        Instance = this;
        player = FindObjectOfType<Player>();
        
    }

    public void ChangeGameStateUI(GameState gameState)
    {
        DisableAllUIs();
        switch (gameState)
        {
            case GameState.GamePlay:
                panels[0].gameObject.SetActive(true);
                break;
            case GameState.Win:
                panels[1].gameObject.SetActive(true);
                break;
            case GameState.Fail:
                panels[2].gameObject.SetActive(true);
                break;
        }
    }

    private void DisableAllUIs()
    {
        foreach (var panel in panels)
        {
            panel.gameObject.SetActive(false);
        }
    }
    
    public void LoadScene()
    {
        SceneManager.LoadScene(0);
        DOTween.KillAll();
    }
    
}
