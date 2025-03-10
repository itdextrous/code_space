using System.Collections.Generic;
using System.Threading.Tasks;
using SAASCLOUDAPP.BusinessLayer.Dto;
using SAASCLOUDAPP.CommonLayer.ViewModels;
using Workfacta.Entities;

namespace SAASCLOUDAPP.BusinessLayer.Services.Interfaces
{
    public interface INewsService
    {
        Task<string> Create(NewsDto dto, string currentUserId);
        Task<string> Update(NewsDto dto, string currentUserId);
        Task<bool> SetNewsNoted(string id, string teamId, string currentUserId);
        Task<bool> Delete(string id, string teamId, string currentUserId);
        Task<GetNewsDto> FindById(string id, string teamId);
        Task<IEnumerable<NewsVM>> GetAllNews(string teamId, bool isAllRecord = false, int? skip = null, int? take = null);
        Task<IEnumerable<NewsVM>> GetNewsForMeeting(string teamId, int? skip = null, int? take = null);
        Task<List<GetTeamsDDNForNewsVM>> GetTeamsDDNForNews(string companyId, string teamId);
        Task<NewsVM> AddUpdateTeamNotes(string id, string teamId, string notesText);
        Task<GetNewsDto> UpdateNewsAttachments(string newsId, List<string> existingAttachments, List<Attachment> newAttachments);
        Task<NewsVM> AddUpdateTeamAttachments(string id, List<string> existingAttachments, List<Attachment> newAttachments, string teamId);
    }
}
