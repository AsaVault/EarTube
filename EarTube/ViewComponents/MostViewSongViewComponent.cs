using EarTube.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.ViewComponents
{
    public class MostViewSongViewComponent :ViewComponent
    {
        private readonly SongRepository _repo;

        public MostViewSongViewComponent(SongRepository repo)
        {
            _repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var mostView = await _repo.HotSongs();

            foreach (var data in mostView)
            {
                data.CalculateTime = _repo.CalculateTime(data.CreatedOn.GetValueOrDefault());
            }
            return View(mostView);
        }
    }
}
