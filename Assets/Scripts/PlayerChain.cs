using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChain : MonoBehaviour
{
    public PlayerController[] players;
    
    PlayerController currentControlled;
    int currentControlledIdx;

    List<Vector3> positionHistory = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerIndex(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetPlayerIndex((currentControlledIdx + 1) % players.Length);
        }

        positionHistory.Add(currentControlled.transform.position);
        const int kHistoryLength = 30;
        if (positionHistory.Count > kHistoryLength)
        {
            positionHistory = positionHistory.GetRange(1, kHistoryLength);
        }

        for (int queueIdx = 1; queueIdx < players.Length; ++queueIdx)
        {
            int playerIdx = (currentControlledIdx + queueIdx) % players.Length;
            players[playerIdx].SetPosition(GetHistory(queueIdx * 15));
        }

        transform.position = currentControlled.transform.position;
    }

    Vector3 GetHistory(int framesAgo)
    {
        int idx = Mathf.Max(0, positionHistory.Count-framesAgo);
        return positionHistory[idx];
    }

    void SetPlayerIndex(int playerIndex)
    {
        for (int i = 0; i < players.Length; ++i)
        {
            int queueIndex = (i - playerIndex + players.Length) % players.Length;

            if (queueIndex == 0)
            {
                //When you swap, jump player to front
                if (currentControlled)
                    players[i].SetPosition(currentControlled.transform.position);
                ActivatePlayer(i);
            }
            else
                DeactivatePlayer(i);
            players[i].SetQueueOrder(queueIndex);
        }

    }

    void ActivatePlayer(int idx)
    {
        currentControlledIdx = idx;
        currentControlled = players[idx];
        players[idx].Activate();
    }

    void DeactivatePlayer(int idx)
    {
        players[idx].Deactivate();
    }
}
