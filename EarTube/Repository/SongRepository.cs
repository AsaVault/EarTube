using EarTube.Data;
using EarTube.Models;
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
        public SongRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<SongModel>> GetAllSongs()
        {
            return await _db.Song.Include(a=>a.User)
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
                      SongLike = song.SongLike
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
                UpdatedOn = DateTime.UtcNow,
                Description = model.Description,
                Id = model.Id,
                Like = model.Like,
                SongUrl = model.SongUrl,
                UserId = model.UserId,
                User = model.User,
                CoverImageUrl = model.CoverImageUrl
            };

            newSong.Comment = new List<Comment>();

            foreach (var comment in model.Comment)
            {
                newSong.Comment.Add(new Comment()
                {
                    Title = comment.Title,
                    Description = comment.Description
                });
            }

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

            newSong.Comment = new List<Comment>();

            foreach (var comment in model.Comment)
            {
                newSong.Comment.Add(new Comment()
                {
                    Title = comment.Title,
                    Description = comment.Description
                });
            }

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

        public async Task<int> LikeSong(SongModel model)
        {


            //var newSong = new Song()
            //{
            //    Title = model.Title,
            //    Artist = model.Artist,
            //    Genre = model.Genre,
            //    UpdatedOn = DateTime.UtcNow,
            //    Description = model.Description,
            //    Id = model.Id,
            //    Like = model.Like,
            //    SongUrl = model.SongUrl,
            //    CoverImageUrl = model.CoverImageUrl,
            //    SongLike = model.SongLike
            //};
            //var newSong = await _db.Song.Where(x => x.Id == id).FirstOrDefaultAsync();

            var newSong = await _db.Song.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                
            newSong.SongLike += 1;
            //model.SongLike += 1;
            newSong.SongLike = model.SongLike;

            _db.Song.Update(newSong);
            await _db.SaveChangesAsync();
            return newSong.Id;
        }




        //Still needed some touch
        public async Task<SongModel> GetSongById(int? id)
        {

            return await _db.Song.Where(x => x.Id == id )
                 .Select(song => new SongModel()
                 {
                     Title = song.Title,
                     Artist = song.Artist,
                     Genre = song.Genre,
                     Description = song.Description,
                     Id = song.Id,
                     Like = song.Like,
                     //SongUrl = song.SongUrl,
                     UserId = song.UserId,
                     CoverImageUrl = song.CoverImageUrl,
                     Comment = song.Comment.Select(g => new CommentModel()
                     {
                         Id = g.Id,
                         Title = g.Title,
                         Description = g.Description,
                         User = g.User
                     }).ToList()
                 }).FirstOrDefaultAsync();

            //return await _db.Song.Where(x => x.Id == id).Include(x => x.Comment).FirstOrDefaultAsync();



        }


        public List<SongModel> SearchBook(string title, string authorName)
        {
            return null;
        }
    }
}