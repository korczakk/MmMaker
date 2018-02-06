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


        public async Task SaveData(FirebaseClient _client, List<ExcelContent> dataToSave)
        {
            //Usuwa poprzednie dane
            DeleteAll(_client).Wait();
            
            foreach (ExcelContent item in dataToSave)
            {
                FirebaseObject<ExcelContent> saved = await _client.Child("MM").Child("MMContent").PostAsync<ExcelContent>(item);

            }

        }



        /// <summary>
        /// Tworzy połączenie z bazą firebase
        /// </summary>
        /// <returns>Zwraca zadanie typu FirebaseClient</returns>
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

        /// <summary>
        /// Kasuje węzeł MMContent
        /// </summary>
        /// <param name="client">Połączenie z filrebase</param>
        /// <returns></returns>
        private async Task DeleteAll(FirebaseClient client)
        {
            await client.Child("MM").Child("MMContent").DeleteAsync();
        }
    }
}
