using UnityEngine;

public class sc_moveInFront : MonoBehaviour
{
    public Transform parentTransform;

    private float initialZPos;

    private void Start()
    {
        initialZPos = parentTransform.position.z;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            parentTransform.Translate(0, 0, collision.transform.position.z - 1);

            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            parentTransform.position = new Vector3(parentTransform.position.x, parentTransform.position.y, initialZPos);


        }
    }
}
