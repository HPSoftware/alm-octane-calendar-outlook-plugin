using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;

namespace SharedCalendar
{
  public static class OutlookUtils
  {
    public static string MY_CALENDAR_DEFAULT_NAME = "Calendar";

    public static string APPOINTMENT_RELEASE_ID_FIELD = "ReleaseId";
    public static string APPOINTMENT_SPRINT_ID_FIELD = "SprintId";
    public static string APPOINTMENT_MILESTONE_ID_FIELD = "MilestoneId";
    public static IDictionary<String, MAPIFolder> m_calendars;

    public static String SelectedCalendarName;

    public static Application GetApplication()
    {
      Application outlookApp = new Microsoft.Office.Interop.Outlook.Application();
      return outlookApp;
    }

    public static Items GetAppointmentsInRange(String calendarName, DateTime startDate, DateTime endDate)
    {
      MAPIFolder calFolder = m_calendars[calendarName];
      string filter = "[Start] >= '" + startDate.ToString("g") + "' AND [End] <= '" + endDate.ToString("g") + "' AND [AllDayEvent] = True";
      Debug.WriteLine(filter);
      Items calItems = null;
      Items restrictItems = null;
      try
      {
        calItems = calFolder.Items;
        calItems.IncludeRecurrences = true;
        calItems.Sort("[Start]", Type.Missing);
        restrictItems = calItems.Restrict(filter);
        if (restrictItems.Count > 0)
        {
          return restrictItems;
        }
        return null;
      }
      catch (System.Exception)
      {
        if (restrictItems != null)
        {
          Marshal.ReleaseComObject(restrictItems);
        }
        return null;
      }
      finally
      {
        if (calItems != null)
        {
          Marshal.ReleaseComObject(calItems);
        }
      }
    }

    public static AppointmentItem AddAppointment(String subject, DateTime startDate, DateTime endDate, String categories, int reminderMinutesBeforeStart, Boolean reminderSet, Dictionary<String, Object> customFields, bool save)
    {
      var calendar = GetSyncCalendarFolder();
      AppointmentItem newAppointment = (AppointmentItem)calendar.Items.Add(OlItemType.olAppointmentItem);
      newAppointment.AllDayEvent = true;
      newAppointment.Start = startDate;
      newAppointment.End = endDate;
      newAppointment.Subject = subject;
      newAppointment.ReminderSet = reminderSet;

      // Add categories
      newAppointment.Categories = categories;

      // Add reminder
      if (reminderMinutesBeforeStart >= 0)
      {
        newAppointment.ReminderMinutesBeforeStart = reminderMinutesBeforeStart;
      }

      if (customFields != null)
      {
        foreach (KeyValuePair<String, object> keyValue in customFields)
        {
          newAppointment.UserProperties.Add(keyValue.Key, OlUserPropertyType.olText, true, Type.Missing);
          newAppointment.UserProperties[keyValue.Key].Value = keyValue.Value;
        }
      }

      if (save)
      {
        newAppointment.Save();
      }

      // todo set appointment category
      return newAppointment;
    }
    public static MailItem AddMailItem()
    {
      MailItem mailItem = (MailItem)GetApplication().CreateItem(OlItemType.olMailItem);
      mailItem.Save();
      return mailItem;
    }

    public static void SelectCalenderTab()
    {
      var activeExplorer = GetApplication().ActiveExplorer();
      activeExplorer.CurrentFolder = GetApplication().Session.GetDefaultFolder(OlDefaultFolders.olFolderCalendar);
    }

    internal static ICollection<String> GetCalendarList(String selectedCalendar)
    {
      Folder currentFolder = GetApplication().ActiveExplorer().CurrentFolder as Folder;
      m_calendars = new Dictionary<String, MAPIFolder>();
      var navPane = GetApplication().ActiveExplorer().NavigationPane;
      CalendarModule objModule = (CalendarModule)navPane.Modules.GetNavigationModule(OlNavigationModuleType.olModuleCalendar);

      // iterate over all groups and add the selected calendars to the list
      // add the previously selected calendar and make sure it's selected
      // don't include the default calendar 
      NavigationGroups objGroup = objModule.NavigationGroups;
      foreach (NavigationGroup navGroup in objGroup)
      {
        foreach (NavigationFolder navFolder in navGroup.NavigationFolders)
        {
          if (navFolder.DisplayName.Equals(MY_CALENDAR_DEFAULT_NAME))
          {
            continue;
          }
          var isSelectedCalendar = navFolder.DisplayName.Equals(selectedCalendar);
          if (navFolder.IsSelected || isSelectedCalendar)
          {
            navFolder.IsSelected = true;
            m_calendars.Add(navFolder.DisplayName, navFolder.Folder);
          }
        }
      }

      return m_calendars.Keys;
    }

    private static MAPIFolder GetSyncCalendarFolder()
    {
      return m_calendars[SelectedCalendarName];
    }
  }
}
