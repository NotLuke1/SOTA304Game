using UnityEngine;

public class sc_cameraFollow : MonoBehaviour
{
    [HeaderAttribute("Camera Stats")]
    public float cameraFOV;
    public float cameraAheadOffsetMultiplier;

    [SerializeField] private sc_movementController playerScript;


    private void Update()
    {

        //moveCameraOnPlayer();

        //moveCameraAheadOfPlayer();
        moveCameraBehindPlayer();
    }


    //Camera follow directly over the players position.
    private void moveCameraOnPlayer()
    {
        transform.position = playerScript.transform.position + new Vector3(0, 0, -10);
    }

    //Camera is ahead of the player's movement. Proportional to the player's speed.
    private void moveCameraAheadOfPlayer()
    {
        transform.position = playerScript.transform.position + (new Vector3 (playerScript.getMoveDirection().x, playerScript.getMoveDirection().y, 0) * cameraAheadOffsetMultiplier) + new Vector3(0, 0, -10);
    }

    //Camera lags behind the player's movement. Proportional to the player's speed.
    private void moveCameraBehindPlayer()
    {
        transform.position = playerScript.transform.position - (new Vector3(playerScript.getMoveDirection().x, playerScript.getMoveDirection().y, 0) * cameraAheadOffsetMultiplier) + new Vector3(0, 0, -10);
    }
}
