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
        [StringLength(500)]
        public string Description { get; set; }
        //[Required(ErrorMessage = "Please choose the language of your book")]

        [Display(Name = "Choose the cover photo of your song")]
        [Required]
        public IFormFile CoverPhoto { get; set; }
        public string CoverImageUrl { get; set; }

        public List< CommentModel> Comment { get; set; }

        [Display(Name = "Upload your song in digital format")]
        [Required]
        public IFormFile SongFile { get; set; }
        public string SongUrl { get; set; }
    }
}
