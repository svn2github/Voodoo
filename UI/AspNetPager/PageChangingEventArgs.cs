using System.ComponentModel;

namespace Voodoo.UI
{
    /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Class[@name="PageChangingEventArgs"]/*'/>
    public sealed class PageChangingEventArgs : CancelEventArgs
    {
        private readonly int _newpageindex;

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Constructor[@name="PageChangingEventArgs"]/*'/>
        public PageChangingEventArgs(int newPageIndex)
        {
            _newpageindex = newPageIndex;
        }

        /// <include file='AspNetPagerDocs.xml' path='AspNetPagerDoc/Property[@name="NewPageIndex"]/*'/>
        public int NewPageIndex
        {
            get { return _newpageindex; }
        }
    }
}
