using Elasticsearch.Net;
using Nest;


const string IndexName = "estest";

var client = new ElasticClient("CLOUDID", new BasicAuthenticationCredentials("elastic", "password"));

var existsResponse = await client.Indices.ExistsAsync(IndexName);
