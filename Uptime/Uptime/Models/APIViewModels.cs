using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using Uptime.Amazon;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Uptime.Models;

namespace Uptime.Models
{
    public class APIViewModels
    {
        // sona = kasutaja input, finalList = kõiki tulemusi hoidev list
        // finalList[0] - tiitlid, [1]- hinnad, [2] - rohkem pakkumisi URL, [3] - tootekirjelduse URL
        public string sona { get; set; }
        public List<List<List<string>>> finalList { get; set; }


        public List<List<List<string>>> keyword(string sona) { 
            ItemSearch search = new ItemSearch();
            search.AssociateTag = "nagerat-21";
            search.AWSAccessKeyId = "Amazoni key ID";
            ItemSearchRequest req = new ItemSearchRequest();
            req.SearchIndex = "All";
            req.Keywords = sona;
            req.Availability = ItemSearchRequestAvailability.Available;
            List<string> pageitem = new List<string>() { "1", "2", "3", "4", "5" };
            List<string> titleAlgList = new List<string>();
            List<string> priceAlgList = new List<string>();
            List<string> detailPageUrl= new List<string>();
            List<string> mediumImgUrl = new List<string>();
            List<string> moreOffersUrl = new List<string>();
            List<List<string>> titleAlgListPartial = new List<List<string>>();
            List<List<string>> imgAlgListPartial = new List<List<string>>();
            List<List<string>> priceAlgListPartial = new List<List<string>>();
            List<List<string>> detailPageUrlPartial = new List<List<string>>();
            List<List<string>> mediumImgUrlPartial = new List<List<string>>();
            List<List<string>> moreOffersUrlPartial = new List<List<string>>();
            List<List<List<string>>> tulemus = new List<List<List<string>>>();
            BasicHttpBinding httpbinding = new BasicHttpBinding() {MaxReceivedMessageSize= 131072 } ;

            for (int i = 0; i < pageitem.Count; i++)
            {
                req.ResponseGroup = new string[3] { "Images", "ItemAttributes", "Offers" };
                req.ItemPage = pageitem[i];
                search.Request = new ItemSearchRequest[] { req };

                AWSECommerceServicePortTypeClient amzwc = new AWSECommerceServicePortTypeClient();
                amzwc.ChannelFactory.Endpoint.EndpointBehaviors.Add(new AmazonSigningEndpointBehavior(
                    "Amazoni key ID", "Amazon Secrey key"));
                ItemSearchResponse resp = amzwc.ItemSearch(search);
                foreach (Item item in resp.Items[0].Item)
                {
                    try
                    {
                        titleAlgList.Add(item.ItemAttributes.Title);
                    }
                    catch (NullReferenceException)
                    {
                        titleAlgList.Add("Pealkiri puudub");
                    }

                    try
                    {
                        string algHind = item.OfferSummary.LowestNewPrice.Amount;
                        if (algHind.Count() < 2)
                        {
                            if (algHind == "0")
                            {
                                priceAlgList.Add(algHind);
                            }
                            string loppHind = algHind.Insert(0, "0.0");
                            priceAlgList.Add(loppHind);
                        }
                        else {
                            string loppHind = algHind.Insert(algHind.Count() - 2, ".");
                            if (loppHind[0].ToString() == ".")
                            {
                                loppHind = loppHind.Insert(0, "0");
                            }
                            priceAlgList.Add(loppHind);
                        }
                    }
                    catch (System.NullReferenceException)
                    {
                        priceAlgList.Add("0");
                    }
                    detailPageUrl.Add(item.DetailPageURL);
                    try
                    {
                        mediumImgUrl.Add(item.MediumImage.URL);
                    }
                    catch (System.NullReferenceException)
                    {
                        mediumImgUrl.Add("0");
                    }
                    try
                    {
                        moreOffersUrl.Add(item.Offers.MoreOffersUrl);
                    }
                    catch (System.NullReferenceException)
                    {
                        moreOffersUrl.Add("0");
                    }
                }
                titleAlgListPartial = ListExtensions.ChunkBy(titleAlgList, 13);
                priceAlgListPartial = ListExtensions.ChunkBy(priceAlgList, 13);
                detailPageUrlPartial = ListExtensions.ChunkBy(detailPageUrl, 13);
                mediumImgUrlPartial = ListExtensions.ChunkBy(mediumImgUrl, 13);
                moreOffersUrlPartial = ListExtensions.ChunkBy(moreOffersUrl, 13);

            }
            tulemus.Add(titleAlgListPartial);
            tulemus.Add(priceAlgListPartial);
            tulemus.Add(detailPageUrlPartial);
            tulemus.Add(mediumImgUrlPartial);
            tulemus.Add(moreOffersUrlPartial);
            return tulemus;
        }
        //otsene kutse HTML vormist, asendab vastava indexi listist vasta pealkirjaga/hinnaga/urliga
        public string innerTitle(List<List<List<string>>> tiitlid, int parameetrid, int koikTiitlid, int tiitel)
        {
            if (tiitlid[parameetrid][koikTiitlid][tiitel] != "0")
            {
                return tiitlid[parameetrid][koikTiitlid][tiitel];
            }
            else
            {
                return "Pealkiri puudub";
            }
        }

        public string innerPrice(List<List<List<string>>> hinnad, int parameetrid, int koikHinnad, int hind)
        {
            if (hinnad[parameetrid][koikHinnad][hind] != "0")
            {
                return hinnad[parameetrid][koikHinnad][hind];
            }
            else
            {
                return "N/A";
            }
        }

        public string detailPageUrl(List<List<List<string>>> urlid, int parameetrid, int koikUrlid, int url)
        {
            if (urlid[parameetrid][koikUrlid][url] != "0")
            {
                return urlid[parameetrid][koikUrlid][url];
            }
            else
            {
                return "N/A";
            }
        }

        public string mediumImgUrl(List<List<List<string>>> urlid, int parameetrid, int koikUrlid, int url)
        {
            if (urlid[parameetrid][koikUrlid][url] != "0")
            {
                return urlid[parameetrid][koikUrlid][url];
            }
            else
            {
                return "N/A";
            }
        }

        public string MoreOffersUrl(List<List<List<string>>> urlid, int parameetrid, int koikUrlid, int url)
        {
            if (urlid[parameetrid][koikUrlid][url] != "0")
            {
                return urlid[parameetrid][koikUrlid][url];
            }
            else
            {
                return "N/A";
            }
        }

    }
    // kuna algne list on List, mis koosneb kõikidest vastetest, lõikan ma listi 13-kaupa lahti
    public static class ListExtensions
    {
        public static List<List<string>> ChunkBy(List<string> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }

    public class AmazonSigningMessageInspector : IClientMessageInspector
        {
            private string accessKeyId = "";
            private string secretKey = "";

            public AmazonSigningMessageInspector(string accessKeyId, string secretKey)
            {
                this.accessKeyId = accessKeyId;
                this.secretKey = secretKey;
            }

            public Object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
            {
                string operation = Regex.Match(request.Headers.Action, "[^/]+$").ToString();
                DateTime now = DateTime.UtcNow;
                String timestamp = now.ToString("yyyy-MM-ddTHH:mm:ssZ");
                String signMe = operation + timestamp;
                Byte[] bytesToSign = Encoding.UTF8.GetBytes(signMe);

                Byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
                HMAC hmacSha256 = new HMACSHA256(secretKeyBytes);
                Byte[] hashBytes = hmacSha256.ComputeHash(bytesToSign);
                String signature = Convert.ToBase64String(hashBytes);

                request.Headers.Add(new AmazonHeader("AWSAccessKeyId", accessKeyId));
                request.Headers.Add(new AmazonHeader("Timestamp", timestamp));
                request.Headers.Add(new AmazonHeader("Signature", signature));
                return null;
            }

            void IClientMessageInspector.AfterReceiveReply(ref System.ServiceModel.Channels.Message Message, Object correlationState)
            {
            }
        }

        public class AmazonSigningEndpointBehavior : IEndpointBehavior
        {
            private string accessKeyId = "";
            private string secretKey = "";

            public AmazonSigningEndpointBehavior(string accessKeyId, string secretKey)
            {
                this.accessKeyId = accessKeyId;
                this.secretKey = secretKey;
            }

            public void ApplyClientBehavior(ServiceEndpoint serviceEndpoint, ClientRuntime clientRuntime)
            {
                clientRuntime.ClientMessageInspectors.Add(new AmazonSigningMessageInspector(accessKeyId, secretKey));
            }

            public void ApplyDispatchBehavior(ServiceEndpoint serviceEndpoint, EndpointDispatcher endpointDispatched)
            {
            }

            public void Validate(ServiceEndpoint serviceEndpoint)
            {
            }

            public void AddBindingParameters(ServiceEndpoint serviceEndpoint, BindingParameterCollection bindingParemeters)
            {
            }
        }

        public class AmazonHeader : MessageHeader
        {
            private string m_name;
            private string value;

            public AmazonHeader(string name, string value)
            {
                this.m_name = name;
                this.value = value;
            }

            public override string Name
            {
                get { return m_name; }
            }
            public override string Namespace
            {
                get { return "http://security.amazonaws.com/doc/2007-01-01/"; }
            }
            protected override void OnWriteHeaderContents(System.Xml.XmlDictionaryWriter writer, MessageVersion messageVersion)
            {
                writer.WriteString(value);
            }
        }

    
}