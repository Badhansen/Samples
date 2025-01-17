﻿using ODataCoreFaq.Data;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ODataCoreFaq.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var httpClient = new HttpClient())
            {
                await httpClient.GetAsync("https://localhost:5001/api/FillDatabase");
            }

            var client = new ODataClient("https://localhost:5001/odata/");

            await BasicApi(client);
            await UntypedFluentApi(client);
            await TypedFluentClient(client);

            Console.ReadKey();
        }

        private static async Task TypedFluentClient(ODataClient client)
        {
            var customers = await client
                .For<Customer>()
                .Filter(x => x.CountryIsoCode == "AT")
                .FindEntriesAsync();
            foreach (var customer in customers)
            {
                Console.WriteLine(customer.CompanyName);
            }
        }

        private static async Task UntypedFluentApi(ODataClient client)
        {
            var x = ODataDynamic.Expression;
            IEnumerable<dynamic> customers = await client
                .For(x.Customers)
                .Filter(x.CountryIsoCode == "AT")
                .FindEntriesAsync();
            foreach (dynamic customer in customers)
            {
                Console.WriteLine(customer.CompanyName);
            }
        }

        private static async Task BasicApi(ODataClient client)
        {
            var customers = await client.FindEntriesAsync(
                "Customers?$filter=CountryIsoCode eq 'AT'");
            foreach (var customer in customers)
            {
                Console.WriteLine(customer["CompanyName"]);
            }
        }
    }
}
