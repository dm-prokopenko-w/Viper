using Core;
using UnityEngine;

namespace PlayerSystem
{
    public class PlayerHead : Character
    {
        private int _rotCount = 0;

        public void ResetRot()
        {
            _rect.rotation = Quaternion.identity;
        }
        
        public void ResetRotCount() =>  _rotCount = 0;

        public bool RotateRight()
        {
            if(_rotCount < 0) return false;
            _rotCount--;

            _rect.Rotate(0,0,-90);
            return true;
        }
        
        public bool RotateLeft()
        {
            if(_rotCount > 0) return false;
            _rotCount++; 
            
            _rect.Rotate(0,0,90);
            return true;
        }
    }
}
