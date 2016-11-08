// (c) Copyright 2016 Hewlett Packard Enterprise Development LP

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.

// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,

// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

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

    public void Init(ICollection<String> calendars, Configuration config)
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
