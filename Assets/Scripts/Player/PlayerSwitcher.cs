using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    void Update()
    {
        // Handle number keys 1-9
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                int playerIndex = i;
                if (playerIndex < GlobalVariables.Instance.players.Count)
                {
                    GlobalVariables.Instance.SwitchToPlayer(playerIndex);
                }
            }
        }
    }
}