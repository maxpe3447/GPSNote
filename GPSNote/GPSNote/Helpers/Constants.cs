using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.Helpers
{
    public static class Constants
    {
        //Navigation to\from settings page
        public const string NAVIGATION_FROM_MAP_PAGE = nameof(NAVIGATION_FROM_MAP_PAGE);
        public const string NAVIGATION_FROM_PINLIST_PAGE = nameof(NAVIGATION_FROM_PINLIST_PAGE);

        public const string LINK_PROTOCOL_HTTP = "http";
        public const string LINK_PROTOCOL_HTTPS = "https";
        public const string LINK_DOMEN = "GpsNote.App";
        public const string LINK_GEO = "geo";
        public const string LINK_SEPARATOR = "/";
        public const byte   LINK_MAX_COUNT_SECTION = 6;
    }
}
