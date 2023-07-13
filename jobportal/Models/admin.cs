using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jobportal.Models
{
    public class admin
    {
        public Dictionary<string, string> adminlist()
        {
            Dictionary<string, string> admin =
            new Dictionary<string, string>();
            admin.Add("sandeep", "sandy");
            admin.Add("divyas", "divya");
            admin.Add("Kunalyadav", "kunaly");
            admin.Add("sri", "123");
            return (admin);
        }


    }
}