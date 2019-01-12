using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
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
        
        [Fact]
        public async Task should_fail_if_required_query_parameter_is_not_valid()
        {
            HttpResponseMessage response = await Client.GetAsync(
                "/api/binding/hardcoded-validated-query-integer?text=a_very_long_text_that_fail");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task should_get_parameter_from_path_segment()
        {
            HttpResponseMessage response = await Client.GetAsync("/api/binding/path/user/12");
            
            Assert.Equal("12", await response.AssertStatusAndGetStringAsync());
        }
        
        [Fact]
        public async Task should_get_bad_request_if_path_segment_binding_failed()
        {
            HttpResponseMessage response = await Client.GetAsync("/api/binding/path/user/wtf");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task should_not_omit_segment_variable()
        {
            HttpResponseMessage response = await Client.GetAsync(
                "/api/binding/path/email");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [Fact]
        public async Task should_omit_optional_segment_variable()
        {
            HttpResponseMessage response = await Client.GetAsync(
                "/api/binding/path/optional");

            Assert.Equal("default", await response.AssertStatusAndGetStringAsync());
        }
        
        [Fact]
        public async Task should_omit_optional_segment_variable_without_default_value()
        {
            HttpResponseMessage response = await Client.GetAsync(
                "/api/binding/path/optional-no-default");

            Assert.Equal("(null value)", await response.AssertStatusAndGetStringAsync());
        }

        [Fact]
        public async Task should_apply_parameter_constraint()
        {
            // There are plenty of constraints. For a full-list, please check
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-2.2#route-constraint-reference
            
            HttpResponseMessage response = await Client.GetAsync(
                "/api/binding/path/constraint/user/123");
            HttpResponseMessage invalidArgResponse = await Client.GetAsync(
                "/api/binding/path/constraint/user/wtf");

            string s = await response.Content.ReadAsStringAsync();

            Assert.Equal("user: 123", await response.AssertStatusAndGetStringAsync());
            Assert.Equal(HttpStatusCode.NotFound, invalidArgResponse.StatusCode);
        }

        [Fact]
        public async Task should_apply_all_remaining_uri()
        {
            HttpResponseMessage response = await Client.GetAsync(
                "/api/binding/remaining-part/a/b/c?q=d");
            
            Assert.Equal(
                "arg: a/b/c", 
                await response.AssertStatusAndGetStringAsync());
        }

        [Fact]
        public async Task should_apply_all_remaining_uri_with_query_binding()
        {
            HttpResponseMessage response = await Client.GetAsync(
                "api/binding/remaining-part-with-query/a/b/c?name=hello");
            
            Assert.Equal(
                "arg: a/b/c, name: hello",
                await response.AssertStatusAndGetStringAsync());
        }
    }
}