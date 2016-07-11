using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Classes
{
    abstract class DropMenuBase
    {
        
        protected bool _dropMasked;
        protected bool _masked;
        public event EventHandler Dropped;

        public void SubscribeDrop(DropMenuBase other)
        {
            other.Dropped += new EventHandler(OnDrop);
        }

        protected void OnDrop(object sender, EventArgs args)
        {
            _dropMasked = true;
        }

        public void Unmask()
        {
            _dropMasked = false;
            _masked = true;
        }

        protected void Drop()
        {
            Dropped(this, EventArgs.Empty);
        }
    }
}
