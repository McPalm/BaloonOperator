﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(PlayerInput), typeof(PlatformingCharacter))]
public class NetworkControls : NetworkBehaviour
{
    const float MIN_REFRESH_DELAY = .03f;

    public Interpolation interpolation;

    PlayerInput PlayerInput { get; set; }
    PlatformingCharacter PlatformingCharacter { get; set; }

    InputToken InputToken { get; set; }
    InputToken OutputToken { get; set; }

    bool holdJump = false;
    float moveX = 0f;
    // private float nextForcedUpdate = 0f;
    double lastUpdate;
    bool jumped = false;
    bool attacked = false;

    double lastBoost;

    // Start is called before the first frame update
    void Start()
    {
        GameManager game = FindObjectOfType<GameManager>();

        PlayerInput = GetComponent<PlayerInput>();
        PlatformingCharacter = GetComponent<PlatformingCharacter>();

        OutputToken = new InputToken();
        if (isLocalPlayer)
        {
            InputToken = PlayerInput.InputToken;
            PlatformingCharacter.OnJump += () => jumped = true;
            FindObjectOfType<CameraFollow>().Follow = new Mobile[] { PlatformingCharacter };
            PlatformingCharacter.OnStomp += PlatformingCharacter_OnStomp;
            var attack = GetComponent<Attack>();
            if (attack)
                attack.OnAttack += () => attacked = true;

            game.RegisterPlayer(gameObject);
        }
        else
        {
            foreach (var inputReaders in GetComponents<IInputReader>())
                inputReaders.InputToken = OutputToken;
            PlayerInput.enabled = false;
            interpolation.enabled = true;
        }

        if (isServer)
        {
            game.RegisterAllPlayers(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(isServer)
        {
            FindObjectOfType<GameManager>().UnRegisterPlayer(gameObject);
        }
    }

    private void PlatformingCharacter_OnStomp(PlatformingCharacter obj)
    {
        lastBoost = NetworkTime.time;
        obj.GetComponent<NetworkControls>().CmdStomp(NetworkTime.time, obj.transform.position);
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            //if (InputToken.Direction.x != moveX || holdJump != InputToken.JumpHeld || nextForcedUpdate < Time.realtimeSinceStartup)
            SendSyncInput();
        }
        
    }

    void SendSyncInput()
    {
        moveX = InputToken.Direction.x;
        holdJump = InputToken.JumpHeld;
        CmdSyncInput(moveX, holdJump, transform.position, new Vector2(PlatformingCharacter.HMomentum, PlatformingCharacter.VMomentum), NetworkTime.time, jumped, attacked);
        jumped = false;
        attacked = false;
        // nextForcedUpdate = Time.realtimeSinceStartup + MIN_REFRESH_DELAY;
    }


    [Command(channel = Channels.DefaultUnreliable)] private void CmdSyncInput(float moveX, bool holdJump, Vector2 pos, Vector2 force, double time, bool jump, bool attack) => RpcSyncInput(moveX, holdJump, pos, force, time, jump, attack);
    [ClientRpc(channel = Channels.DefaultUnreliable)] private void RpcSyncInput(float moveX, bool holdJump, Vector2 pos, Vector2 force, double time, bool jump, bool attack)
    {
        if (isLocalPlayer)
            return;
        if (lastUpdate > time)
            return;
        lastUpdate = time;
        OutputToken.JumpHeld = holdJump;
        OutputToken.Direction = new Vector2(moveX, 0f);
        //if (((Vector2)transform.position - pos).sqrMagnitude > .5f)
            transform.position = pos;
        PlatformingCharacter.HMomentum = force.x;
        PlatformingCharacter.VMomentum = force.y;
        if(jump)
            OutputToken.PressJump();
        if (attack)
            OutputToken.PressUse();
    }

    [Command(channel = 2, ignoreAuthority = true)] private void CmdStomp(double time, Vector2 at) => RpcStomp(time, at);
    [ClientRpc(channel = 2)] private void RpcStomp(double time, Vector2 at)
    {
        if(time > lastBoost)
        {
            PlatformingCharacter.Resimulate((float)(NetworkTime.time - time), at, stomp: true);
        }
    }
}
