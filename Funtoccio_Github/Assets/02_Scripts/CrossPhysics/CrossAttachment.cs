using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossAttachment : MonoBehaviour
{
    public float lineLength;
    public Rigidbody spineRigidBody;
    public GameObject spineTransform;

     float dragForce = 3.099f;

    
    //sets strings of puppet at right place because rigidbody has some physics that apply gravity on it, which we "delete" by adding the dragforce (when puppet is in right position) -
    //if the strings are too short, let him fall down!
    void Update()
    {
        //if puppet strings are at right place, make sure they stay there!
        if((spineTransform.transform.position.y < gameObject.transform.position.y - lineLength + 0.005f) && spineTransform.transform.position.y > gameObject.transform.position.y - lineLength - 0.005f)
        {
            spineRigidBody.velocity = new Vector3(0, dragForce, 0);
        }
        //if puppet strings are way too long, put him on right position
         if (spineTransform.transform.position.y < gameObject.transform.position.y - lineLength)
         {
            //set new position for puppet 
            spineTransform.transform.position = new Vector3(spineTransform.transform.position.x, gameObject.transform.position.y - lineLength, spineTransform.transform.position.z);
            spineRigidBody.velocity = new Vector3(0, dragForce, 0);
         }

    }
}
