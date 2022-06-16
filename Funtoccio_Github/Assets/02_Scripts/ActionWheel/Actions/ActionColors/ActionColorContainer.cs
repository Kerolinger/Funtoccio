using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DataContainer/ActionColors", fileName = "New Action Colors")]
public class ActionColorContainer : ScriptableObject
{
    [SerializeField] private Color _neutralColor;
    [SerializeField] private Color _selectedColor;

    public Color neutralColor 
    {
        get { return _neutralColor; }
    }

    public Color selectedColor
    {
        get { return _selectedColor; }
    }
}