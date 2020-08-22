using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wallet : NetworkBehaviour
{
    public int held;

    private void Start()
    {
        OnChangeSum.Invoke(held);
    }

    public void AddMoney(int money)
    {
        held += money;
        OnChangeSum.Invoke(held);
        OnChangeSumFormat.Invoke($"$ {held}");
        if (isServer)
            RpcUpdateMoney(money);
    }
    [ClientRpc(channel = Channels.DefaultReliable)]
    public void RpcUpdateMoney(int money)
    {
        OnChangeSum.Invoke(held);
        OnChangeSumFormat.Invoke($"$ {held}");
    }

    public UnityEvent<int> OnChangeSum;
    public UnityEvent<string> OnChangeSumFormat;
}
