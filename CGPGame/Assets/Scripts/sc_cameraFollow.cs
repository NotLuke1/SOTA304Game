using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class sc_cameraFollow : MonoBehaviour
{
    [HeaderAttribute("Camera Stats")]
    public float cameraFOV;
    public float cameraAheadOffsetMultiplier;

    [SerializeField] private float mouseOffsetMultiplier;

    public enum cameraFollowType
    {
        OnPlayer,
        AheadPlayer,
        BehindPlayer,
        TowardsMouse
    }
    public cameraFollowType cameraFollowMode;

    [HeaderAttribute("Linked Objects")]
    [SerializeField] private sc_movementController playerScript;

 

    private void Update()
    {

        
        //Updates camera follow logic based on selected mode, will be removed when its finalised
        switch (cameraFollowMode)
        {
            case cameraFollowType.OnPlayer:
                moveCameraOnPlayer(); break;
            case cameraFollowType.AheadPlayer:
                moveCameraAheadOfPlayer(); break;
            case cameraFollowType.BehindPlayer:
                moveCameraBehindPlayer(); break;
            case cameraFollowType.TowardsMouse:
                moveCameraTowardsMouse(); break;

        }

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


    //Camera is shifted towards the mouse position, offset is always proportional to screen size (i think)
    private void moveCameraTowardsMouse()
    {

        Vector2 mouseOffset = playerScript.getMousePos() - new Vector2 (Display.main.renderingWidth/2, Display.main.renderingHeight/2);
        Vector2 proportionalOffset = new Vector2(mouseOffset.x / (Display.main.renderingWidth/2), mouseOffset.y / (Display.main.renderingHeight/2));
        transform.position = playerScript.transform.position + new Vector3(proportionalOffset.x * mouseOffsetMultiplier, proportionalOffset.y * mouseOffsetMultiplier, -10);
    }
}
