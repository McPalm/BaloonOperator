﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : NetworkBehaviour
{
    public string[] stages;
    string NextStage => stages[StageDifficulty % stages.Length];

    public SceneLoader SceneLoader { get; set; }
    public GameObject Player { get; set; }
    public List<GameObject> AllPlayers { get; set; }

    float resetCooldown = 0f;

    static public int StageDifficulty = 0;


    public UnityEvent OnWinEvent;
    public UnityEvent OnGameOverEvent;
    public UnityEvent OnPlayStateEvent;
    public UnityEvent OnResetEvent;

    public WeaponProperties defaultWeapon;

    private void Awake()
    {
        AllPlayers = new List<GameObject>();
    }

    bool lose = false;
    bool win = false;

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            SceneLoader = GetComponent<SceneLoader>();
            SceneLoader.LoadScene(NextStage);
            StartCoroutine(StateMachine());
        }
        OnResetEvent.AddListener(() =>
        {
            var inventory = Player.GetComponent<Inventory>();
            inventory.Clear();
        });
    }

    bool IsGameOver()
    {
        if (AllPlayers.Count > 0)
        {
            foreach (GameObject go in AllPlayers)
            {
                if (go.GetComponent<Health>().CurrentHealth > 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    IEnumerator StateMachine()
    {
        yield return new WaitForSeconds(1f);
    Play:
        RpcOnPlayState();
        for (; ; )
        {
            yield return null;
            if (IsGameOver())
                goto GameOver;
            if (win)
                goto Win;
            if (lose)
                goto GameOver;
        }
    GameOver:
        RpcOnGameOver();
        StageDifficulty = 0;
        yield return new WaitForSeconds(3f);
        lose = false;
        win = false;
        ResetGame();
        
        FindObjectOfType<Wallet>().ResetWallet();
        goto Play;
    Win:
        StageDifficulty++;
        RpcOnWin(StageDifficulty);
        yield return new WaitForSeconds(1f);
        SceneLoader.LoadScene(NextStage);
        win = false;
        lose = false;
        HealAllPlayersOne();
        goto Play;
    }

    [Server]
    public void TriggerLoss()
    {
        lose = true;
    }

    [Server]
    public void TriggerWin()
    {
        if (isServer)
        {
            win = true;
        }
    }

    [Server]
    public void ResetGame()
    {
        if (resetCooldown > Time.time)
            throw new System.Exception("Your reloading the stage to quickly");
        resetCooldown = Time.time + 1f;
        SceneLoader.LoadScene(NextStage);
        foreach (GameObject go in AllPlayers)
        {
            go.GetComponent<Health>().Heal(new DamageData()
            {
                damage = 999,
                source = gameObject,
                authorative = true,
            });
        }

        RpcOnReset();
    }

    public void RegisterPlayer(GameObject player)
    {
        Player = player;
    }


    public void RegisterAllPlayers(GameObject player)
    {
        AllPlayers.Add(player);
    }
    public void UnRegisterPlayer(GameObject player) => AllPlayers.Remove(player);

    void HealAllPlayersOne()
    {
        foreach (var player in AllPlayers)
        {
            var health = player.GetComponent<Health>();
            health.Heal(new DamageData()
            {
                damage = 1 - Mathf.Min(health.CurrentHealth, 0),
                authorative = true,
            });
        }
    }

    [ClientRpc]
    void RpcOnWin(int stage)
    {
        StageDifficulty = stage;
        OnWinEvent.Invoke();
    }
    [ClientRpc] void RpcOnGameOver() => OnGameOverEvent.Invoke();
    [ClientRpc] void RpcOnPlayState() => OnPlayStateEvent.Invoke();
    [ClientRpc] void RpcOnReset() => OnResetEvent.Invoke();
    
}
