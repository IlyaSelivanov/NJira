using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJira.Domain.Entities
{
    public class Cart
    {
        public Cart()
        {
            Issues = new List<Issue>();
        }

        public List<Issue> Issues { get; set; }

        public void AddItem(Issue issue)
        {
            Issue i = null;

            if (Issues.Count != 0)
            {
                i = Issues.Where(t => t.Key == issue.Key).FirstOrDefault();

                if (i == null)
                    Issues.Add(issue);
            }
            else
            {
                Issues.Add(issue);
            }
        }

        public void ClearCart()
        {
            Issues.Clear();
        }
    }
}
