using System;
using UnityEngine;

namespace EthanZarov.SimpleTools
{
   [CreateAssetMenu(menuName = "Simple Tools/Frame Group")]
   public class AnimationFrameGroup : ScriptableObject
   {
       public Sprite[] frames;
       public float frameDuration;
       [Space] public LoopType loopType;
       public enum LoopType
       {
           Loop,
           PingPong,
           Clamp
       }

       private float _tempIndex;
       private int _intIndex;
      
       public Sprite GetFrameAtTime(float timeActive)
       {
           _tempIndex = timeActive / frameDuration;
           switch (loopType)
           {
               case LoopType.Loop:
                   return frames[Mathf.FloorToInt(_tempIndex % frames.Length)];
               case LoopType.PingPong:
                   _intIndex = Mathf.FloorToInt(_tempIndex % (frames.Length * 2));
                   if (_intIndex >= frames.Length) _intIndex = frames.Length - (_intIndex - frames.Length) - 1;
                   return frames[_intIndex];
               case LoopType.Clamp:
                   return frames[Mathf.Clamp(Mathf.FloorToInt(_tempIndex), 0, frames.Length-1)];
               default:
                   return frames[Mathf.FloorToInt(_tempIndex % frames.Length)];
           }
          
       }

       public Sprite GetFrameAt(int frameIndex)
       {
           return frames[frameIndex % frames.Length];
       }

       public int GetIndexAtTime(float timeActive)
       {
           _tempIndex = timeActive / frameDuration;
           switch (loopType)
           {
               case LoopType.Loop:
                   return Mathf.FloorToInt(_tempIndex % frames.Length);
               case LoopType.PingPong:
                   _intIndex = Mathf.FloorToInt(_tempIndex % (frames.Length * 2));
                   if (_intIndex >= frames.Length) _intIndex = frames.Length - (_intIndex - frames.Length) - 1;
                   return _intIndex;
               case LoopType.Clamp:
                   return Mathf.Clamp(Mathf.FloorToInt(_tempIndex), 0, frames.Length-1);
               default:
                   return Mathf.FloorToInt(_tempIndex % frames.Length);
           }
       }
      
      
       public float GetTIndexAtTime(float timeActive)
       {
           float overallDuration = frameDuration * frames.Length;
           //TODO: Add loopType switch statement
           return (timeActive % overallDuration) / overallDuration;
       }
      
   }
}
