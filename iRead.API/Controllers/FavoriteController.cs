﻿using iRead.API.Models;
using iRead.API.Models.Favorite;
using iRead.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace iRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoriteController : CustomControllerBase
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteController(IFavoriteRepository _favoriteRepository, ILogger<CustomControllerBase> logger) : base(logger)
        {
            this._favoriteRepository = _favoriteRepository;
        }

        [HttpGet]
        [Route("User/{userId}")]
        public async Task<ActionResult<IEnumerable<FavoriteResponse>>> Get(int userId)
        {
            var userFavorites = await _favoriteRepository.GetUserFavorites(userId);
            return ReturnIfNotEmpty(userFavorites, $"No favorites found for user with id {userId}.", false);
        }

        [HttpGet]
        [Route("User/{userId}/Book/{bookId}")]
        public async Task<ActionResult> FavoriteExists(int userId, int bookId)
        {
            var bookExists = await _favoriteRepository.FavoriteExists(bookId, userId);
            return ReturnResponse(ResponseType.Data, "", bookExists);
        }

        [HttpPost]
        public async Task<ActionResult<FavoriteResponse>> Create([FromBody] NewFavorite favorite)
        {
            try
            {
                var newFavorite = await _favoriteRepository.CreateFavorite(favorite);
                return ReturnResponse(ResponseType.Created, "", newFavorite);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }

        }

        [HttpDelete]
        [Route("User/{userId}/Book/{bookId}")]
        public async Task<ActionResult> Delete(int userId, int bookId)
        {
            try
            {
                if (!await _favoriteRepository.FavoriteExists(bookId, userId))
                    return ReturnResponse(ResponseType.NotFound, "This favorite was not found.");

                await _favoriteRepository.DeleteFavorite(bookId, userId);
                return ReturnResponse(ResponseType.Deleted, "Αφαιρέθηκε με επιτυχία.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ReturnResponse(ResponseType.Error);
            }
        }

    }
}
