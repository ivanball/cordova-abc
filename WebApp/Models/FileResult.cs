﻿using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class FileResult
    {
        public IEnumerable<string> Names { get; set; }
        public IEnumerable<string> FileNames { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime UpdatedTimestamp { get; set; }
        public string DownloadLink { get; set; }
        public IEnumerable<string> ContentTypes { get; set; }
    }
}
