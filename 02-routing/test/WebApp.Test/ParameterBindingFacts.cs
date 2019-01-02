using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace WebApp.Test
{
    public class ParameterBindingFacts : ApiFactsBase
    {
        [Fact]
        public async Task should_bind_to_hard_coded_uri()
        {
            HttpResponseMessage response = await Client.GetAsync("/api/binding/hardcoded");
            string content = await response.AssertStatusAndGetStringAsync();
            Assert.Equal("hardcoded", content);
        }

        [Fact]
        public async Task should_bind_to_query_string_for_hard_coded_template()
        {
            HttpResponseMessage response = await Client.GetAsync(
                "/api/binding/hardcoded-query?message=hi");
            string content = await response.AssertStatusAndGetStringAsync();
            Assert.Equal("HI", content);
        }

        [Fact]
        public async Task should_bind_to_query_string_with_auto_type_conversion()
        {
            HttpResponseMessage response = await Client.GetAsync(
                "/api/binding/hardcoded-query-integer?value=2");
            string content = await response.AssertStatusAndGetStringAsync();
            Assert.Equal("2", content);
        }
        
        [Fact]
        public async Task should_fail_if_query_auto_type_conversion_failed()
        {
            HttpResponseMessage response = await Client.GetAsync(
                "/api/binding/hardcoded-query-integer?value=not_a_number");
            
            // Note that the [ApiController] attribute contributes to the behavior.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        public async Task should_be_ok_if_query_parameter_is_omitted()
        {
            HttpResponseMessage response = await Client.GetAsync(
                "/api/binding/hardcoded-query-integer");

            string content = await response.AssertStatusAndGetStringAsync();
            
            Assert.Equal("0", content);
        }
        
        [Fact]
        public async Task should_fail_if_required_query_parameter_is_omitted()
        {
            HttpResponseMessage response = await Client.GetAsync(
                "/api/binding/hardcoded-required-query-integer");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}