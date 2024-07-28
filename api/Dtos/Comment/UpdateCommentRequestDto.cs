using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5,ErrorMessage ="Title must be 5 characters or more!")]
        [MaxLength(280,ErrorMessage ="Title must be 280 characters or less!")]
        public string Title { get; set; }=string.Empty;
        
        [Required]
        [MinLength(5,ErrorMessage ="Content must be 5 characters or more!")]
        [MaxLength(280,ErrorMessage ="Content must be 280 characters or less!")]
        public string Content {get; set;}=string.Empty;

    }
}