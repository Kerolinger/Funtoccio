using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PuppetPlay/Action", fileName = "New Action")]
public class Action : ScriptableObject
{
    public string actionName;
    public GameEvent actionEvent;
}
