using GPSNote.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Services.LinkManager
{
    public class LinkManager : ILinkManager
    {
        private LinkModel _linkModel;
        private bool _isHave = false;

        public bool IsHave => _isHave;

        public LinkModel GetLinkModel()
        {
            _isHave = true;
            return _linkModel;
        }

        public void SetLinkModel(LinkModel link)
        {
            _linkModel = link;
        }
    }
}
