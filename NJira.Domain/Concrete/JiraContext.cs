using Atlassian.Jira;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJira.Domain.Concrete
{
    public class JiraSettings
    {
        public string Url = @"http://pm.quartsoft.com";
        public string User = "";
        public string Password = "";
    }

    public class JiraContext
    {
        private Jira jira;
        private JiraSettings jiraSettings = new JiraSettings();

        public JiraContext()
        {
            jira = Jira.CreateRestClient(jiraSettings.Url, jiraSettings.User, jiraSettings.Password);
            jira.MaxIssuesPerRequest = 1000;
        }

        public Jira Jira
        {
            get
            {
                if (jira == null)
                    InitJira();

                return jira;
            }
        }

        private void InitJira()
        {
            jira = Jira.CreateRestClient(jiraSettings.Url, jiraSettings.User, jiraSettings.Password);
            jira.MaxIssuesPerRequest = 1000;
        }
    }
}
