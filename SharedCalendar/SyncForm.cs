using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hpe.Nga.Api.UI.Core.Configuration;

namespace SharedCalendar
{
  public partial class SyncForm : Form
  {
    private const int WM_NCHITTEST = 0x84;
    private const int HTCLIENT = 0x1;
    private const int HTCAPTION = 0x2;

    public SyncForm()
    {
      InitializeComponent();
      CenterToScreen();
    }

    ///
    /// Handling the window messages
    ///
    protected override void WndProc(ref Message message)
    {
      base.WndProc(ref message);

      if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
        message.Result = (IntPtr)HTCAPTION;
    }

    public void Init(ICollection<String> calendars, LoginConfiguration config)
    {
      lbSyncRelease.Text = String.Format(lbSyncRelease.Text, config.ReleaseName);

      foreach(String calendar in calendars) {
        cbCalendars.Items.Add(calendar);
      }

      cbCalendars.SelectedItem = config.CalendarName;
    }
    
    private void cbCalendars_SelectedIndexChanged(object sender, EventArgs e)
    {
      btnSync.Enabled = true;
    }

    public String SelectedCalendar 
    {
      get { return (String)cbCalendars.SelectedItem; }
    }

    private void closeImg_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
