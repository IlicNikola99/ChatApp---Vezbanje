using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public class Message
    {
        public Operation Operation { get; set; }
        public User user;
        public String Text { get; set; }
        public bool IsSuccessfull { get; set; }
        public override string ToString()
        {
            return user.Name + ": " + Text;
        }
    }
}
