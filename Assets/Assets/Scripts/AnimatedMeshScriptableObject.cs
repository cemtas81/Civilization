using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMeshScriptableObject : ScriptableObject
{
    public int AnimationFPS;
    public List<Animation> Animations = new List<Animation>();

    [Serializable]
    public struct Animation
    {
        public string Name;
        public List<Mesh> Meshes;
    }
}
