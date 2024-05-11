using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using AdminPanelBeta.ConnectHttp;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace AdminPanelBeta.Tests
{
    [TestClass]
    public class AuthorizationTests
    {
        private readonly HttpClient _client;

        public AuthorizationTests()
        {
            _client = new HttpClient();
        }

        [TestMethod]
        public async Task TestSuccessfulAuthorization()
        {
            // Arrange
            var credentials = new { tel = "000333000", password = "00000000" };
            string json = JsonConvert.SerializeObject(credentials);

            // Act
            HttpResponseMessage response = await _client.PostAsync(APIConfig.BaseUrl + "/authorization",
                new StringContent(json, Encoding.UTF8, "application/json"));

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            // Дополнительные проверки на перенаправление на главное меню
        }

        [TestMethod]
        public async Task TestAuthorizationWithIncorrectData()
        {
            // Arrange
            var credentials = new { tel = "000000000", password = "00000000" }; // Неправильные данные
            string json = JsonConvert.SerializeObject(credentials);

            // Act
            HttpResponseMessage response = await _client.PostAsync(APIConfig.BaseUrl + "/authorization",
                new StringContent(json, Encoding.UTF8, "application/json"));

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            // Дополнительные проверки на сообщение об ошибке
        }

        [TestMethod]
        public async Task TestSuccessfulEmployeeAddition()
        {
            // Arrange
            var newUser = new
            {
                name = "Иван",
                surname = "Иванов",
                birth = "1990-01-01",
                tel = "1234567890",
                password = "securePassword123"
            };

            string json = JsonConvert.SerializeObject(newUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _client.PostAsync(APIConfig.BaseUrl + "/registration", content);

            // Assert
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Дополнительные проверки на успешное добавление сотрудника
        }

        [TestMethod]
        public async Task TestEmployeeAdditionWithoutFullName()
        {
            // Arrange
            var newUser = new
            {
                tel = "1234567890",
                // Остальные обязательные поля не указаны
            };

            string json = JsonConvert.SerializeObject(newUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await _client.PostAsync(APIConfig.BaseUrl + "/registration", content);

            // Assert
            Assert.IsFalse(response.IsSuccessStatusCode);
            // Дополнительные проверки на сообщение об ошибке "ФИО сотрудника не указано"
        }
    }
}