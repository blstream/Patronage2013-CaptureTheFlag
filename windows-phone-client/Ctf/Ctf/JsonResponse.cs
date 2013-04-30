using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ctf
{
    class JsonResponse
    {
        //Login SUCCESS
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }

        //Registration SUCCESS/FAILED
        public int error_code { get; set; }

        //Login/Registration FAILED
        public string error { get; set; }
        public string error_description { get; set; }        
    }
}
