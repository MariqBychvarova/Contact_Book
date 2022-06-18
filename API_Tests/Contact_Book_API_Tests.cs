using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace API_Tests
{
    public class Contact_Book_API_Tests
    {

        private RestClient client;
        string url = "https://contactbook.nakov.repl.co/api";

        [SetUp]
        public void Setup()
        {
            client = new RestClient();
        }

        [Test]
        public void Test_First_Contact()
        {
            RestRequest request = new RestRequest($"{url}/contacts");
            RestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            List<Contact> contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);

            Contact firstContact = contacts[0];

            Assert.AreEqual("Steve", firstContact.firstName);
            Assert.AreEqual("Jobs", firstContact.lastName);
        }

        [Test]
        public void Test_Find_Contact_By_KeyWord()
        {
            string keyword = "albert";
            RestRequest request = new RestRequest($"{url}/contacts/search/{keyword}");
            RestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            List<Contact> contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);
            Assert.That(contacts.Count > 0);
            Contact contact = contacts[0];

            Assert.AreEqual("Albert", contact.firstName);
            Assert.AreEqual("Einstein", contact.lastName);
        }

        [Test]
        public void Test_Invalid_Search()
        {
            string keyword = "missing12345";
            RestRequest request = new RestRequest($"{url}/contacts/search/{keyword}");
            RestResponse response = client.Execute(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            List<Contact> contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);
            Assert.That(contacts.Count == 0);
        }

        [Test]
        public void Test_Create_Contact_Invalid_Data()
        {
            RestRequest request = new RestRequest($"{url}/contacts");
            string firstName = string.Empty;
            string lastName = "Ivanov";

            request.AddBody(new { firstName, lastName });
            RestResponse response = client.Execute(request, Method.Post);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);            
        }

        [Test]
        public void Test_Create_Contact_Valid_Data()
        {
            RestRequest request = new RestRequest($"{url}/contacts");
            string firstName = "Ivan" + DateTime.Now.Ticks;
            string lastName = "Ivanov"+DateTime.Now.Ticks;
            string email = $"{firstName}@{lastName}.com";
            string phone = $"{DateTime.Now.Ticks}";

           var body = new 
            {
                firstName,
                lastName,
                email,
                phone
            };

            request.AddJsonBody(body);
            RestResponse response = client.Execute(request, Method.Post);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            request = new RestRequest($"{url}/contacts");
            response = client.Execute(request);

            List<Contact> contacts = JsonSerializer.Deserialize<List<Contact>>(response.Content);
            Contact contact = contacts.Last();

            Assert.AreEqual(firstName, contact.firstName);
            Assert.AreEqual(lastName, contact.lastName);
        }
    }
}