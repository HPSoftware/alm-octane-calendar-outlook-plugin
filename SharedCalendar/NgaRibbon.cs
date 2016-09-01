using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Hpe.Nga.Api.Core.Connector;
using Hpe.Nga.Api.Core.Entities;
using Hpe.Nga.Api.Core.Services;
using Hpe.Nga.Api.Core.Services.RequestContext;
using Hpe.Nga.Api.UI.Core.Configuration;
using Microsoft.Office.Core;
using Microsoft.Office.Tools.Ribbon;
using SharedCalendar.Properties;
using Hpe.Nga.Api.Core.Services.GroupBy;
using Office = Microsoft.Office.Core;

// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new NgaRibbon();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace SharedCalendar
{
    [ComVisible(true)]
    public class NgaRibbon : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;
        ConfigurationPersistService persistService = new ConfigurationPersistService();
        LoginConfiguration loginConfig;
        private Boolean isLoggedIn = false;

        public NgaRibbon()
        {
          persistService.ConfigurationFileName = "SharedCalendar.configuration";
        }

        public Boolean GetIsLoggedIn(IRibbonControl control)
        {
          return isLoggedIn;
        }

        public String GetBtnConnectLable(IRibbonControl control)
        {
            if (isLoggedIn)
            {
                return "Disconnect";
            }
            else
            {
                return "Connect";
            }
        }

        public void OnLogin(Office.IRibbonControl control)
        {
          if (isLoggedIn)
          {
            // disconnect
            RestConnector.GetInstance().Disconnect();
            
            isLoggedIn = false;
          }
          else
          {
            // connect
            SettingsForm form = new SettingsForm();
            var config = persistService.Load<LoginConfiguration>();
            var calendarName = config.CalendarName;
            form.Configuration = config;

            if (form.ShowDialog() == DialogResult.OK)
            {
              loginConfig = form.Configuration;
              loginConfig.CalendarName = calendarName;

              //save last successful configuration
              persistService.Save(loginConfig);
              NgaUtils.init(loginConfig.SharedSpaceId, loginConfig.WorkspaceId, loginConfig.ReleaseId);
              isLoggedIn = true;
              
              // select the calendar tab 
              OutlookUtils.SelectCalenderTab();
            }
          }
          if (ribbon != null)
          {
            ribbon.Invalidate();
          }
        }

        public void OnSync(Office.IRibbonControl control)
        {
            try
            {
              SyncForm form = new SyncForm();
              var config = persistService.Load<LoginConfiguration>();
              // get calender list and initialize the form
              ICollection<String> calendars = OutlookUtils.GetCalendarList(config.CalendarName);
              form.Init(calendars, config);

              if (form.ShowDialog() == DialogResult.OK)
              {
                config.CalendarName = form.SelectedCalendar;
                persistService.Save(config);
                OutlookUtils.SelectedCalendarName = config.CalendarName;
                //Get by id
                Release release = NgaUtils.GetSelectedRelease(); //NgaUtils.GetReleaseById(releaseId);
                EntityListResult<Sprint> sprints = NgaUtils.GetSprintsByRelease(release.Id);
                OutlookSyncUtils.SyncSprintsToOutlook(config.CalendarName, release, sprints);

                EntityListResult<Milestone> milestones = NgaUtils.GetMilestonesByRelease(release.Id);
                OutlookSyncUtils.SyncMilestonesToOutlook(config.CalendarName, release, milestones);
                String str = String.Format("Sync completed successfully.{0}Summary : {1} sprints and {2} milestones.",
                    Environment.NewLine, sprints.data.Count, milestones.data.Count);
                MessageBox.Show(str, "Sync completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
              }
            }
            catch (Exception e)
            {
              String errorMsg = "Sync failed : " + e.Message + Environment.NewLine + e.StackTrace;
              MessageBox.Show(errorMsg, "Sync Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OnMailReport(Office.IRibbonControl control)
        {
            try
            {
                //Get by id
                Release release = NgaUtils.GetSelectedRelease(); //NgaUtils.GetReleaseById(releaseId);
                GroupResult groupResult = NgaUtils.GetAllDefectWithGroupBy(release.Id);
                GroupResult usGroupResult = NgaUtils.GetAllStoriesWithGroupBy(release.Id);
                OutlookSyncUtils.getReleaseMailReport(release, groupResult, usGroupResult);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to generate report: " + e.Message + Environment.NewLine + e.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Bitmap imageConnect_GetImage(IRibbonControl control)
        {
          if (isLoggedIn)
          {
            return Resources.disconnect;
          }
          return Resources.connect;
        }

        public Bitmap imageSync_GetImage(IRibbonControl control)
        {
          return Resources.sync;
        }

        public Bitmap imageReport_GetImage(IRibbonControl control)
        {
          return Resources.release_report;
        }
        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
          return GetResourceText("SharedCalendar.NgaRibbon.xml");
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, visit http://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion


    }
}
