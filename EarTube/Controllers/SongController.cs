using EarTube.Areas.Identity.Data;
using EarTube.Helpers;
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
using System.Security.Claims;
using System.Threading.Tasks;

namespace EarTube.Controllers
{
    [Authorize]
    public class SongController : Controller
    {
        private readonly SongRepository _songRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SongController(SongRepository songRepository,
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _songRepository = songRepository;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ViewResult> GetAllSongs(bool isSuccess, int? songId)

        {
            //var userId = _userManager.GetUserId(this.HttpContext.User);
            var datas = await _songRepository.GetAllSongs();

            ViewBag.IsSuccess = TempData["Alert"];
            ViewBag.SongId = TempData["SongID"];
            //isSuccess = false;
            TempData["Alert"] = false;
            TempData["SongID"] = 0;

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

        //AddOrEdit Action Method
        public async Task<IActionResult> AddOrEdit( int? songId = 0, bool isSuccess = false)
        {
            if(songId == 0)
            {
                var model = new SongModel();
                return View(model);
            }
            else
            {
                ViewBag.IsSuccess = isSuccess;
                ViewBag.SongId = songId;
                if (songId == null)
                {
                    return NotFound();
                }
                var data = await _songRepository.GetSongById(songId);
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
            }
        }

        //Post AddOrEdit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(SongModel songModel, bool isSuccess = false, int songId = 0)
        {
            if (ModelState.IsValid)
            {
                if(songId == 0)
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

                        

                        int id = await _songRepository.AddNewSong(songModel);
                        if (id > 0)
                        {
                            TempData["Alert"] = true;
                            TempData["SongID"] = id;
                            //return RedirectToAction(nameof(GetAllSongs), new { songId = Id });
                        }

                        //ViewBag.IsSuccess = isSuccess;
                        //ViewBag.SongId = id;
                    }
                    //return View();
                }
                else
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

                    await _songRepository.EditSong(songModel);
                    //if (id > 0)
                    //{
                    //    //return RedirectToAction(nameof(EditSong), new { isSuccess = true, songId = id });
                    //    //return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "GetAllSongs", _songRepository.GetAllSongs()) });
                    //}
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _songRepository.GetAllSongs()) });
            }
            //return View(songModel);
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", songModel) });
        }
        

        [Route("song-details/{id}", Name = "songDetails")]
        public async Task<ViewResult> GetSong(int id, bool isSuccess)
        {
            //var userId = _userManager.GetUserId(this.HttpContext.User);

            //ViewBag.comment = new Comment();
            //ViewBag.IsSuccess = TempData["Alert"];
            //isSuccess = false;
            
            var data = await _songRepository.GetSongById(id);
            ViewBag.IsSubscribe = await CheckSubscribe(id); 
            ViewBag.IsSuccess = TempData["Alert"];
            ViewBag.LikeSuccess = TempData["LikeAlert"];
            ViewBag.IsLikeSuccess = TempData["AlreadyLikeAlert"];
            ViewBag.DislikeSuccess = TempData["DislikeAlert"];
            ViewBag.IsDislikeSuccess = TempData["AlreadyDislikeAlert"];
            TempData["Alert"] = false;
            TempData["LikeAlert"] = false;
            TempData["AlreadyLikeAlert"] = false;
            TempData["DislikeAlert"] = false;
            TempData["AlreadyDislikeAlert"] = false;

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

        //Youtube Like song
        [Route("youtube-like-song/{id}", Name = "youtubeLikeSong")]

        public async Task<IActionResult> YoutubeLikeSong(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _songRepository.GetSongById(id);
            
            var userId = _userManager.GetUserId(this.HttpContext.User);
            bool youtubeLikeSong = await _songRepository.YoutubeLikeSong(data, userId);
            //data.SongLike += 1;
            if (youtubeLikeSong)
            {
                TempData["LikeAlert"] = true;
                TempData["AlreadyLikeAlert"] = false;
                return RedirectToAction(nameof(GetSong), new { id = data.Id });
            }

            TempData["LikeAlert"] = false;
            TempData["AlreadyLikeAlert"] = true;
            return RedirectToAction(nameof(GetSong), new { id = data.Id });
        }

        //Youtube Dislike song logic
        //Youtube Like song
        [Route("youtube-dislike-song/{id}", Name = "youtubeDislikeSong")]

        public async Task<IActionResult> YoutubeDislikeSong(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _songRepository.GetSongById(id);
            var userId = _userManager.GetUserId(this.HttpContext.User);
            bool youtubeDislikeSong = await _songRepository.YoutubeDisikeSong(data, userId);
            //data.SongLike += 1;
            if (youtubeDislikeSong)
            {
                TempData["DislikeAlert"] = true;
                TempData["AlreadyDislikeAlert"] = false;
                return RedirectToAction(nameof(GetSong), new { id = data.Id });
            }

            TempData["DislikeAlert"] = false;
            TempData["AlreadyDislikeAlert"] = true;
            return RedirectToAction(nameof(GetSong), new { id = data.Id });
        }

        //Like Song New
        [Route("like-song/{id}", Name = "likeSong")]

        public async Task<IActionResult> LikeSong(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var data = await _songRepository.GetSongById(id);
            var userId = _userManager.GetUserId(this.HttpContext.User);
            bool likeSong = await _songRepository.LikeSong(data, userId);
            //data.SongLike += 1;
            if (likeSong)
            {
                TempData["LikeAlert"] = true;
                TempData["AlreadyLikeAlert"] = false;
                return RedirectToAction(nameof(GetSong), new { id = data.Id });
            }

            TempData["LikeAlert"] = false;
            TempData["AlreadyLikeAlert"] = true;
            return RedirectToAction(nameof(GetSong), new { id = data.Id });
        }

        //UserSongLikeFormer

        public async Task<IActionResult> UserSongLike(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _songRepository.GetSongById(id);

            int likeSong = await _songRepository.UserLikeSong(data);
            //data.SongLike += 1;
            if (likeSong > 0)
            {
                return RedirectToAction(nameof(GetSong), new { id = data.Id });
            }


            return NotFound();
        }

        //Song Dislike
        [Route("dislike-song/{id}", Name = "dislikeSong")]

        public async Task<IActionResult> DislikeSong(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _songRepository.GetSongById(id);
            var userId = _userManager.GetUserId(this.HttpContext.User);
            bool dislikeSong = await _songRepository.DisikeSong(data, userId);
            //data.SongLike += 1;
            if (dislikeSong)
            {
                TempData["DislikeAlert"] = true;
                TempData["AlreadyDislikeAlert"] = false;
                return RedirectToAction(nameof(GetSong), new { id = data.Id });
            }

            TempData["DislikeAlert"] = false;
            TempData["AlreadyDislikeAlert"] = true;
            return RedirectToAction(nameof(GetSong), new { id = data.Id });
        }

        [Route("subscribe-account/{id}", Name = "subscribe")]
        public async Task<IActionResult> SubscribeGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //var data = await _songRepository.GetSongById(id);
            //var subscribe = await CheckSubscribe(id);
            //data.SongLike += 1;
            var data = await _songRepository.GetSongById(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var accountUserId = data.UserId;
            var userId = _userManager.GetUserId(this.HttpContext.User);
            var userEmail = user.Email;

            bool subscribe = await _songRepository.SubscribeRepo(data, accountUserId, userId, userEmail);
            if (subscribe)
            {
                return RedirectToAction(nameof(GetSong), new { id = data.Id });
            }
            return RedirectToAction(nameof(GetSong), new { id = data.Id });
        }


        //Check Subscribe
        public async Task<bool> CheckSubscribe(int? id)
        {
                var data = await _songRepository.GetSongById(id);
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var accountUserId = data.UserId;
                var userId = _userManager.GetUserId(this.HttpContext.User);
                var userEmail = user.Email;

                bool subscribe = await _songRepository.SubscribeStatus(data, accountUserId, userId, userEmail);

            return subscribe;
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