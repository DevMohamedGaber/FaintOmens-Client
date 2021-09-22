using UnityEngine;
namespace Game.UI
{
    public class Workshop : WindowWithBasicCurrencies
    {
        [SerializeField] SubWindowBase[] pages;
        int currentPage = 0;
        public override void Refresh()
        {
            if(isVisible)
            {
                pages[currentPage].Refresh();
            }
        }
        public void GoToPage(int index)
        {
            for(int i = 0; i < pages.Length; i++)
            {
                if(i == index)
                {
                    pages[i].Show();
                }
                else
                {
                    pages[i].Hide();
                }
            }
            currentPage = index;
        }
        public void Plus()
        {
            GoToPage(0);
        }
        public void Socket()
        {
            GoToPage(1);
        }
        public void Quality()
        {
            GoToPage(2);
        }
        public void Craft()
        {
            GoToPage(3);
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            GoToPage(0);
        }
    }
}