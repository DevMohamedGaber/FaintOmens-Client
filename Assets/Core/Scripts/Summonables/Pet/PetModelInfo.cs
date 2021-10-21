using UnityEngine;
using RTLTMPro;
namespace Game.Components
{
    public class PetModelInfo : MonoBehaviour
    {
        public RTLTextMeshPro3D ownerNameTxt;
        public RTLTextMeshPro3D nameTxt;
        public GameObject stunnedText;
        public RuntimeAnimatorController animatorController;
        public Avatar avatar;
        public Collider collider;
        public void Disappear()
        {
            Destroy(this);
        }
    }
}