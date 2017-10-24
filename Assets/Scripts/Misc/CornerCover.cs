using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerCover : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Squadie")
        {
            Debug.Log("Squadie in Corner Cover");
            other.gameObject.GetComponent<AIController>().InCornerCover(true);
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Squadie")
        {
            Debug.Log("Squadie leaving Corner Cover");
            other.gameObject.GetComponent<AIController>().InCornerCover(false);
        }
    }
}
