Work with an up-to-date, shared calender that is synchronized with your ALM Octane releases, sprints and milestones. Send reports to stakeholders listing user stories and defects that are still open.

## Disclaimer
Certain versions of software accessible here may contain branding from Hewlett-Packard Company (now HP Inc.) and Hewlett Packard Enterprise Company.  As of September 1, 2017, the software is now offered by Micro Focus, a separately owned and operated company.  Any reference to the HP and Hewlett Packard Enterprise/HPE marks is historical in nature, and the HP and Hewlett Packard Enterprise/HPE marks are the property of their respective owners.


#### Known issues
- Failed to compile in VS2017  with exception : The “FindRibbons” task could not be loaded from the assembly
- The problem was mentioned in https://stackoverflow.com/a/30777648/1954871
You can try to install Microsoft.VisualStudio.Tools.Office.BuildTasks.dll in the GAC: "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.2 Tools\gacutil" /i "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\Microsoft\VisualStudio\v15.0\OfficeTools\Microsoft.VisualStudio.Tools.Office.BuildTasks.dll"
