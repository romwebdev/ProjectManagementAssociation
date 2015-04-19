using Association.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Association.ViewsModel
{
    public class StudentIndexData
    {
        public int ParentID { get; set; }
        public string FullName { get; set; }
        public bool Assigned { get; set; }

    }
}