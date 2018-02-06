using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;
using MmMaker.Model;

namespace MmMaker.Service
{
    public class FirebaseConnector
    {


        public async void GetData(FirebaseClient _client)
        {
            //
            //await _client.Child("MM").PostAsync<ExcelContent>(new ExcelContent() {BarCode = 123 });

            var q = await _client.Child("MM").OnceAsync<ExcelContent>();

        }


        public async Task<FirebaseClient> Connect()
        {
            //wykonuje autentykacje
            string apiKey = "AIzaSyBaQegiosq-yCEp1CdNsZ6dGiAhQgN8fgw";

            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));

            FirebaseAuthLink authLink = await authProvider.SignInWithEmailAndPasswordAsync("kamil.korczak@gmail.com", "HPdj690P");


            FirebaseClient _client = new FirebaseClient("https://shoppinglist-dba72.firebaseio.com/", new FirebaseOptions()
            {
                AuthTokenAsyncFactory = () => Task.FromResult(authLink.FirebaseToken)
            });

            return _client;
        }

    }
}
