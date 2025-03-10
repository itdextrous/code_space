using System.Collections.Generic;
using SAASCLOUDAPP.BusinessLayer;
using SAASCLOUDAPP.BusinessLayer.Dto;
using SAASCLOUDAPP.BusinessLayer.Dto.Legacy;
using SAASCLOUDAPP.CommonLayer.Models;
using SAASCLOUDAPP.CommonLayer.ViewModels;
using Workfacta.Shared.Services.Storage;

namespace SAASCLOUDAPP.API.Services
{
    /// <summary>
    /// A collection of functions designed to populate the <see cref="AttachmentVm.AttachmentUrl"/> for all children of the given object.
    /// </summary>
    public static class AttachmentStorageServiceExtensions
    {
        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, List<AttachmentVm> attachments)
        {
            if (attachments == null) return;
            attachments.ForEach(a =>
            {
                a.AttachmentUrl = storageService.GetPublicUri(a.AttachmentPath).OriginalString;
            });
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, IAttachmentHolder model)
        {
            storageService.PopulateAttachmentUrls(model?.attachments);
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, IEnumerable<IAttachmentHolder> models)
        {
            if (models == null) return;

            foreach (var model in models)
            {
                storageService.PopulateAttachmentUrls(model);
            }
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, CustomGoalsModel model)
        {
            if (model == null) return;
            storageService.PopulateAttachmentUrls(model?.actions);
            storageService.PopulateAttachmentUrls(model as IAttachmentHolder);
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, CustomGoalWeekModel model)
        {
            if (model == null) return;
            storageService.PopulateAttachmentUrls(model?.actions);
            storageService.PopulateAttachmentUrls(model as IAttachmentHolder);
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, IEnumerable<CustomGoalWeekModel> models)
        {
            if (models == null) return;

            foreach (var model in models)
            {
                storageService.PopulateAttachmentUrls(model);
            }
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, ActionsForMeetingListModel model)
        {
            if (model == null) return;
            storageService.PopulateAttachmentUrls(model?.actionsOverDueInNextSevenDays);
            storageService.PopulateAttachmentUrls(model?.actionsOverDue);
            storageService.PopulateAttachmentUrls(model?.actionsDueThisWeek);
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, actionsTreeModel model)
        {
            if (model == null) return;
            storageService.PopulateAttachmentUrls(model.fullData);
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, IEnumerable<actionsTreeModel> models)
        {
            if (models == null) return;

            foreach (var model in models)
            {
                storageService.PopulateAttachmentUrls(model);
            }
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, GoalActionsTreeModel model)
        {
            if (model == null) return;
            storageService.PopulateAttachmentUrls(model.fullData);
            storageService.PopulateAttachmentUrls(model.children);
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, IEnumerable<GoalActionsTreeModel> models)
        {
            if (models == null) return;

            foreach (var model in models)
            {
                storageService.PopulateAttachmentUrls(model);
            }
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, ActionsTreeModel model)
        {
            if (model == null) return;
            storageService.PopulateAttachmentUrls(model.fullData);
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, IEnumerable<ActionsTreeModel> models)
        {
            if (models == null) return;

            foreach (var model in models)
            {
                storageService.PopulateAttachmentUrls(model);
            }
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, IssueSolvedActionsTreeModel model)
        {
            if (model == null) return;
            storageService.PopulateAttachmentUrls(model.fullData);
            storageService.PopulateAttachmentUrls(model.children);
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, IEnumerable<IssueSolvedActionsTreeModel> models)
        {
            if (models == null) return;

            foreach (var model in models)
            {
                storageService.PopulateAttachmentUrls(model);
            }
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, InfoGramDataVM model)
        {
            if (model == null) return;
            storageService.PopulateAttachmentUrls(model.goalData);
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, goalWeeklyActionsTreeModel model)
        {
            if (model == null) return;
            storageService.PopulateAttachmentUrls(model.fullData);
            storageService.PopulateAttachmentUrls(model.children);
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, IEnumerable<goalWeeklyActionsTreeModel> models)
        {
            if (models == null) return;

            foreach (var model in models)
            {
                storageService.PopulateAttachmentUrls(model);
            }
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, GoalNumberReportVM model)
        {
            if (model == null) return;
            storageService.PopulateAttachmentUrls(model.numberWeekRecord);
            storageService.PopulateAttachmentUrls(model.CustomGoalWeek);
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, ChallengeOpportunityVM model)
        {
            if (model == null) return;
            storageService.PopulateAttachmentUrls(model.allChallenges);
            storageService.PopulateAttachmentUrls(model.allOpportunity);
            storageService.PopulateAttachmentUrls(model.allDiscussions);
        }
        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, List<IndividualDashboardVM> models)
        {
            if (models == null) return;
            foreach (var model in models)
            {
                storageService.PopulateAttachmentUrls(model.actions);
                storageService.PopulateAttachmentUrls(model.goals);
                storageService.PopulateAttachmentUrls(model.numbers);
            }
        }
        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, GetCustomDashboardGoalVM model)
        {
            if (model == null) return;
            storageService.PopulateAttachmentUrls(model.goalList);
        }
        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, GetCustomDashboardNumberVM model)
        {
            if (model == null) return;
            storageService.PopulateAttachmentUrls(model.numberList);
        }
        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, IEnumerable<GetAnnualGoalsModel> models)
        {
            if (models == null) return;
            foreach (var model in models)
            {
                storageService.PopulateAttachmentUrls(model);
            }
        }
        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, IEnumerable<AnnualGoalTreeModel> models)
        {
            if (models == null) return;
            foreach (var model in models)
            {
                storageService.PopulateAttachmentUrls(model?.fullData);
            }
        }

        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, IEnumerable<NewsVM> models)
        {
            if (models == null) return;

            foreach (var model in models)
            {
                storageService.PopulateAttachmentUrls(model?.Attachments);
            }
        }
        public static void PopulateAttachmentUrls(this IAttachmentStorageService storageService, GetNewsDto model)
        {
            storageService.PopulateAttachmentUrls(model?.Attachments);
        }
    }
}