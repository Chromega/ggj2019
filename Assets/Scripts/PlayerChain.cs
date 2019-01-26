using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChain : MonoBehaviour
{
    public List<PlayerController> players;
    
    PlayerController currentControlled;
    int currentControlledIdx;

    List<Vector3> positionHistory = new List<Vector3>();

    public static PlayerChain Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

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
            SetPlayerIndex((currentControlledIdx + 1) % players.Count);
        }

        positionHistory.Add(currentControlled.transform.position);
        const int kHistoryLength = 60;
        if (positionHistory.Count > kHistoryLength)
        {
            positionHistory = positionHistory.GetRange(1, kHistoryLength);
        }

        for (int queueIdx = 1; queueIdx < players.Count; ++queueIdx)
        {
            int playerIdx = (currentControlledIdx + queueIdx) % players.Count;
            players[playerIdx].SetPosition(GetHistory(queueIdx * 30), currentControlled.spriteRenderer.flipX?-1:1);
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
        for (int i = 0; i < players.Count; ++i)
        {
            int queueIndex = (i - playerIndex + players.Count) % players.Count;

            if (queueIndex == 0)
            {
                //When you swap, jump player to front
                if (currentControlled)
                {
                    players[i].CopyFromOther(currentControlled);
                }
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

    public void AddToChain(PlayerController controller)
    {
        players.Add(controller);
        controller.SetQueueOrder(players.Count - 1);
        controller.Deactivate();
    }
}
