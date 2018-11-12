// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.CognitiveServices.Search.LocalSearch.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the top-level object that the response includes when the
    /// request succeeds.
    /// </summary>
    public partial class SearchResponse : Response
    {
        /// <summary>
        /// Initializes a new instance of the SearchResponse class.
        /// </summary>
        public SearchResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the SearchResponse class.
        /// </summary>
        /// <param name="id">A String identifier.</param>
        /// <param name="readLink">The URL that returns this resource.</param>
        /// <param name="webSearchUrl">The URL to Bing's search result for this
        /// item.</param>
        /// <param name="queryContext">An object that contains the query string
        /// that Bing used for the request. This object contains the query
        /// string as entered by the user. It may also contain an altered query
        /// string that Bing used for the query if the query string contained a
        /// spelling mistake.</param>
        /// <param name="places">A list of local entities such as restaurants
        /// or hotels that are relevant to the query.</param>
        public SearchResponse(string id = default(string), string readLink = default(string), string webSearchUrl = default(string), IList<Action> potentialAction = default(IList<Action>), IList<Action> immediateAction = default(IList<Action>), string preferredClickthroughUrl = default(string), string adaptiveCard = default(string), QueryContext queryContext = default(QueryContext), Places places = default(Places), SearchResultsAnswer lottery = default(SearchResultsAnswer), double? searchResultsConfidenceScore = default(double?))
            : base(id, readLink, webSearchUrl, potentialAction, immediateAction, preferredClickthroughUrl, adaptiveCard)
        {
            QueryContext = queryContext;
            Places = places;
            Lottery = lottery;
            SearchResultsConfidenceScore = searchResultsConfidenceScore;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets an object that contains the query string that Bing used for
        /// the request. This object contains the query string as entered by
        /// the user. It may also contain an altered query string that Bing
        /// used for the query if the query string contained a spelling
        /// mistake.
        /// </summary>
        [JsonProperty(PropertyName = "queryContext")]
        public QueryContext QueryContext { get; private set; }

        /// <summary>
        /// Gets a list of local entities such as restaurants or hotels that
        /// are relevant to the query.
        /// </summary>
        [JsonProperty(PropertyName = "places")]
        public Places Places { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "lottery")]
        public SearchResultsAnswer Lottery { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "searchResultsConfidenceScore")]
        public double? SearchResultsConfidenceScore { get; private set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (QueryContext != null)
            {
                QueryContext.Validate();
            }
            if (Places != null)
            {
                Places.Validate();
            }
            if (Lottery != null)
            {
                Lottery.Validate();
            }
        }
    }
}
