// (c) Copyright 2016 Hewlett Packard Enterprise Development LP

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.

// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,

// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

using MicroFocus.Adm.Octane.Api.Core.Entities;
using MicroFocus.Adm.Octane.Api.Core.Services;
using MicroFocus.Adm.Octane.Api.Core.Services.GroupBy;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SharedCalendar
{
    public class OutlookSyncUtils
	{
		public static void SyncSprintsToOutlook(String calendarName, Release release, EntityListResult<Sprint> sprints)
		{

			//set sprint map
			Dictionary<string, Sprint> sprintMap = new Dictionary<string, Sprint>();
			foreach (Sprint sprint in sprints.data)
			{
				sprintMap[sprint.Id] = sprint;
			}

			//iterate outlook appointments
			Items resultItems = OutlookUtils.GetAppointmentsInRange(calendarName, new DateTime(2015, 1, 1), new DateTime(2030, 1, 1));
			foreach (AppointmentItem appointment in resultItems)
			{
				UserProperty releaseUP = appointment.UserProperties[OutlookUtils.APPOINTMENT_RELEASE_ID_FIELD];
				string appointmentReleaseId = (releaseUP == null ? null : releaseUP.Value);

				UserProperty sprintUP = appointment.UserProperties[OutlookUtils.APPOINTMENT_SPRINT_ID_FIELD];
				string appointmentSprintId = (sprintUP == null ? null : sprintUP.Value);

				if (!string.IsNullOrEmpty(appointmentReleaseId) && !string.IsNullOrEmpty(appointmentSprintId)) //sprint
				{
					if (sprintMap.ContainsKey(appointmentSprintId))
					{
						Sprint tempSprint = sprintMap[appointmentSprintId];
						sprintMap.Remove(appointmentSprintId);


						if (tempSprint != null)
						{
							SyncSprintToOutlook(tempSprint, appointment);
						}
					}
					else
					{
						//Delete a Sprint that no longer exist
						appointment.Delete();
					}
				}

				Marshal.ReleaseComObject(appointment);
			}

			//create sprints that were not deleted from map
			foreach (Sprint sprint in sprintMap.Values)
			{
				Dictionary<string, string> customFields = new Dictionary<string, string>();
				customFields.Add(OutlookUtils.APPOINTMENT_RELEASE_ID_FIELD, sprint.Release.Id);
				customFields[OutlookUtils.APPOINTMENT_SPRINT_ID_FIELD] = sprint.Id;
				String sprintName = getSprintAppointmentName(sprint);
				OutlookUtils.AddAppointment(calendarName, sprintName, sprint.StartDate, sprint.EndDate, "", 0, false, customFields, true);
			}

		}

		private static void SyncReleaseToOutlook(Release release, AppointmentItem appointment)
		{
			bool modified = false;
			if (appointment.Start.Date != release.StartDate.Date)
			{
				appointment.Start = release.StartDate;
				modified = true;
			}
			if (appointment.End.Date != release.EndDate.Date)
			{
				appointment.End = release.EndDate;
				modified = true;
			}
			if (!release.Name.Equals(appointment.Subject))
			{
				appointment.Subject = release.Name;
				modified = true;
			}
			if (modified)
			{
				appointment.Save();
			}
		}

		private static void SyncSprintToOutlook(Sprint sprint, AppointmentItem appointment)
		{
			bool modified = false;
			if (appointment.Start.Date != sprint.StartDate.Date)
			{
				appointment.Start = sprint.StartDate;
				modified = true;
			}
			if (appointment.End.Date != sprint.EndDate.Date)
			{
				appointment.End = sprint.EndDate;
				modified = true;
			}

			String sprintName = getSprintAppointmentName(sprint);
			if (!sprintName.Equals(appointment.Subject))
			{
				appointment.Subject = sprintName;
				modified = true;
			}
			if (modified)
			{
				appointment.Save();
			}
		}

		private static String getSprintAppointmentName(Sprint sprint)
		{
			return sprint.Release.Name + " " + sprint.Name;
		}

		public static void SyncMilestonesToOutlook(String calendarName, Release release, EntityListResult<Milestone> milestones)
		{
			//set sprint map
			Dictionary<string, Milestone> milestonesMap = new Dictionary<string, Milestone>();

			foreach (Milestone milestone in milestones.data)
			{
				milestonesMap[milestone.Id] = milestone;
			}

			//iterate outlook appointments
			Items resultItems = OutlookUtils.GetAppointmentsInRange(calendarName, new DateTime(2015, 1, 1), new DateTime(2030, 1, 1));
			foreach (AppointmentItem appointment in resultItems)
			{
				UserProperty releaseUP = appointment.UserProperties[OutlookUtils.APPOINTMENT_RELEASE_ID_FIELD];
				string appointmentReleaseId = (releaseUP == null ? null : releaseUP.Value);

				UserProperty milestoneUP = appointment.UserProperties[OutlookUtils.APPOINTMENT_MILESTONE_ID_FIELD];
				string appointmentMilestoneId = (milestoneUP == null ? null : milestoneUP.Value);

				if (!string.IsNullOrEmpty(appointmentReleaseId) && !string.IsNullOrEmpty(appointmentMilestoneId)) //milestone
				{
					if (milestonesMap.ContainsKey(appointmentMilestoneId))
					{
						Milestone tempMilestone = milestonesMap[appointmentMilestoneId];
						milestonesMap.Remove(appointmentMilestoneId);


						if (tempMilestone != null)
						{
							SyncMilestoneToOutlook(tempMilestone, appointment);
						}
					}
					else
					{
						//Delete a Milestone that no longer exist
						appointment.Delete();
					}
				}

				Marshal.ReleaseComObject(appointment);
			}

			//create milestones that were not deleted from map
			foreach (Milestone milestone in milestonesMap.Values)
			{
				Dictionary<string, string> customFields = new Dictionary<string, string>();
				customFields.Add(OutlookUtils.APPOINTMENT_RELEASE_ID_FIELD, milestone.Release.Id);
				customFields[OutlookUtils.APPOINTMENT_MILESTONE_ID_FIELD] = milestone.Id;
				String milestoneName = getMilestoneAppointmentName(milestone);
				MilestoneDataContainer msExtraData = getMilestoneData(milestone);

				OutlookUtils.AddAppointment(calendarName, milestoneName, milestone.GetStartDate(), milestone.GetEndDate(), msExtraData.Category, msExtraData.ReminderMinutesBeforeStart, msExtraData.ReminderSet, customFields, true);
			}
		}

		private static void SyncMilestoneToOutlook(Milestone milestone, AppointmentItem appointment)
		{
			bool modified = false;
			if ((appointment.Start.Date != milestone.GetStartDate().Date) || (appointment.End.Date != milestone.GetEndDate().Date))
			{
				appointment.Start = milestone.GetStartDate();
				appointment.End = milestone.GetEndDate();
				modified = true;
			}

			String milestoneName = getMilestoneAppointmentName(milestone);
			if (!milestoneName.Equals(appointment.Subject))
			{
				appointment.Subject = milestoneName;
				modified = true;
			}

			MilestoneDataContainer notificationData = getMilestoneData(milestone);

			if (notificationData.ReminderMinutesBeforeStart == 0 && appointment.ReminderSet)
			{
				appointment.ReminderSet = false;
				modified = true;
			}
			if (notificationData.ReminderMinutesBeforeStart != appointment.ReminderMinutesBeforeStart)
			{
				appointment.ReminderSet = true;
				appointment.ReminderMinutesBeforeStart = notificationData.ReminderMinutesBeforeStart;
				modified = true;
			}

			if (String.Compare(appointment.Categories, notificationData.Category) != 0)
			{
				appointment.Categories = notificationData.Category;
				modified = true;
			}

			if (modified)
			{
				appointment.Save();
			}
		}

		private static MilestoneDataContainer getMilestoneData(Milestone milestone)
		{
			MilestoneDataContainer dataContainer = new MilestoneDataContainer();
			String desc = milestone.Description;
			if (desc != null)
			{
				string[] lines = milestone.Description.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < lines.Length; i++)
				{
					string[] phrases = lines[i].Split('@');
					if (phrases.Length > 1)
					{
						dataContainer.AddPhrase(phrases[1]);
					}
				}
			}

			return dataContainer;
		}

		private static String getMilestoneAppointmentName(Milestone milestone)
		{
			return milestone.Release.Name + " " + milestone.Name;
		}
		public static void getReleaseMailReport(Release release, GroupResult groupResult, GroupResult usGroupResult)
		{
			MailItem mailItem = OutlookUtils.AddMailItem();
			mailItem.Subject = "Release Status: #" + release.Id + " - " + release.Name + " (" + release.StartDate.ToShortDateString() + " - " + release.EndDate.ToShortDateString() + ")";
			mailItem.BodyFormat = OlBodyFormat.olFormatHTML;
			String htmlBody = getHtmlTemplate();
			htmlBody = htmlBody.Replace("@releasename", (release.Name + " (" + release.StartDate.ToShortDateString() + " - " + release.EndDate.ToShortDateString() + ")"));
			htmlBody = htmlBody.Replace("@numofsprints", release.NumOfSprints.ToString());

			int totatDefects = 0;
			int maxWidth = 100;
			int maxVal = 1;
			Dictionary<string, int> map = new Dictionary<string, int>();
			map.Add("list_node.severity.urgent", 0);
			map.Add("list_node.severity.very_high", 0);
			map.Add("list_node.severity.high", 0);
			map.Add("list_node.severity.medium", 0);
			map.Add("list_node.severity.low", 0);
			map.Add("list_node.severity.empty", 0);
			foreach (Group group in groupResult.groups)
			{
				if (group.value == null)
				{
					map["list_node.severity.empty"] = group.count;
				}
				else
				{
					map[group.value.logical_name] = group.count;
				}

				maxVal = (maxVal < group.count ? group.count : maxVal);
				totatDefects += group.count;
			}
			htmlBody = htmlBody.Replace("@defecttotal", totatDefects.ToString());
			int urgentVal = ((int)(map["list_node.severity.urgent"] * maxWidth / maxVal));
			htmlBody = htmlBody.Replace("@urgentwidth", urgentVal.ToString());
			htmlBody = htmlBody.Replace("@urgentcol", ((urgentVal == 0) ? "none" : "red"));
			htmlBody = htmlBody.Replace("@urgentval", (map["list_node.severity.urgent"]).ToString());

			int veryhighVal = ((int)(map["list_node.severity.very_high"] * maxWidth / maxVal));
			htmlBody = htmlBody.Replace("@veryhighwidth", veryhighVal.ToString());
			htmlBody = htmlBody.Replace("@veryhighcol", ((veryhighVal == 0) ? "none" : "#D4522F"));
			htmlBody = htmlBody.Replace("@veryhighval", (map["list_node.severity.very_high"]).ToString());

			int highVal = ((int)(map["list_node.severity.high"] * maxWidth / maxVal));
			htmlBody = htmlBody.Replace("@highwidth", highVal.ToString());
			htmlBody = htmlBody.Replace("@highcol", ((highVal == 0) ? "none" : "#D97934"));
			htmlBody = htmlBody.Replace("@highval", (map["list_node.severity.high"]).ToString());

			int mediumVal = ((int)(map["list_node.severity.medium"] * maxWidth / maxVal));
			htmlBody = htmlBody.Replace("@mediumwidth", mediumVal.ToString());
			htmlBody = htmlBody.Replace("@mediumcol", ((mediumVal == 0) ? "none" : "#EDAE66"));
			htmlBody = htmlBody.Replace("@mediumval", (map["list_node.severity.medium"]).ToString());

			int lowVal = ((int)(map["list_node.severity.low"] * maxWidth / maxVal));
			htmlBody = htmlBody.Replace("@lowwidth", lowVal.ToString());
			htmlBody = htmlBody.Replace("@lowcol", ((lowVal == 0) ? "none" : "#E3CB44"));
			htmlBody = htmlBody.Replace("@lowval", (map["list_node.severity.low"]).ToString());

			int emptyVal = ((int)(map["list_node.severity.empty"] * maxWidth / maxVal));
			htmlBody = htmlBody.Replace("@emptywidth", emptyVal.ToString());
			htmlBody = htmlBody.Replace("@emptycol", ((emptyVal == 0) ? "none" : "#ffff00"));
			htmlBody = htmlBody.Replace("@emptyval", (map["list_node.severity.empty"]).ToString());

			StringBuilder storiesStrBuilder = new StringBuilder(" (");
			int totalStories = 0;
			bool isFirst = true;
			foreach (Group group in usGroupResult.groups)
			{
				if (isFirst)
				{
					isFirst = false;
				}
				else
				{
					storiesStrBuilder.Append(",  ");
				}
				storiesStrBuilder.Append(group.value.name + ": " + group.count);
				totalStories += group.count;
			}
			storiesStrBuilder.Append(")");
			htmlBody = htmlBody.Replace("@userstoriestotal", totalStories.ToString() + storiesStrBuilder.ToString());
			mailItem.HTMLBody = htmlBody;
			mailItem.Display();
		}
		private static String getHtmlTemplate()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string location = assembly.CodeBase;
			string directoryPath = Path.GetDirectoryName(new Uri(location).LocalPath);
			string fullPath = Path.Combine(directoryPath, "ReportTemplate.html");
			var htmlbody = File.ReadAllText(fullPath);
			return htmlbody;
		}

	}
}
