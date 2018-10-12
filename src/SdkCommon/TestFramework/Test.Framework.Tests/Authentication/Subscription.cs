using Microsoft.Azure;
using Microsoft.Azure.Test;
using Microsoft.Azure.Test.HttpRecorder;
using Spec.TestSupport.Test.Client;
using System;
using System.Net;
using Xunit;
using Xunit.Extensions;

namespace Spec.TestSupport.Test.Authentication
{
    public class Subscription : TestBase, IDisposable
    {
        private string TEST_CONNECTION_STRING;
        private string AZURE_TEST_MODE;
        private string TEST_ORGID_AUTHENTICATION;
        private string TEST_CSM_ORGID_AUTHENTICATION;

        public Subscription()
        {
            TEST_CONNECTION_STRING = Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING");
            AZURE_TEST_MODE = Environment.GetEnvironmentVariable("AZURE_TEST_MODE");
            TEST_ORGID_AUTHENTICATION = Environment.GetEnvironmentVariable("TEST_ORGID_AUTHENTICATION");
            TEST_CSM_ORGID_AUTHENTICATION = Environment.GetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION");
        }

        [Theory]
        [InlineData("Playback", "Environment=Prod")]
        [InlineData("Playback", "Environment=Current")]
        public void RdfeTestsWithAADAuth(string mode, string envString)
        {
            Environment.SetEnvironmentVariable("TEST_CONNECTION_STRING", "");
            Environment.SetEnvironmentVariable("AZURE_TEST_MODE", mode);
            Environment.SetEnvironmentVariable("TEST_ORGID_AUTHENTICATION", envString);
            using (UndoContext context = UndoContext.Current)
            {
                context.Start();
                var environmentFactory = new RDFETestEnvironmentFactory();
                var client = TestBase.GetServiceClient<SimpleClient>(environmentFactory);
                var response = client.RdfeCheckSbNamespace();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Theory]
        [InlineData("Playback", "")]
        [InlineData("Playback", "Environment=Prod")]
        [InlineData("Playback", "Environment=Current")]
        public void CsmTests(string mode, string envString)
        {
            Environment.SetEnvironmentVariable("TEST_CONNECTION_STRING", "");
            Environment.SetEnvironmentVariable("AZURE_TEST_MODE", mode);
            Environment.SetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION", envString);
            using (UndoContext context = UndoContext.Current)
            {
                context.Start();
                var environmentFactory = new CSMTestEnvironmentFactory();
                var client = TestBase.GetServiceClient<SimpleClient>(environmentFactory);
                var response = client.CsmGetLocation();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public void RdfeTestsWithCertAuthBaseuriInProd()
        {
            Environment.SetEnvironmentVariable("AZURE_TEST_MODE", "Playback");
            Environment.SetEnvironmentVariable("TEST_ORGID_AUTHENTICATION", "");
            Environment.SetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION", "");
            Environment.SetEnvironmentVariable("TEST_CONNECTION_STRING", "ManagementCertificate=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;"
                                                                       + "BaseUri=https://management.core.windows.net;SubscriptionId=2c224e7e-3ef5-431d-a57b-e71f4662e3a6");
            using (UndoContext context = UndoContext.Current)
            {
                context.Start();
                var environmentFactory = new RDFETestEnvironmentFactory();
                var client = TestBase.GetServiceClient<SimpleClient>(environmentFactory);
                Assert.Equal<Type>(typeof(CertificateCloudCredentials), client.Credentials.GetType());
                var response = client.RdfeCheckSbNamespace();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public void RdfeTestsWithCertAuthBaseUriInDogfood()
        {
            Environment.SetEnvironmentVariable("AZURE_TEST_MODE", "Playback");
            Environment.SetEnvironmentVariable("TEST_ORGID_AUTHENTICATION", "");
            Environment.SetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION", "");
            Environment.SetEnvironmentVariable("TEST_CONNECTION_STRING", "ManagementCertificate=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;"
                                                                       + "BaseUri=https://management-preview.core.windows-int.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a");
            using (UndoContext context = UndoContext.Current)
            {
                context.Start();
                var environmentFactory = new RDFETestEnvironmentFactory();
                var client = TestBase.GetServiceClient<SimpleClient>(environmentFactory);
                Assert.Equal<Type>(typeof(CertificateCloudCredentials), client.Credentials.GetType());
                var response = client.RdfeCheckSbNamespace();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public void TestsWithSubscriptionIdUsesValueFromTheConnectionString()
        {
            Environment.SetEnvironmentVariable("AZURE_TEST_MODE", "Playback");
            Environment.SetEnvironmentVariable("TEST_ORGID_AUTHENTICATION", "");
            Environment.SetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION", "");
            Environment.SetEnvironmentVariable("TEST_CONNECTION_STRING", "ManagementCertificate=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;"
                                                                       + "BaseUri=https://management-preview.core.windows-int.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72k");
            using (UndoContext context = UndoContext.Current)
            {
                context.Start();
                var environmentFactory = new RDFETestEnvironmentFactory();
                var client = TestBase.GetServiceClient<SimpleClient>(environmentFactory);
                Assert.Equal<Type>(typeof(CertificateCloudCredentials), client.Credentials.GetType());
                Assert.Equal<string>("ee39cb6d-d45b-4694-825a-f4d6f87ed72a", client.Credentials.SubscriptionId);
                var response = client.RdfeCheckSbNamespace();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public void TestsWithSubscriptionIdUsesValueFromTheTestFile()
        {
            Environment.SetEnvironmentVariable("AZURE_TEST_MODE", "Playback");
            Environment.SetEnvironmentVariable("TEST_ORGID_AUTHENTICATION", "Environment=Dogfood;SubscriptionId=0a496442-635f-4974-bda4-2d339b9a6b3b;UserId=admin@aad18.ccsctp.net;Password=Pa$$w0rd");
            Environment.SetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION", "");
            using (UndoContext context = UndoContext.Current)
            {
                context.Start();
                var environmentFactory = new RDFETestEnvironmentFactory();
                var client = TestBase.GetServiceClient<SimpleClient>(environmentFactory);
                Assert.Equal("0a496442-635f-4974-bda4-2d339b9a6b3c", client.Credentials.SubscriptionId);
                var response = client.RdfeCheckSbNamespace();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public void RdfeTestsWithCertAuthEnvironmentInDogfood()
        {
            Environment.SetEnvironmentVariable("AZURE_TEST_MODE", "Playback");
            Environment.SetEnvironmentVariable("TEST_ORGID_AUTHENTICATION", "");
            Environment.SetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION", "");
            Environment.SetEnvironmentVariable("TEST_CONNECTION_STRING", "ManagementCertificate=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;"
                                                                       + "Environment=Dogfood;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a");
            using (UndoContext context = UndoContext.Current)
            {
                context.Start();
                var environmentFactory = new RDFETestEnvironmentFactory();
                var client = TestBase.GetServiceClient<SimpleClient>(environmentFactory);
                Assert.Equal<Type>(typeof(CertificateCloudCredentials), client.Credentials.GetType());
                var response = client.RdfeCheckSbNamespace();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public void RdfeTestsWithCertAuthBaseuriEnvironmentInDogfood()
        {
            Environment.SetEnvironmentVariable("AZURE_TEST_MODE", "Playback");
            Environment.SetEnvironmentVariable("TEST_ORGID_AUTHENTICATION", "");
            Environment.SetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION", "");
            Environment.SetEnvironmentVariable("TEST_CONNECTION_STRING", "ManagementCertificate=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;"
                                                                       + "BaseUri=https://management-preview.core.windows-int.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a");
            using (UndoContext context = UndoContext.Current)
            {
                context.Start();
                var environmentFactory = new RDFETestEnvironmentFactory();
                var client = TestBase.GetServiceClient<SimpleClient>(environmentFactory);
                Assert.Equal<Type>(typeof(CertificateCloudCredentials), client.Credentials.GetType());
                var response = client.RdfeCheckSbNamespace();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Theory]
        [InlineData("BaseUri=https://management-preview.core.windows-int.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a;Environment=Dogfood")]
        [InlineData("GraphUri=https://management-preview.core.windows-int.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a;Environment=Dogfood")]
        [InlineData("GalleryUri=https://management-preview.core.windows-int.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a;Environment=Dogfood")]
        [InlineData("AADAuthEndpoint=https://management-preview.core.windows-int.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a;Environment=Dogfood")]
        [InlineData("GalleryUri=http://foo;AADAuthEndpoint=https://management-preview.core.windows-int.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a;Environment=Dogfood")]
        public void EnvironmentFactoryThrowsIfRdfeConnectionStringHasEnvironmentAndEndpoints(string connection)
        {
            Environment.SetEnvironmentVariable("TEST_CONNECTION_STRING", "");
            Environment.SetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION", "");
            Environment.SetEnvironmentVariable("TEST_ORGID_AUTHENTICATION", connection);
            Assert.Throws<ArgumentException>(() => new RDFETestEnvironmentFactory().GetTestEnvironment());
        }

        [Theory]
        [InlineData("BaseUri=https://management-preview.core.windows-int.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a;Environment=Dogfood")]
        [InlineData("GraphUri=https://management-preview.core.windows-int.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a;Environment=Dogfood")]
        [InlineData("GalleryUri=https://management-preview.core.windows-int.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a;Environment=Dogfood")]
        [InlineData("AADAuthEndpoint=https://management-preview.core.windows-int.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a;Environment=Dogfood")]
        [InlineData("GalleryUri=http://foo;AADAuthEndpoint=https://management-preview.core.windows-int.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a;Environment=Dogfood")]
        public void EnvironmentFactoryThrowsIfCsmConnectionStringHasEnvironmentAndEndpoints(string connection)
        {
            Environment.SetEnvironmentVariable("TEST_CONNECTION_STRING", "");
            Environment.SetEnvironmentVariable("TEST_ORGID_AUTHENTICATION", "");
            Environment.SetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION", connection);
            Assert.Throws<ArgumentException>(() => new CSMTestEnvironmentFactory().GetTestEnvironment());
        }

        [Fact]
        public void EnvironmentFactoryInCsmUsesBaseUriEndpointFromConnectionString()
        {
            HttpMockServer.Mode = HttpRecorderMode.Playback;
            Environment.SetEnvironmentVariable("TEST_CONNECTION_STRING", "");
            Environment.SetEnvironmentVariable("TEST_ORGID_AUTHENTICATION", "");
            Environment.SetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION", "BaseUri=https://foo.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a;RawToken=123");
            var environment = new CSMTestEnvironmentFactory().GetTestEnvironment();
            Assert.Equal("https://foo.net/", environment.BaseUri.ToString());
            Assert.Equal(TestEnvironment.EnvEndpoints[EnvironmentNames.Prod].GalleryUri, environment.Endpoints.GalleryUri);
        }

        [Fact]
        public void EnvironmentFactoryInCsmUsesEndpointFromConnectionString()
        {
            HttpMockServer.Mode = HttpRecorderMode.Playback;
            Environment.SetEnvironmentVariable("TEST_CONNECTION_STRING", "");
            Environment.SetEnvironmentVariable("TEST_ORGID_AUTHENTICATION", "");
            Environment.SetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION", "GraphUri=https://www.graph.net;BaseUri=https://foo.net;SubscriptionId=ee39cb6d-d45b-4694-825a-f4d6f87ed72a");
            var environment = new CSMTestEnvironmentFactory().GetTestEnvironment();
            Assert.Equal("https://foo.net/", environment.BaseUri.ToString());
            Assert.Equal(TestEnvironment.EnvEndpoints[EnvironmentNames.Prod].GalleryUri, environment.Endpoints.GalleryUri);
            Assert.Equal("https://www.graph.net/", environment.Endpoints.GraphUri.ToString());
        }

        
        public void Dispose()
        {
            Environment.SetEnvironmentVariable("TEST_CONNECTION_STRING", TEST_CONNECTION_STRING);
            Environment.SetEnvironmentVariable("AZURE_TEST_MODE", AZURE_TEST_MODE);
            Environment.SetEnvironmentVariable("TEST_ORGID_AUTHENTICATION", TEST_ORGID_AUTHENTICATION);
            Environment.SetEnvironmentVariable("TEST_CSM_ORGID_AUTHENTICATION", TEST_CSM_ORGID_AUTHENTICATION);
        }
    }
}
