using GPSNote.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Services.LinkManager
{
    public class LinkManagerService : ILinkManagerService
    {
        private LinkModel _linkModel;
        private bool _isHave = false;

        public bool IsHave => _isHave;

        public void Clear()
        {
            _linkModel = null;
            _isHave = false;
        }

        public LinkModel GetLinkModel()
        {
            return _linkModel;
        }

        public void SetLinkModel(LinkModel link)
        {
            _isHave = true;
            _linkModel = link;
        }
    }
}
