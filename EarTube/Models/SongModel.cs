using EarTube.Areas.Identity.Data;
using EarTube.MyValidator;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.Models
{
    public class SongModel
    {
        public int Id { get; set; }
        [StringLength(100, MinimumLength = 5)]
        [Required(ErrorMessage = "Please enter the title of your song")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter the artist name")]
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string Like { get; set; }
        private const  int size  = 5;
        public int SongLike { get; set; }
        public int SongDisLike { get; set; }
        [Display(Name = "View")]
        public int SongView { get; set; }
        [Display(Name = "Subscribe")]
        public int Subscriber { get; set; }
        public DateTime FromCreation { get; set; }
        
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Choose cover photo")]
        //[Required]
        public IFormFile CoverPhoto { get; set; }
        public string CoverImageUrl { get; set; }
        public DateTime? CreatedOn { get; set; }
        public List< CommentModel> Comment { get; set; }
        public CommentModel Comments { get; set; }

        [MyUploadFileSizeValidator(sizeInBytes: size * 1024 * 1024,
                               ErrorMessage = "Image filesize should be smaller than 5 MB")]
        [Display(Name = "Upload your song")]
        [Required]
        public IFormFile SongFile { get; set; }
        public string SongUrl { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string CalculateTime { get; set; }


    }
}
