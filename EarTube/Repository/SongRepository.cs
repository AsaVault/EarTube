﻿using EarTube.Areas.Identity.Data;
using EarTube.Data;
using EarTube.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.Repository
{
    public class SongRepository
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public SongRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<List<SongModel>> GetAllSongs()
        {

            return await _db.Song.Include(a => a.User)
                  .Select(song => new SongModel()
                  {
                      Title = song.Title,
                      Artist = song.Artist,
                      Genre = song.Genre,
                      Description = song.Description,
                      Id = song.Id,
                      Like = song.Like,
                      UserId = song.UserId,
                      User = song.User,
                      SongUrl = song.SongUrl,
                      CoverImageUrl = song.CoverImageUrl,
                      SongLike = song.SongLike,
                      CreatedOn = song.CreatedOn,
                      SongDisLike = song.SongDisLike,
                      SongView = song.SongView,
                      Subscriber = song.Subscriber,
                      FromCreation = song.FromCreation
                  }).OrderByDescending(song=>song.Id).ToListAsync();
        }


        public async Task<List<SongModel>> HotSongs()
        {

            return await _db.Song.Include(a => a.User)
                  .Select(song => new SongModel()
                  {
                      Title = song.Title,
                      Artist = song.Artist,
                      Genre = song.Genre,
                      Description = song.Description,
                      Id = song.Id,
                      Like = song.Like,
                      UserId = song.UserId,
                      User = song.User,
                      SongUrl = song.SongUrl,
                      CoverImageUrl = song.CoverImageUrl,
                      SongLike = song.SongLike,
                      CreatedOn = song.CreatedOn,
                      SongDisLike = song.SongDisLike,
                      SongView = song.SongView,
                      Subscriber = song.Subscriber,
                      FromCreation = song.FromCreation
                  }).OrderByDescending(song => song.SongView).ToListAsync();
        }

        public async Task<List<SongModel>> GetSongByUser(string userId)
        {

            return await _db.Song.Include(a => a.User).Where(s => s.UserId == userId)
                  .Select(song => new SongModel()
                  {
                      Title = song.Title,
                      Artist = song.Artist,
                      Genre = song.Genre,
                      Description = song.Description,
                      Id = song.Id,
                      Like = song.Like,
                      UserId = song.UserId,
                      User = song.User,
                      SongUrl = song.SongUrl,
                      CoverImageUrl = song.CoverImageUrl,
                      SongLike = song.SongLike,
                      SongDisLike = song.SongDisLike,
                      SongView = song.SongView,
                      Subscriber = song.Subscriber,
                      CreatedOn = song.CreatedOn,
                      FromCreation = song.FromCreation
                  }).ToListAsync();
        }

        public async Task<int> AddNewSong(SongModel model)
        {
            var newSong = new Song()
            {
                Title = model.Title,
                Artist = model.Artist,
                Genre = model.Genre,
                CreatedOn = DateTime.UtcNow,
                Description = model.Description,
                Id = model.Id,
                Like = model.Like,
                SongUrl = model.SongUrl,
                UserId = model.UserId,
                User = model.User,
                CoverImageUrl = model.CoverImageUrl,
                SongView = model.SongView,
                Subscriber = model.Subscriber,
                FromCreation = model.FromCreation 
            };

            //newSong.Comment = new List<Comment>();

            //foreach (var comment in model.Comment)
            //{
            //    newSong.Comment.Add(new Comment()
            //    {
            //        Title = comment.Title,
            //        Description = comment.Description
            //    });
            //}

            await _db.Song.AddAsync(newSong);
            await _db.SaveChangesAsync();

            return newSong.Id;

        }

        public async Task<int> EditSong(SongModel model)
        {
            var newSong = new Song()
            {
                Title = model.Title,
                Artist = model.Artist,
                Genre = model.Genre,
                UpdatedOn = DateTime.UtcNow,
                Description = model.Description,
                Id = model.Id,
                Like = model.Like,
                SongUrl = model.SongUrl,
                CoverImageUrl = model.CoverImageUrl
            };

            //newSong.Comment = new List<Comment>();

            //foreach (var comment in model.Comment)
            //{
            //    newSong.Comment.Add(new Comment()
            //    {
            //        Title = comment.Title,
            //        Description = comment.Description
            //    });
            //}

            _db.Song.Update(newSong);
            await _db.SaveChangesAsync();

            return newSong.Id;

        }

        public async Task<int> DeleteSong(SongModel model)
        {
            var newSong = new Song()
            {
                Title = model.Title,
                Artist = model.Artist,
                Genre = model.Genre,
                UpdatedOn = DateTime.UtcNow,
                CreatedOn = DateTime.UtcNow,
                Description = model.Description,
                Id = model.Id,
                Like = model.Like,
                SongUrl = model.SongUrl,
                CoverImageUrl = model.CoverImageUrl
            };

            _db.Song.Remove(newSong);
            await _db.SaveChangesAsync();

            return newSong.Id;

        }

        public async Task<bool> LikeSong(SongModel model, string userId)
        {
            var likeSong = false;
            var userSongLike = await _db.UserSongLike.AnyAsync(u => u.UserId == userId && u.SongId == model.Id);
            if (!userSongLike)
            {
                var newSong = await _db.Song.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                newSong.SongLike += 1;
                _db.Song.Update(newSong);
                _db.UserSongLike.Add(new UserSongLike { UserId = userId, SongId = model.Id });
                await _db.SaveChangesAsync();
                likeSong = true;
            }

            return likeSong;
        }


        //UserLikeSongFormer

        public async Task<int> UserLikeSong(SongModel model)
        {
            var newSong = await _db.Song.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            newSong.SongLike += 1;
            _db.Song.Update(newSong);
            await _db.SaveChangesAsync();
            return newSong.Id;
        }


        //Dislike Song
        public async Task<bool> DisikeSong(SongModel model, string userId)
        {
            var dislikeSong = false;
            var userSongDislike = await _db.UserSongDislike.AnyAsync(u => u.UserId == userId && u.SongId == model.Id);
            if (!userSongDislike)
            {
                var newSong = await _db.Song.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                newSong.SongDisLike += 1;
                _db.Song.Update(newSong);
                _db.UserSongDislike.Add(new UserSongDislike { UserId = userId, SongId = model.Id });
                await _db.SaveChangesAsync();
                dislikeSong = true;
            }

            return dislikeSong;
        }

        //Youtube Like Song Logic

        public async Task<bool> YoutubeLikeSong(SongModel model, string userId)
        {
            var likeSong = false;
            var userSongLike = await _db.UserSongLike.AnyAsync(u => u.UserId == userId && u.SongId == model.Id);
            if (!userSongLike)
            {
                var newSong = await _db.Song.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                var userSongDislike = await _db.UserSongDislike.AnyAsync(u => u.UserId == userId && u.SongId == model.Id);
                if (!userSongDislike)
                {
                    newSong.SongLike += 1;
                }
                else
                {
                    var dislikeData = await _db.UserSongDislike.FirstOrDefaultAsync(u => u.UserId == userId && u.SongId == model.Id);
                    _db.UserSongDislike.Remove(dislikeData);
                    newSong.SongDisLike -= 1;
                    newSong.SongLike += 1;
                }

                _db.Song.Update(newSong);
                _db.UserSongLike.Add(new UserSongLike { UserId = userId, SongId = model.Id });
                await _db.SaveChangesAsync();
                likeSong = true;
            }

            return likeSong;
        }

        //Youtube dislike song logic
        public async Task<bool> YoutubeDisikeSong(SongModel model, string userId)
        {
            var dislikeSong = false;
            var userSongDislike = await _db.UserSongDislike.AnyAsync(u => u.UserId == userId && u.SongId == model.Id);
            if (!userSongDislike)
            {
                var newSong = await _db.Song.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                var userSongLike = await _db.UserSongLike.AnyAsync(u => u.UserId == userId && u.SongId == model.Id);
                if (!userSongLike)
                {
                    newSong.SongDisLike += 1;
                }
                else
                {
                    var likeData = await _db.UserSongLike.FirstOrDefaultAsync(u => u.UserId == userId && u.SongId == model.Id);
                    _db.UserSongLike.Remove(likeData);
                    newSong.SongLike -= 1;
                    newSong.SongDisLike += 1;
                }


                _db.Song.Update(newSong);
                _db.UserSongDislike.Add(new UserSongDislike { UserId = userId, SongId = model.Id });
                await _db.SaveChangesAsync();
                dislikeSong = true;
            }

            return dislikeSong;
        }

        //Still needed some touch
        public async Task<SongModel> GetSongById(int? id)
        {
            var fromDb = await _db.Song.FirstOrDefaultAsync(x =>x.Id == id);
            fromDb.SongView += 1;
            _db.Song.Update(fromDb);
            await _db.SaveChangesAsync();

             var result = await _db.Song.Where(x => x.Id == id).Include(c => c.Comment)
                 .Select(song => new SongModel()
                 {
                     Title = song.Title,
                     Artist = song.Artist,
                     Genre = song.Genre,
                     Description = song.Description,
                     Id = song.Id,
                     Like = song.Like,
                     SongLike = song.SongLike,
                     SongUrl = song.SongUrl,
                     SongDisLike = song.SongDisLike,
                     UserId = song.UserId,
                     SongView = song.SongView,
                     Subscriber = song.Subscriber,
                     FromCreation = song.FromCreation,
                     CoverImageUrl = song.CoverImageUrl,
                     Comment = song.Comment.Select(g => new CommentModel()
                     {
                         Id = g.Id,
                         Title = g.Title,
                         Description = g.Description,
                         User = g.User,
                         CommentDisikes = g.CommentDisikes,
                         CommentLikes = g.CommentLikes
                     }).OrderByDescending(c=>c.Id).ToList()
                 }).FirstOrDefaultAsync();
            return result;
        }


        public List<SongModel> SearchBook(string title, string authorName)
        {
            return null;
        }
    }
}