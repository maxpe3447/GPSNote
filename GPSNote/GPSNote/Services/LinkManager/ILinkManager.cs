using GPSNote.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Services.LinkManager
{
    public interface ILinkManagerService
    {
        LinkModel GetLinkModel();
        void SetLinkModel(LinkModel link);

        void Clear();

        bool IsHave { get; }
    }
}
