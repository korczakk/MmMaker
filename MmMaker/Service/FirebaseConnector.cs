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

        FirebaseClient _client;

        public async void GetData()
        {
            //
            //await _client.Child("MM").PostAsync<ExcelContent>(new ExcelContent() {BarCode = 123 });

            var q = await _client.Child("MM").OnceAsync<ExcelContent>();

        }


        public async Task Connect()
        {
            //wykonuje autentykacje
            string apiKey = "AIzaSyBaQegiosq-yCEp1CdNsZ6dGiAhQgN8fgw";

            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));

            FirebaseAuthLink authLink = await authProvider.SignInWithEmailAndPasswordAsync("kamil.korczak@gmail.com", "HPdj690P");


            _client = new FirebaseClient("https://shoppinglist-dba72.firebaseio.com/", new FirebaseOptions()
            {
                AuthTokenAsyncFactory = () => Task.FromResult(authLink.FirebaseToken)
            });


        }

    }
}
