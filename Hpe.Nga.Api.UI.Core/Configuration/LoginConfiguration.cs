using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hpe.Nga.Api.Core.Services.Core;

namespace Hpe.Nga.Api.UI.Core.Configuration
{
    [CryptoProperties("password")]
    public class LoginConfiguration : BaseConfiguration
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

        public LoginConfiguration()
            : base()
        {
        }

        public LoginConfiguration(IDictionary<string, object> properties)
            : base(properties)
        {
        }


        public LoginConfiguration(String serverUrl, String name, String password)
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

        public long SharedSpaceId
        {
            get
            {
                return GetIdAsLong(SHARED_SPACE_ID_FIELD);
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


        public long WorkspaceId
        {
            get
            {
                return GetIdAsLong(WORKSPACE_ID_FIELD);
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

        public long ReleaseId
        {
            get
            {
                return GetIdAsLong(RELEASE_ID_FIELD);
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
