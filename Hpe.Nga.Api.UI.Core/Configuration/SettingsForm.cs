﻿// (c) Copyright 2016 Hewlett Packard Enterprise Development LP

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.

// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,

// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

using MicroFocus.Adm.Octane.Api.Core.Connector;
using MicroFocus.Adm.Octane.Api.Core.Entities;
using MicroFocus.Adm.Octane.Api.Core.Services;
using MicroFocus.Adm.Octane.Api.Core.Services.Query;
using MicroFocus.Adm.Octane.Api.Core.Services.RequestContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Hpe.Nga.Api.UI.Core.Configuration
{
    public partial class SettingsForm : Form
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool loadingConfiguration = false;

        private static RestConnector restConnector = new RestConnector();
        private static EntityService entityService = new EntityService(restConnector);

        public static EntityService EntityService
        {
            get
            {
                return entityService;
            }
        }

        public static RestConnector RestConnector
        {
            get
            {
                return restConnector;
            }
        }

        public SettingsForm()
        {
            InitializeComponent();
            OnLoginSettingsChanged(null, null);
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

        public Configuration Configuration
        {
            get
            {
                SharedSpace selectedSS = (SharedSpace)cmbSharedSpace.SelectedItem;
                Workspace selectedWorkspace = (Workspace)cmbWorkspace.SelectedItem;
                Release selectedRelease = (Release)cmbRelease.SelectedItem;

                Configuration conf = new Configuration(txtServer.Text, txtName.Text, txtPassword.Text);
                if (selectedSS != null)
                {
                    conf.SharedSpaceId = selectedSS.Id;
                    conf.SharedSpaceName = selectedSS.Name;
                }
                if (selectedWorkspace != null)
                {
                    conf.WorkspaceId = selectedWorkspace.Id;
                    conf.WorkspaceName = selectedWorkspace.Name;
                }
                if (selectedRelease != null)
                {
                    conf.ReleaseId = selectedRelease.Id;
                    conf.ReleaseName = selectedRelease.Name;
                }
                return conf;
            }
            set
            {
                if (value != null)
                {
                    loadingConfiguration = true;

                    txtServer.Text = value.ServerUrl;
                    txtName.Text = value.Name;
                    txtPassword.Text = value.Password;

                    if (value.SharedSpaceName != null)
                    {
                        SharedSpace ss = new SharedSpace();
                        ss.Name = value.SharedSpaceName;
                        ss.Id = value.SharedSpaceId;
                        cmbSharedSpace.Items.Clear();
                        cmbSharedSpace.Items.Add(ss);

                        cmbSharedSpace.SelectedItem = ss;
                    }
                    if (value.WorkspaceName != null)
                    {
                        Workspace workspace = new Workspace();
                        workspace.Name = value.WorkspaceName;
                        workspace.Id = value.WorkspaceId;
                        cmbWorkspace.Items.Clear();
                        cmbWorkspace.Items.Add(workspace);
                        cmbWorkspace.SelectedItem = workspace;
                    }
                    if (value.ReleaseName != null)
                    {
                        Release release = new Release();
                        release.Name = value.ReleaseName;
                        release.Id = value.ReleaseId;
                        cmbRelease.Items.Clear();
                        cmbRelease.Items.Add(release);
                        cmbRelease.SelectedItem = release;
                    }

                    loadingConfiguration = false;
                }
            }
        }

        private void OnLoginSettingsChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtServer.Text) || String.IsNullOrEmpty(txtName.Text) || String.IsNullOrEmpty(txtPassword.Text))
            {
                btnAuthenticate.Enabled = false;
            }
            else
            {
                btnAuthenticate.Enabled = true;
            }

            ClearConnectSettings();

            lblStatus.Text = "";
        }

        private void EnableLoginButton(bool enable)
        {
            //btnLogin.Enabled = enable;
            if (enable)
            {
                btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(169)))), ((int)(((byte)(130)))));
                btnLogin.ForeColor = System.Drawing.Color.White;
            }
            else
            {
                btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(228)))));
                btnLogin.ForeColor = System.Drawing.Color.LightGray;
            }
        }

        private void OnConnectSettingsChanged()
        {
            btnLogin.Enabled = cmbSharedSpace.SelectedItem != null && cmbWorkspace.SelectedItem != null && cmbRelease.SelectedItem != null;
        }

        private void ClearConnectSettings()
        {
            cmbSharedSpace.Items.Clear();
            cmbSharedSpace.Enabled = false;

            cmbWorkspace.Items.Clear();
            cmbWorkspace.Enabled = false;

            cmbRelease.Items.Clear();
            cmbRelease.Enabled = false;

            EnableLoginButton(false);
        }

        private void OnLoginClick(object sender, EventArgs args)
        {
            try
            {
                lblStatus.Text = "Authenticating ...";
                lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
                bool connected = restConnector.Connect(txtServer.Text, new UserPassConnectionInfo(txtName.Text, txtPassword.Text));
                if (connected)
                {
                    btnAuthenticate.Enabled = true;
                    Application.DoEvents();
                    lblStatus.Text = "";
                    LoadSharedSpaces();
                }
                else
                {
                    lblStatus.Text = "Failed to authenticate. Validate connection URL is in format http://<domain>:<port>";
                    lblStatus.ForeColor = Color.Red;
                }
            }
            catch (Exception e)
            {
                Exception innerException = e.InnerException ?? e;
                String exMessage = "";
                if (innerException.Message.Contains("401"))
                {
                    exMessage = "Failed to authenticate. Validate your username and password.";
                }
                else if (innerException.Message.Contains("404"))
                {
                    exMessage = "Failed to authenticate. Validate connection URL is in format http://<domain>:<port>";
                }
                else
                {
                    exMessage = "Failed to authenticate : " + innerException.Message;
                }
                lblStatus.Text = exMessage;
                lblStatus.ForeColor = Color.Red;
            }

        }

        private void LoadSharedSpaces()
        {
            EntityListResult<SharedSpace> sharedSpaces = null;
            try
            {
                sharedSpaces = EntityService.Get<SharedSpace>(new SiteContext());
            }
            catch (Exception)
            {

            }
            if (sharedSpaces == null)
            {
                SharedSpace defaultSharedSpace = new SharedSpace();
                defaultSharedSpace.Id = "1001";
                defaultSharedSpace.Name = "Default shared space";

                sharedSpaces = new EntityListResult<SharedSpace>();
                sharedSpaces.data = new List<SharedSpace>();
                sharedSpaces.data.Add(defaultSharedSpace);
                sharedSpaces.total_count = 1;
            }
            FillCombo(cmbSharedSpace, sharedSpaces.data);
        }

        private void FillCombo<T>(ComboBox combo, List<T> data) where T : BaseEntity
        {
            T selected = (T)combo.SelectedItem;
            T newSelected = null;
            combo.Items.Clear();

            List<T> ordered = data.OrderBy(en => en.Name).ToList();
            foreach (T item in ordered)
            {
                //fill combo
                combo.Items.Add(item);

                //find previously selected item
                if (selected != null && item.Id == selected.Id)
                {
                    newSelected = item;
                }
            }


            if (newSelected != null)
            {
                combo.SelectedItem = newSelected;
            }
            else if (data.Count > 0)
            {
                combo.SelectedItem = data[0];
            }

            combo.Enabled = true;
        }

        private void LoadWorkspaces(string sharedSpaceId)
        {
            try
            {
                SharedSpaceContext context = SharedSpaceContext.Create(sharedSpaceId);
                EntityListResult<Workspace> workspaces = EntityService.Get<Workspace>(context, null, null);
                FillCombo<Workspace>(cmbWorkspace, workspaces.data);
            }
            catch (Exception e)
            {
                lblStatus.Text = "Failed to load workspaces : " + e.Message;
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void LoadReleases(string sharedSpaceId, string workspaceId)
        {
            WorkspaceContext context = WorkspaceContext.Create(sharedSpaceId, workspaceId);
            EntityListResult<Release> result = EntityService.Get<Release>(context);

            if (result != null)
            {
                FillCombo<Release>(cmbRelease, result.data);

                EnableLoginButton(true);
            }
            else
            {
                ClearCombo(cmbRelease);
            }
        }

        private void ClearCombo(ComboBox combo)
        {
            combo.Items.Clear();
            combo.Enabled = false;
        }

        private void cmbSharedSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadingConfiguration)
            {
                return;
            }
            LoadWorkspaces(((SharedSpace)cmbSharedSpace.SelectedItem).Id);
            OnConnectSettingsChanged();
        }

        private void cmbWorkspace_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loadingConfiguration)
            {
                return;
            }

            LoadReleases(((SharedSpace)cmbSharedSpace.SelectedItem).Id, ((Workspace)cmbWorkspace.SelectedItem).Id);
            OnConnectSettingsChanged();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

        }

        private void closeImg_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
