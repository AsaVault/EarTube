using EarTube.Data;
using EarTube.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarTube.Repository
{
    public interface ISongRepository
    {
        Task<bool> AccountUserStatus(string accountUserId, string userId);
        Task<int> AddNewSong(SongModel model);
        string CalculateTime(DateTime createdOn);
        Task<int> DeleteSong(SongModel model);
        Task<bool> DisikeSong(SongModel model, string userId);
        Task<int> EditSong(SongModel model);
        Task<List<SongModel>> GetAllSongs();
        Task<SongModel> GetSongById(int? id);
        Task<List<SongModel>> GetSongByUser(string userId);
        List<AccountSubscriber> GetSubscriber(string userId);
        Task<List<SongModel>> HotSongs();
        Task<bool> LikeSong(SongModel model, string userId);
        Task<bool> OldSubscribeRepo(SongModel model, string accountUserId, string userId, string userEmail);
        List<SongModel> SearchSong(string title, string authorName);
        Task<bool> SubscribeRepo(SongModel model, string accountUserId, string userId, string userEmail, string userFirstName, string userLastName);
        Task<bool> SubscribeStatus(SongModel model, string accountUserId, string userId, string userEmail);
        Task<int> UserLikeSong(SongModel model);
        Task<bool> YoutubeDisikeSong(SongModel model, string userId);
        Task<bool> YoutubeLikeSong(SongModel model, string userId);
    }
}