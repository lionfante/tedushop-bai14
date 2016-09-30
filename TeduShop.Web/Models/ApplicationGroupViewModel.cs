using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeduShop.Web.Models
{
    public class ApplicationGroupViewModel
    {
        public int ID { set; get; }

        [MaxLength(250,ErrorMessage ="Tên không được quá {0} ký tự")]
        public string Name { set; get; }

        [MaxLength(250, ErrorMessage = "Mô tả không được quá {0} ký tự")]
        public string Description { set; get; }

        public IEnumerable<ApplicationRoleViewModel> Roles { set; get; }
    }
}