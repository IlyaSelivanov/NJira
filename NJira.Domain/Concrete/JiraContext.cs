using Atlassian.Jira;
using System;
using System.Linq;

namespace NJira.Domain.Concrete
{
    public class JiraSettings
    {
        public string Url = @"http://pm.quartsoft.com";
        public string User = "ilyaqs";
        public string Password = "kjkbnf23q";
    }

    public class JiraContext
    {
        private Jira jira;
        private JiraSettings jiraSettings = new JiraSettings();

        public JiraContext()
        {
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

        public IQueryable<Issue> Issues
        {
            get
            {
                if (jira == null)
                    InitJira();

                return jira.Issues.Queryable;
            }
        }

        private void InitJira()
        {
            jira = Jira.CreateRestClient(jiraSettings.Url, jiraSettings.User, jiraSettings.Password);
            jira.MaxIssuesPerRequest = 1000;
        }
    }
}
