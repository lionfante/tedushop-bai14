using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeduShop.Web.Models
{
    public class FeedbackViewModel
    {
        public int ID { set; get; }

        [MaxLength(250, ErrorMessage = "Tên không được quá {0} ký tự")]
        [Required(ErrorMessage = "Tên phải nhập")]
        public string Name { set; get; }

        [MaxLength(250, ErrorMessage ="Email không được quá {0} ký tự")]
        public string Email { set; get; }

        [MaxLength(500, ErrorMessage ="Nội dung không được quá {0} ký tự")]
        public string Message { set; get; }

        public DateTime CreatedDate { set; get; }

        [Required(ErrorMessage ="Chưa check trạng thái")]
        public bool Status { set; get; }

        public ContactDetailViewModel ContactDetailViewModel { set; get; }
    }
}