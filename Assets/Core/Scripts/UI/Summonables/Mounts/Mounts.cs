using UnityEngine;
using TMPro;
using System.Collections.Generic;
namespace Game.UI
{
    public class Mounts : WindowWithBasicCurrencies
    {
        [Header("Mounts")]
        [SerializeField] MountPage[] pages;
        [SerializeField] MountsList mounts;
        int activePage = 0;
        public void Status()
        {
            GoToPage(0);
        }
        public void Upgrade()
        {
            GoToPage(1);
        }
        public void StarUp()
        {
            GoToPage(2);
        }
        public void Train()
        {
            GoToPage(3);
        }
        void GoToPage(int page)
        {
            for(int i = 0; i < pages.Length; i++)
            {
                pages[i].gameObject.SetActive(i == page);
            }
            activePage = page;
            pages[page].Refresh();
        }
        public void Show(ushort mountId)
        {
            gameObject.SetActive(true);
            mounts.SelectId(mountId);
        }
        public void OnMountUpdated(Mount mount)
        {
            mounts.UpdateMount(mount);
            pages[activePage].Refresh();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            Status();
        }
    }
}