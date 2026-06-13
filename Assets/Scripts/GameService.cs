using UnityEngine;

namespace Game
{
    public enum GameState
    {
        FreeRoam,
        Dialog,
        Battle
    }
    public class GameService : MonoBehaviour
    {
        [SerializeField] PlayerController playerController;

        GameState state;

        void Update()
        {
            if (state == GameState.FreeRoam)
            {
                playerController.HandleUpdate();
            }
            else if (state == GameState.Dialog)
            {
                // Handle dialog updates
            }
            else if (state == GameState.Battle)
            {
                // Handle battle updates
            }
        }
    }

}
