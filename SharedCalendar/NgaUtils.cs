using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hpe.Nga.Api.Core.Entities;
using Hpe.Nga.Api.Core.Services;
using Hpe.Nga.Api.Core.Services.Query;
using Hpe.Nga.Api.Core.Services.RequestContext;
using Hpe.Nga.Api.Core.Services.GroupBy;

namespace SharedCalendar
{
    public static class NgaUtils
    {
        private static EntityService entityService = EntityService.GetInstance();
        private static WorkspaceContext workspaceContext;
        private static long selectedReleaseId = 0;

        public static void init(long sharedSpaceId, long workspaceId, long releaseId)
        {
            workspaceContext = GetWorkspaceContext(sharedSpaceId, workspaceId);
            selectedReleaseId = releaseId;
        }

        private static WorkspaceContext GetWorkspaceContext(long sharedSpaceId, long workspaceId)
        {
            SharedSpaceContext sharedSpaceContext = new SharedSpaceContext(sharedSpaceId);
            //EntityListResult<Workspace> workspaces = entityService.Get<Workspace>(sharedSpaceContext);
            //Workspace workspace = workspaces.data[0];
            //long workspaceId = 2029;//workspaces.data[0].Id hardcoded workaround
            return new WorkspaceContext(sharedSpaceId, workspaceId);
        }

        public static Release GetSelectedRelease()
        {
            return GetReleaseById(selectedReleaseId);
        }

        public static Release GetReleaseById(long id)
        {
            List<String> fields = new List<string>();
            fields.Add(Release.NAME_FIELD);
            fields.Add(Release.START_DATE_FIELD);
            fields.Add(Release.END_DATE_FIELD);
            fields.Add(Release.NUM_OF_SPRINTS_FIELD);
            Release release = entityService.GetById<Release>(workspaceContext, id, fields);

            Debug.Assert(release.Id == id);
            return release;
        }

        public static EntityListResult<Sprint> GetSprintsByRelease(long releaseId)
        {
            List<String> fields = new List<string>();
            fields.Add(Sprint.NAME_FIELD);
            fields.Add(Sprint.START_DATE_FIELD);
            fields.Add(Sprint.END_DATE_FIELD);
            fields.Add(Sprint.RELEASE_FIELD);

            List<QueryPhrase> queryPhrases = new List<QueryPhrase>();
            QueryPhrase releaseIdPhrase = new LogicalQueryPhrase("id", releaseId);
            QueryPhrase byReleasePhrase = new CrossQueryPhrase(Sprint.RELEASE_FIELD, releaseIdPhrase);

            queryPhrases.Add(byReleasePhrase);

            EntityListResult<Sprint> result = entityService.Get<Sprint>(workspaceContext, queryPhrases, fields);
            Release release = result.data[0].Release;
            Debug.Assert(release.Id == releaseId);
            return result;
        }

        public static EntityListResult<Milestone> GetMilestonesByRelease(long releaseId)
        {
            List<String> fields = new List<string>();
            fields.Add(Milestone.NAME_FIELD);
            fields.Add(Milestone.DATE_FIELD);
            fields.Add(Milestone.RELEASES_FIELD);
            fields.Add(Milestone.DESCRIPTION_FIELD);

            List<QueryPhrase> queryPhrases = new List<QueryPhrase>();
            QueryPhrase releaseIdPhrase = new LogicalQueryPhrase("id", releaseId);
            QueryPhrase byReleasePhrase = new CrossQueryPhrase(Milestone.RELEASES_FIELD, releaseIdPhrase);

            queryPhrases.Add(byReleasePhrase);

            EntityListResult<Milestone> result = entityService.Get<Milestone>(workspaceContext, queryPhrases, fields);
            return result;
        }

        public static GroupResult GetAllDefectWithGroupBy(long releaseId)
        {
            List<String> fields = new List<string>();
            fields.Add(WorkItem.NAME_FIELD);
            fields.Add(WorkItem.SUBTYPE_FIELD);


            List<QueryPhrase> queries = new List<QueryPhrase>();
            LogicalQueryPhrase subtypeQuery = new LogicalQueryPhrase(WorkItem.SUBTYPE_FIELD, WorkItem.SUBTYPE_DEFECT);
            queries.Add(subtypeQuery);
            QueryPhrase releaseIdPhrase = new LogicalQueryPhrase("id", releaseId);
            QueryPhrase byReleasePhrase = new CrossQueryPhrase(WorkItem.RELEASE_FIELD, releaseIdPhrase);
            queries.Add(byReleasePhrase);
            LogicalQueryPhrase phaseNamePhrase = new LogicalQueryPhrase("name", "Done");
            phaseNamePhrase.NegativeCondition = true;
            CrossQueryPhrase phaseIdPhrase = new CrossQueryPhrase("metaphase", phaseNamePhrase);
            CrossQueryPhrase byPhasePhrase = new CrossQueryPhrase(WorkItem.PHASE_FIELD, phaseIdPhrase);
            queries.Add(byPhasePhrase);

            GroupResult result = entityService.GetWithGroupBy<WorkItem>(workspaceContext, queries, "severity");
            return result;
        }

        public static GroupResult GetAllStoriesWithGroupBy(long releaseId)
        {
            List<String> fields = new List<string>();
            fields.Add(WorkItem.NAME_FIELD);
            fields.Add(WorkItem.SUBTYPE_FIELD);


            List<QueryPhrase> queries = new List<QueryPhrase>();
            LogicalQueryPhrase subtypeQuery = new LogicalQueryPhrase(WorkItem.SUBTYPE_FIELD, WorkItem.SUBTYPE_STORY);
            queries.Add(subtypeQuery);
            QueryPhrase releaseIdPhrase = new LogicalQueryPhrase("id", releaseId);
            QueryPhrase byReleasePhrase = new CrossQueryPhrase(WorkItem.RELEASE_FIELD, releaseIdPhrase);
            queries.Add(byReleasePhrase);
            LogicalQueryPhrase phaseNamePhrase = new LogicalQueryPhrase("name", "Done");
            phaseNamePhrase.NegativeCondition = true;
            CrossQueryPhrase phaseIdPhrase = new CrossQueryPhrase("metaphase", phaseNamePhrase);
            CrossQueryPhrase byPhasePhrase = new CrossQueryPhrase(WorkItem.PHASE_FIELD, phaseIdPhrase);
            queries.Add(byPhasePhrase);

            GroupResult result = entityService.GetWithGroupBy<WorkItem>(workspaceContext, queries, WorkItem.PHASE_FIELD);
            return result;
        }

        public static EntityListResult<WorkItem> GetStoriesByRelease(long releaseId)
        {
            List<String> fields = new List<string>();
            fields.Add(WorkItem.NAME_FIELD);
            fields.Add(WorkItem.SUBTYPE_FIELD);

            List<QueryPhrase> queryPhrases = new List<QueryPhrase>();
            List<QueryPhrase> queries = new List<QueryPhrase>();
            LogicalQueryPhrase subtypeQuery = new LogicalQueryPhrase(WorkItem.SUBTYPE_FIELD, WorkItem.SUBTYPE_STORY);
            queryPhrases.Add(subtypeQuery);
            QueryPhrase releaseIdPhrase = new LogicalQueryPhrase("id", releaseId);
            QueryPhrase byReleasePhrase = new CrossQueryPhrase(WorkItem.RELEASE_FIELD, releaseIdPhrase);
            queryPhrases.Add(byReleasePhrase);
            LogicalQueryPhrase phaseNamePhrase = new LogicalQueryPhrase("name", "Done");
            phaseNamePhrase.NegativeCondition = true;
            CrossQueryPhrase phaseIdPhrase = new CrossQueryPhrase("metaphase", phaseNamePhrase);
            CrossQueryPhrase byPhasePhrase = new CrossQueryPhrase(WorkItem.PHASE_FIELD, phaseIdPhrase);
            queryPhrases.Add(byPhasePhrase);


            EntityListResult<WorkItem> result = entityService.Get<WorkItem>(workspaceContext, queryPhrases, fields, 1);
            return result;
        }

    }
}
