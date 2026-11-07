using UnityEngine;
using UnityEngine.InputSystem;

public class sc_actionController : MonoBehaviour
{


    [HeaderAttribute("Linked Objects")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private GameObject photoViewObject;
    [SerializeField] private GameObject flashlightViewObject;





    public void OnTakePhoto(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Photo");
            flashlightViewObject.GetComponent<MeshRenderer>().enabled = false;
            photoViewObject.GetComponent<MeshRenderer>().enabled = true;
            GetComponent<sc_movementController>().setCurrentView(photoViewObject.GetComponent<sc_fieldOfView>());
        }

        
    }
}
