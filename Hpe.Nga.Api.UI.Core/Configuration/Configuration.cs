// (c) Copyright 2016 Hewlett Packard Enterprise Development LP

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.

// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,

// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

using System;
using System.Collections.Generic;

namespace Hpe.Nga.Api.UI.Core.Configuration
{
    [CryptoProperties("password")]
    public class Configuration : BaseConfiguration
    {
        public static string SERVER_URL_FIELD = "serverUrl";
        public static string NAME_FIELD = "name";
        public static string PASSWORD_FIELD = "password";

        public static string SHARED_SPACE_ID_FIELD = "sharedSpaceId";
        public static string SHARED_SPACE_NAME_FIELD = "sharedSpaceName";

        public static string WORKSPACE_ID_FIELD = "workspaceId";
        public static string WORKSPACE_NAME_FIELD = "workspaceName";

        public static string RELEASE_ID_FIELD = "releaseId";
        public static string RELEASE_NAME_FIELD = "releaseName";

        public static string CALENDAR_NAME = "calendarName";


        #region Ctors

        public Configuration()
            : base()
        {
        }

        public Configuration(IDictionary<string, object> properties)
            : base(properties)
        {
        }


        public Configuration(String serverUrl, String name, String password)
        {
            ServerUrl = serverUrl;
            Name = name;
            Password = password;
        }

        #endregion

        #region Login Properties

        public string ServerUrl
        {
            get
            {
                return GetStringValue(SERVER_URL_FIELD);
            }
            set
            {
                SetValue(SERVER_URL_FIELD, value);
            }

        }

        public string Name
        {
            get
            {
                return GetStringValue(NAME_FIELD);
            }
            set
            {
                SetValue(NAME_FIELD, value);
            }

        }

        public string Password
        {
            get
            {
                return GetStringValue(PASSWORD_FIELD);
            }
            set
            {
                SetValue(PASSWORD_FIELD, value);
            }

        }


        #endregion

        #region Connect Properties

        private long GetIdAsLong(String propertyName)
        {
            object value = GetValue(propertyName);
            long lValue = 0;
            if (value != null)
            {
                if (value is long)
                {
                    lValue = (long)value;
                }
                else if (value is int)
                {
                    lValue = (int)value;
                }
                else if (value is String)
                {
                    lValue = long.Parse((String)value);
                }
            }
            return lValue;
        }

        public string SharedSpaceId
        {
            get
            {
                return GetStringValue(SHARED_SPACE_ID_FIELD);
            }
            set
            {
                SetValue(SHARED_SPACE_ID_FIELD, value);
            }

        }

        public string SharedSpaceName
        {
            get
            {
                return GetStringValue(SHARED_SPACE_NAME_FIELD);
            }
            set
            {
                SetValue(SHARED_SPACE_NAME_FIELD, value);
            }

        }


        public string WorkspaceId
        {
            get
            {
                return GetStringValue(WORKSPACE_ID_FIELD);
            }
            set
            {
                SetValue(WORKSPACE_ID_FIELD, value);
            }

        }

        public string WorkspaceName
        {
            get
            {
                return GetStringValue(WORKSPACE_NAME_FIELD);
            }
            set
            {
                SetValue(WORKSPACE_NAME_FIELD, value);
            }

        }

        public string ReleaseId
        {
            get
            {
                return GetStringValue(RELEASE_ID_FIELD);
            }
            set
            {
                SetValue(RELEASE_ID_FIELD, value);
            }

        }

        public string ReleaseName
        {
            get
            {
                return GetStringValue(RELEASE_NAME_FIELD);
            }
            set
            {
                SetValue(RELEASE_NAME_FIELD, value);
            }

        }

        public string CalendarName
        {
          get
          {
            return GetStringValue(CALENDAR_NAME);
          }
          set
          {
            SetValue(CALENDAR_NAME, value);
          }

        }

        #endregion


        public override string ToString()
        {
            return m_properties == null ? "No properties" : String.Format("Server :{0}, User {1} - {2}", ServerUrl, Name, Password);
        }
    }
}
