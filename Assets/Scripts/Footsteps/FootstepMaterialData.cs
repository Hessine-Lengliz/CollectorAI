using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FootstepMaterialData")]
public class FootstepMaterialData : ScriptableObject
{
    public string materialName;
    public List<AudioClip> footstepSounds;
}