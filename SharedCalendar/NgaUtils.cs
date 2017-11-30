// (c) Copyright 2016 Hewlett Packard Enterprise Development LP

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.

// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,

// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

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
using Hpe.Nga.Api.UI.Core.Configuration;

namespace SharedCalendar
{
    public static class NgaUtils
    {
        private static WorkspaceContext workspaceContext;
        private static string selectedReleaseId = "0";

        public static void init(string sharedSpaceId, string workspaceId, string releaseId)
        {
            workspaceContext = GetWorkspaceContext(sharedSpaceId, workspaceId);
            selectedReleaseId = releaseId;
        }

        private static WorkspaceContext GetWorkspaceContext(string sharedSpaceId, string workspaceId)
        {
            return WorkspaceContext.Create(sharedSpaceId, workspaceId);
        }

        public static Release GetSelectedRelease()
        {
            return GetReleaseById(selectedReleaseId);
        }

        public static Release GetReleaseById(string id)
        {
            List<String> fields = new List<string>();
            fields.Add(Release.NAME_FIELD);
            fields.Add(Release.START_DATE_FIELD);
            fields.Add(Release.END_DATE_FIELD);
            fields.Add(Release.NUM_OF_SPRINTS_FIELD);
            Release release = SettingsForm.EntityService.GetById<Release>(workspaceContext, id, fields);

            Debug.Assert(release.Id == id);
            return release;
        }

        public static EntityListResult<Sprint> GetSprintsByRelease(string releaseId)
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

            EntityListResult<Sprint> result = SettingsForm.EntityService.Get<Sprint>(workspaceContext, queryPhrases, fields);
            Release release = result.data[0].Release;
            return result;
        }

        public static EntityListResult<Milestone> GetMilestonesByRelease(string releaseId)
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

            EntityListResult<Milestone> result = SettingsForm.EntityService.Get<Milestone>(workspaceContext, queryPhrases, fields);
            return result;
        }

        // return all not done defects
        public static GroupResult GetAllDefectWithGroupBy(string releaseId)
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
            LogicalQueryPhrase doneMetaphasePhrase = new LogicalQueryPhrase("logical_name", "metaphase.work_item.done");
            NegativeQueryPhrase notDoneMetaphasePhrase = new NegativeQueryPhrase(doneMetaphasePhrase);
            CrossQueryPhrase phaseIdPhrase = new CrossQueryPhrase("metaphase", notDoneMetaphasePhrase);
            CrossQueryPhrase byPhasePhrase = new CrossQueryPhrase(WorkItem.PHASE_FIELD, phaseIdPhrase);
            queries.Add(byPhasePhrase);

            GroupResult result = SettingsForm.EntityService.GetWithGroupBy<WorkItem>(workspaceContext, queries, "severity");
            return result;
        }

        // return all not done user user stories
        public static GroupResult GetAllStoriesWithGroupBy(string releaseId)
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
            LogicalQueryPhrase doneMetaphasePhrase = new LogicalQueryPhrase("logical_name", "metaphase.work_item.done");
            NegativeQueryPhrase notDoneMetaphasePhrase = new NegativeQueryPhrase(doneMetaphasePhrase);
            CrossQueryPhrase phaseIdPhrase = new CrossQueryPhrase("metaphase", notDoneMetaphasePhrase);
            CrossQueryPhrase byPhasePhrase = new CrossQueryPhrase(WorkItem.PHASE_FIELD, phaseIdPhrase);
            queries.Add(byPhasePhrase);

            GroupResult result = SettingsForm.EntityService.GetWithGroupBy<WorkItem>(workspaceContext, queries, WorkItem.PHASE_FIELD);
            return result;
        }

    }
}
