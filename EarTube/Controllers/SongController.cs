using EarTube.Areas.Identity.Data;
using EarTube.Models;
using EarTube.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.Controllers
{
    [Authorize]
    public class SongController : Controller
    {
        private readonly SongRepository _songRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public SongController(SongRepository songRepository,
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager)
        {
            _songRepository = songRepository;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        public async Task<ViewResult> GetAllSongs(bool isSuccess, int songId)

        {
            var userId = _userManager.GetUserId(this.HttpContext.User);
            var datas = await _songRepository.GetAllSongs();

            ViewBag.IsSuccess = TempData["Alert"];
            //isSuccess = false;
            TempData["Alert"] = false;
            ViewBag.SongId = songId;

            //foreach (var data in datas)
            //{
            //    data.UserId = userId;
            //}
            return View(datas);
            //return RedirectToAction(datas, new { isSuccess = true, songId = datas.Count() });
        }


        //Get Song by UserId

        public async Task<IActionResult> GetSongByUser(/*bool isSuccess, int songId*/)

        {
            var userId = _userManager.GetUserId(this.HttpContext.User);
            var datas = await _songRepository.GetSongByUser(userId);


            if (datas.Count < 0)
                return RedirectToAction(nameof(GetAllSongs));


             return View(datas);
            //return RedirectToAction(datas, new { isSuccess = true, songId = datas.Count() });
        }

        // Get - AddNewSong
        public ViewResult AddNewSong(/*bool isSuccess = false, int songId = 0*/)
        {
            var model = new SongModel();

            //ViewBag.IsSuccess = isSuccess;
            //ViewBag.SongId = songId;
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddNewSong(SongModel songModel, bool isSuccess = false, int songId = 0)
        {
            var userId = _userManager.GetUserId(this.HttpContext.User);
            songModel.UserId = userId;
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

                ViewBag.IsSuccess = isSuccess;
                ViewBag.SongId = songId;

                int id = await _songRepository.AddNewSong(songModel);
                if (id > 0)
                {
                    //return RedirectToAction(nameof(AddNewSong), new { isSuccess = true, songId = id });
                    TempData["Alert"] = true;
                    return RedirectToAction(nameof(GetAllSongs), new { songId = id});
                }
            }

            return View();
        }

        [Route("song-details/{id}", Name = "songDetails")]
        public async Task<ViewResult> GetSong(int id, bool isSuccess)
        {
            var userId = _userManager.GetUserId(this.HttpContext.User);

            //ViewBag.comment = new Comment();
            //ViewBag.IsSuccess = TempData["Alert"];
            //isSuccess = false;
            
            var data = await _songRepository.GetSongById(id);
            ViewBag.IsSuccess = TempData["Alert"];
            TempData["Alert"] = false;
            //data.UserId = userId;

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