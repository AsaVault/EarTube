using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EarTube.Models;
using EarTube.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EarTube.Controllers
{
    public class SongController : Controller
    {
        private readonly SongRepository _songRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SongController(SongRepository songRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _songRepository = songRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ViewResult> GetAllSongs()
        {
            var data = await _songRepository.GetAllSongs();

            return View(data);
        }
        public  ViewResult AddNewSong(bool isSuccess = false, int songId = 0)
        {
            var model = new SongModel();

            ViewBag.IsSuccess = isSuccess;
            ViewBag.SongId = songId;
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddNewSong(SongModel songModel)
        {
            if (ModelState.IsValid)
            {
                if (songModel.CoverPhoto != null)
                {
                    string folder = "songs/cover/";
                    songModel.CoverImageUrl = await UploadImage(folder, songModel.CoverPhoto);
                }

                

                if (songModel.SongFile != null)
                {
                    string folder = "songs/songfiles/";
                    songModel.SongUrl = await UploadImage(folder, songModel.SongFile);
                }

                int id = await _songRepository.AddNewSong(songModel);
                if (id > 0)
                {
                    return RedirectToAction(nameof(AddNewSong), new { isSuccess = true, songId = id });
                }
            }

            return View();
        }

        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }
    }
}