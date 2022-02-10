using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;

namespace GPSNote.Helpers
{
    public class TextResources
    {
        readonly ResourceManager ResourceManager;
        public TextResources(Type resource)
        {
            ResourceManager = new ResourceManager(resource);
        }

        public string this[string key]
        {
            get
            {
                return ResourceManager.GetString(key);
            }
        }


       
    }
}
