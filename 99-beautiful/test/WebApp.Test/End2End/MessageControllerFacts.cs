using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApp.Test.Utils;
using WebApp.TestBase;
using Xunit;

namespace WebApp.Test.End2End
{
    public class MessageControllerFacts : WebAppFactBase<Startup>
    {
        [Fact]
        public async Task should_get_all_messages()
        {
            HttpResponseMessage response = await Client.GetAsync("message");
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            string content = await response.Content.ReadAsStringAsync();
            var deserialized = JsonConvert.DeserializeAnonymousType(
                content,
                new {text = default(string)}.AsArrayType());
            
            Assert.Equal(2, deserialized.Length);
            Assert.Equal("Hello", deserialized[0].text);
            Assert.Equal("World", deserialized[1].text);
        }
    }
}