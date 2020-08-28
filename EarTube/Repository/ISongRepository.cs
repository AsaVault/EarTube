using EarTube.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EarTube.Repository
{
    public interface ISongRepository
    {
        Task<int> AddNewSong(SongModel model);
        Task<int> DeleteSong(SongModel model);
        Task<bool> DisikeSong(SongModel model, string userId);
        Task<int> EditSong(SongModel model);
        Task<List<SongModel>> GetAllSongs();
        Task<SongModel> GetSongById(int? id);
        Task<List<SongModel>> GetSongByUser(string userId);
        Task<List<SongModel>> HotSongs();
        Task<bool> LikeSong(SongModel model, string userId);
        List<SongModel> SearchBook(string title, string authorName);
        Task<int> UserLikeSong(SongModel model);
        Task<bool> YoutubeDisikeSong(SongModel model, string userId);
        Task<bool> YoutubeLikeSong(SongModel model, string userId);
    }
}