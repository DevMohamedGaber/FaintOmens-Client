using UnityEngine;
using UnityEngine.UI;
namespace Game.UI
{
    public class UIInteractiveButton : UIBasicButton
    {
        [SerializeField] Image image;
        [SerializeField] Sprite inactiveSprite;
        [SerializeField] Sprite activeSprite;
        [SerializeField] bool _active;
        public bool active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
                UpdateGraphic();
            }
        }
        public void SetActive(bool value)
        {
            _active = value;
            if(image != null)
            {
                UpdateImage();
            }
        }
        void UpdateGraphic()
        {
            // Update siblings
            if(transform.parent.childCount > 1)
            {
                UIInteractiveButton[] children = transform.parent.GetComponentsInChildren<UIInteractiveButton>();
                if(children.Length > 1)
                {
                    for(int i = 0; i < children.Length; i++)
                    {
                        if(children[i] != this)
                        {
                            children[i].SetActive(false);
                        }
                    }
                }
                else
                {
                    children[0].SetActive(false);
                }
            }
            // Update this button
            //if(image != null)
            //    UpdateImage();
        }
        void UpdateImage()
        {
            if(_active)
            {
                if(activeSprite != null)
                {
                    image.sprite = activeSprite;
                    if(!image.gameObject.activeSelf)
                    {
                        image.gameObject.SetActive(true);
                    }
                }
                else
                {
                    image.gameObject.SetActive(false);
                }
            }
            else
            {
                if(inactiveSprite != null)
                {
                    image.sprite = inactiveSprite;
                    if(!image.gameObject.activeSelf)
                    {
                        image.gameObject.SetActive(true);
                    }
                }
                else
                {
                    image.gameObject.SetActive(false);
                }
            }
        }
        public override void OnInvokeAction()
        {
            base.OnInvokeAction();
            active = true;
        }
    }
}