using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointRotTranslater : MonoBehaviour
{
    public PuppetAnimationController m_animController;
    public ConfigurableJoint configJoint;
    public Transform kreuzTransform;
    public float multiplier;

    void Update()
    {
        if (m_animController.isFacingRight)
        {
            multiplier = 1;

        }
        else
        {
            multiplier = -1;


        }

        configJoint.targetRotation = Quaternion.Euler(-multiplier * kreuzTransform.localRotation.eulerAngles.z, -kreuzTransform.localRotation.eulerAngles.y,kreuzTransform.localRotation.eulerAngles.x * multiplier);
    }
}
