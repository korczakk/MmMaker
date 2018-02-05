using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;

namespace MmMaker.Service
{
    public class FirebaseConnector
    {
        FirebaseClient _client;

        public FirebaseConnector()
        {
            string appKey = "shoppinglist-dba72";

            FirebaseAuthProvider auth = new FirebaseAuthProvider(appKey);

            _client = new FirebaseClient("", new FirebaseOptions() { });
        }
    }
}
