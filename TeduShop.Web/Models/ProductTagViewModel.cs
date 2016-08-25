using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeduShop.Model.Models;

namespace TeduShop.Web.Models
{
    public class ProductTagViewModel
    {

        public int ProductID { set; get; }

        public string TagID { set; get; }

        public virtual Product Product { set; get; }

        public virtual Tag Tag { set; get; }
    }
}