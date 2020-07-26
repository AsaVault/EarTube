using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EarTube.Models;
using EarTube.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace EarTube.Controllers
{
    [Authorize]
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

        // Get - AddNewSong
        public ViewResult AddNewSong(bool isSuccess = false, int songId = 0)
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

        [Route("song-details/{id}", Name = "songDetails")]
        public async Task<ViewResult> GetSong(int id)
        {
            ViewBag.comment = new Comment();
            var data = await _songRepository.GetSongById(id);

            return View(data);
        }

        [Route("edit-song/{id}", Name = "editSong")]
        public async Task<IActionResult> EditSong(int? id, bool isSuccess = false)
        {
            ViewBag.IsSuccess = isSuccess;
            ViewBag.SongId = id;

            if (id == null)
            {
                return NotFound();
            }
            var data = await _songRepository.GetSongById(id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSong(SongModel songModel)
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

                int id = await _songRepository.EditSong(songModel);
                if (id > 0)
                {
                    return RedirectToAction(nameof(EditSong), new { isSuccess = true, songId = id });
                }
            }

            return View();
        }

        [Route("delete-song/{id}", Name = "deleteSong")]

        public async Task<IActionResult> DeleteSong(int? id, bool isSuccess = false)
        {
            ViewBag.IsSuccess = isSuccess;

            if (id == null)
            {
                return NotFound();
            }

            var data = await _songRepository.GetSongById(id);

            int removeSong = await _songRepository.DeleteSong(data);
            if (removeSong > 0)
            {
                return RedirectToAction(nameof(GetAllSongs), new { isSuccess = true });
            }


            return NotFound();
        }

        [Route("like-song/{id}", Name = "likeSong")]

        public async Task<IActionResult> LikeSong(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _songRepository.GetSongById(id);


            int likeSong = await _songRepository.LikeSong(data);
            data.SongLike += 1;
            if (likeSong > 0)
            {
                return RedirectToAction(nameof(GetAllSongs));
            }


            return NotFound();
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